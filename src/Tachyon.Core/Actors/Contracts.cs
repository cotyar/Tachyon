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
    /// Addressable interface allows to send signal to any actor.
    /// </summary>
    public interface IAddressable : IComparable<IAddressable>, IEquatable<IAddressable>, IConsistentlyHashable
    {
    }

    /// <summary>
    /// A reference to an addressable resource accessible in distributed ecosystem:
    /// an actor, stream, key-value store entry etc.
    /// </summary>
    /// <remarks>
    /// Do not confuse this with IVar data type from Concurrent ML.
    /// </remarks>
    /// <typeparam name="M"></typeparam>
    public interface IVar<in M> : IAddressable
    {
        IVar<N> Narrow<N>() where N : M;
    }
    
    public interface ITimer : IDisposable
    {
        void Schedule<M>(TimeSpan delay, IVar<M> target, M message, CancellationToken token = default(CancellationToken));
        void Schedule<M>(TimeSpan delay, TimeSpan interval, IVar<M> target, M message, CancellationToken token = default(CancellationToken));
    }

    public interface ICell : IAsyncDisposable
    {
        IAddressable Self { get; }
        IActorRuntime Runtime { get; }
        ValueTask<Unit> Stop();
        void Unhandled(object messageOrSignal);
        void Send(ISignal signal); //TODO: use custom awaiter to release thread control i.e. when mailbox is full
    }

    /// <summary>
    /// A typed version of <see cref="ICell"/>.
    /// </summary>
    /// <typeparam name="S">Type of the associated actor's state.</typeparam>
    /// <typeparam name="M">Type of the associated actor's protocol messages.</typeparam>
    public interface ICell<S, M> : ICell
    {
        new IVar<M> Self { get; }
        S State { get; }
        void Send(M message); //TODO: use custom awaiter to release thread control i.e. when mailbox is full
    }
    
    public interface IActorRuntime : IAsyncDisposable, IHostedService
    {
        ITimer Timer { get; }
        TaskScheduler TaskScheduler { get; }
        IVar<DeadLetter> DeadLetters { get; }

        Task StartAsync(CancellationToken token = default(CancellationToken));
    }
}