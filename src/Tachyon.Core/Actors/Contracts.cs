#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Contracts.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Tachyon.Core;
using Tachyon.Core.Actors.Mailboxes;

namespace Tachyon.Actors
{
    /// <summary>
    /// Messages marked as silient will not be send to dead letters when unhandled.
    /// </summary>
    public interface ISilient { }

    public sealed class DeadLetter : IEquatable<DeadLetter>
    {
        public IAddressable Receiver { get; }
        public object Message { get; }

        public DeadLetter(IAddressable receiver, object message)
        {
            Receiver = receiver;
            Message = message;
        }

        public bool Equals(DeadLetter other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return Equals(Receiver, other.Receiver) && Equals(Message, other.Message);
        }

        public override int GetHashCode()
        {
            var hashCode = -2084569196;
            hashCode = hashCode * -1521134295 + EqualityComparer<IAddressable>.Default.GetHashCode(Receiver);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Message);
            return hashCode;
        }

        public override bool Equals(object obj) => obj is DeadLetter deadLetter && Equals(deadLetter);

        public override string ToString() => $"DeadLetter(receiver: {Receiver}, message: {Message})";
    }

    /// <summary>
    /// Signals are system messages. They are used to control actor's lifecycle and
    /// to inform about system-wide events. Signals are handled with higher priority
    /// than user-defined messages.
    /// </summary>
    public interface ISignal : ISilient { }

    /// <summary>
    /// Denotes a message that can carry metadata necessary to track a call graph for a givent request.
    /// </summary>
    public interface ITraceable
    {
        /// <summary>
        /// An original message emitter. In case when we have a tree-like graph request chain,
        /// this value allows to identify it.
        /// </summary>
        long CausationId { get; }

        /// <summary>
        /// A correlation id which may be used to correlate a request message and a response
        /// for that message.
        /// </summary>
        long CorrelationId { get; }
    }

    /// <summary>
    /// Predefined kind of message, used mostly for request-response pattern.
    /// </summary>
    /// <typeparam name="Reply"></typeparam>
    public interface IRequest<Reply>
    {
        /// <summary>
        /// An expected received for a reply message for a given request.
        /// </summary>
        Var<Reply> ReplyTo { get; }
    }

    /// <summary>
    /// Defines a namespace addressation rules for a given <see cref="IAddressable"/>.
    /// </summary>
    public enum KeyspaceType : byte
    {
        Local = 1,
        Global = 2
    }

    /// <summary>
    /// Addressable interface allows to send signal to any actor.
    /// </summary>
    public interface IAddressable : IComparable<IAddressable>, IEquatable<IAddressable>, IConsistentlyHashable
    {
        KeyspaceType Keyspace { get; }
        ReadOnlySpan<byte> Key { get; }
    }
    
    /// <summary>
    /// Timers allow to schedule delayed or repeated messages. Unlike <see cref="IReminder"/>s,
    /// <see cref="ITimer"/> is expected to have no underlying durable state backend. Therefore
    /// messages scheduled with it should not work with prolonged periods of time.
    /// </summary>
    /// <seealso cref="IReminder"/>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Schedules a <paramref name="message"/> to be send to a given <paramref name="target"/>
        /// after a brief <paramref name="delay"/>. Scheduled message send can be cancelled by
        /// attaching a <paramref name="token"/>.
        /// </summary>
        /// <remarks>
        /// Keep in mind that the state of <see cref="ITimer"/> is by default in-memory, which means
        /// it's not reliable for a longer periods of time. Therefore it's not safe to set a
        /// <paramref name="delay"/> for longer than minutes.
        /// </remarks>
        void Schedule<M>(TimeSpan delay, Var<M> target, M message, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Schedules a <paramref name="message"/> to be send to a given <paramref name="target"/>
        /// in a given <paramref name="interval"/>, after an initial <paramref name="delay"/>.
        /// Scheduled message send can be cancelled by attaching a <paramref name="token"/>.
        /// </summary>
        /// <remarks>
        /// Keep in mind that the state of <see cref="ITimer"/> is by default in-memory, which means
        /// it's not reliable for a longer periods of time. Therefore it's not safe to set a
        /// <paramref name="delay"/> for longer than minutes.
        /// </remarks>
        void Schedule<M>(TimeSpan delay, TimeSpan interval, Var<M> target, M message, CancellationToken token = default(CancellationToken));
    }

    /// <summary>
    /// A long running scheduler, which state should be backed up by a persistent storage.
    /// Because of that it shouldn't be used to send messages with high frequency or in short
    /// periods of time - a <see cref="ITimer"/> is an interface dedicated to it.
    /// </summary>
    /// <seealso cref="ITimer"/>
    public interface IReminder : IDisposable
    {
        /// <summary>
        /// Schedules a <paramref name="message"/> to be send to a given <paramref name="target"/>.
        /// Scheduled message will be triggered at a date provided by <paramref name="fireAt"/>
        /// parameter.
        ///
        /// This schedule requires a unique <paramref name="key"/> to be provided in order to mark
        /// and possibly <see cref="Cancel"/> this schedule instance. If this key was used before,
        /// a new schedule will override it.
        /// </summary>
        /// <returns>A task which completes after schedule has been successfully persisted.</returns>
        Task Schedule<M>(string key, DateTime fireAt, Var<M> target, M message);

        /// <summary>
        /// Cancels a previously <see cref="Schedule{M}"/> message send request using provided
        /// <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Key used previously to schedule a send request.</param>
        /// <returns>
        /// A task which completes after cancellation has been acknowledges by the persistend
        /// backend. Returns a true if scheduled request was cancelled successfully or false
        /// if it didn't existed or has fired already.
        /// </returns>
        Task<bool> Cancel(string key);
    }

    /// <summary>
    /// Interface which enables a message passing capabilities. Usually caried by
    /// <see cref="ICell"/> or <see cref="IActorRuntime"/>.
    /// </summary>
    public interface IMessageEmitter
    {
        /// <summary>
        /// A fire-and-forget way to send a <paramref name="message"/> to a given
        /// <paramref name="target"/>.
        /// </summary>
        /// <remarks>
        /// Do not confuse this method with request-response or ACK semantics. An awaiter
        /// will NOT wait for a recipient to complete the message or return any response.
        /// </remarks>
        /// <typeparam name="M">Type of a message to send.</typeparam>
        /// <param name="target">An identifier of a message recipient.</param>
        /// <param name="message">A message to be send.</param>
        /// <returns>
        /// An awaiter, which completes immediatelly if message was put on top of mailbox
        /// (in case of local actor) or accepted for remote layer (in case of remote one),
        /// or yields when a mailbox is full or remote connection was not established.
        /// </returns>
        MailboxAwaiter Send<M>(Var<M> target, M message);

        /// <summary>
        /// A fire-and-forget way to send a <paramref name="message"/> to a given
        /// <paramref name="target"/>.
        ///
        /// A returned value informs if message was emitted successfully, but it doesn't
        /// inform if it was received or not. If case if it'll return false, a message
        /// may need to be send again.
        /// </summary>
        /// <typeparam name="M">Type of a message to send.</typeparam>
        /// <param name="target">An identifier of a message recipient.</param>
        /// <param name="message">A message to be send.</param>
        /// <returns>
        /// True if message was successfully put into receiver's mailbox or accepted by
        /// remote layer to be send. False, if mailbox is full (message was not put in
        /// this case) or when remote layer is unresponsive.
        /// </returns>
        bool TrySend<M>(Var<M> target, M message);

        /// <summary>
        /// A fire-and-forget way to send a <paramref name="signal"/> to a given
        /// <paramref name="target"/>. Signals have higher priority than messages and
        /// will always be picked first from actor's mailbox.
        /// </summary>
        /// <remarks>
        /// Do not confuse this method with request-response or ACK semantics. An awaiter
        /// will NOT wait for a recipient to complete the message or return any response.
        /// </remarks>
        /// <param name="target">A signal recipient.</param>
        /// <param name="signal">
        /// Signal informing about system operation concerning target actor.
        /// </param>
        /// <returns>
        /// An awaiter, which completes immediatelly if signal was put on top of mailbox
        /// (in case of local actor) or accepted for remote layer (in case of remote one),
        /// or yields when a mailbox is full or remote connection was not established.
        /// </returns>
        MailboxAwaiter Signal(IAddressable target, ISignal signal);

        /// <summary>
        /// A fire-and-forget way to send a <paramref name="signal"/> to a given
        /// <paramref name="target"/>. Signals have higher priority than messages and
        /// will always be picked first from actor's mailbox.
        ///
        /// A returned value informs if message was emitted successfully, but it doesn't
        /// inform if it was received or not. If case if it'll return false, a message
        /// may need to be send again.
        /// </summary>
        /// <param name="target">A signal recipient.</param>
        /// <param name="signal">
        /// Signal informing about system operation concerning target actor.
        /// </param>
        /// <returns>
        /// True if signal was successfully put into receiver's mailbox or accepted by
        /// remote layer to be send. False, if mailbox is full (message was not put in
        /// this case) or when remote layer is unresponsive.
        /// </returns>
        bool TrySignal(IAddressable target, ISignal signal);
    }

    public interface ICell : IMessageEmitter, IAsyncDisposable
    {
        /// <summary>
        /// Self variable reference for a current actor cell.
        /// </summary>
        IAddressable Self { get; }

        /// <summary>
        /// An actor runtime, in context of which current actor is living in.
        /// </summary>
        IActorRuntime Runtime { get; }
        ValueTask<Unit> Stop();
    }

    /// <summary>
    /// A typed version of <see cref="ICell"/>.
    /// </summary>
    /// <typeparam name="S">Type of the associated actor's state.</typeparam>
    /// <typeparam name="M">Type of the associated actor's protocol messages.</typeparam>
    public interface ICell<S, M> : ICell
    {
        /// <summary>
        /// Self variable reference for a current actor cell.
        /// </summary>
        new Var<M> Self { get; }

        /// <summary>
        /// Actor cell embedded state.
        /// </summary>
        S State { get; }
    }

    /// <summary>
    /// A a provider for a <see cref="System.Random"/> value generator. Used
    /// for simulation environment.
    /// </summary>
    public interface IRandomGenerator
    {
        /// <summary>
        /// Returns a initialized <see cref="System.Random"/> object.
        /// This property is expected to be thread safe, however do not
        /// store it in separate variable to be used from different thread.
        /// </summary>
        Random Random { get; }
    }

    /// <summary>
    /// A system <see cref="DateTime"/> provider. 
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Returns a current <see cref="DateTime"/> in a UTC time zone.
        /// 
        /// If you want to use simulation environment use this provider instead of calling <see cref="DateTime.UtcNow"/>.
        /// </summary>
        /// <returns></returns>
        DateTime UtcNow();
    }
    
    public interface IActorRuntime : IAsyncDisposable, IHostedService, IRandomGenerator, IClock, ITimer, IReminder
    {
        TaskScheduler TaskScheduler { get; }
        Var<DeadLetter> DeadLetters { get; }

        Task StartAsync(CancellationToken token = default(CancellationToken));
    }
}