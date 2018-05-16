#region copyright
// -----------------------------------------------------------------------
//  <copyright file="MessagingSemantics.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    /// <typeparam name="R">Type of reply message.</typeparam>
    public interface IRequest<R>
    {
        /// <summary>
        /// An expected received for a reply message for a given request.
        /// </summary>
        Var<R> ReplyTo { get; }
    }

    /// <summary>
    /// Deliverable message is expected to conform to at-least-once delivery semantics, possibly
    /// being delivered multiple times, until acknowledged by the recipient.
    /// </summary>
    /// <typeparam name="R">Type of reply message.</typeparam>
    public interface IDeliverable<R> : IRequest<R>, ITraceable where R: ITraceable
    {
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

}