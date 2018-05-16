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
        void Schedule<M>(TimeSpan delay, Var<IChannel<M>> target, M message, CancellationToken token = default(CancellationToken));

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
        void Schedule<M>(TimeSpan delay, TimeSpan interval, Var<IChannel<M>> target, M message, CancellationToken token = default(CancellationToken));
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
        Task Schedule<M>(string key, DateTime fireAt, Var<IChannel<M>> target, M message);

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
        new Var<IChannel<M>> Self { get; }

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
        Var<IChannel<DeadLetter>> DeadLetters { get; }

        Task StartAsync(CancellationToken token = default(CancellationToken));
    }
}