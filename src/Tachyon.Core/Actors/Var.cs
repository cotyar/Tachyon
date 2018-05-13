#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Var.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;
using Tachyon.Core;
using Tachyon.Core.Actors.Mailboxes;

namespace Tachyon.Actors
{
    /// <summary>
    /// A reference to an addressable resource accessible in distributed ecosystem:
    /// an actor, stream, key-value store entry etc.
    /// </summary>
    /// <remarks>
    /// Do not confuse this with IVar data type from Concurrent ML.
    /// </remarks>
    /// <typeparam name="M"></typeparam>
    [Immutable]
    public sealed class Var<M> : IAddressable
    {
        private readonly int hash;
        private readonly string key;

        /// <summary>
        /// Instead of sending the message through actor runtime every time,
        /// let's cache the direct pipe to a corresponding channel to either
        /// local or global resource and use it directly in subsequent tries.
        /// </summary>
        private IChannel<M> channel;

        public Var([NotNull]string key)
        {
            this.key = key;
            this.hash = Murmur.Hash(key);
        }

        public string Key => key;

        internal void Send(M message, IActorRuntime runtime)
        {
            if (channel == null || channel.IsDisposed)
            {
                //TODO: retrieve channel from the runtime
            }

            channel.Send(message);
        }

        internal void Signal(ISignal signal, IActorRuntime runtime)
        {
            if (channel == null || channel.IsDisposed)
            {
                //TODO: retrieve channel from the runtime
            }

            channel.Signal(signal);
        }

        public Var<N> Narrow<N>() where N : M => new Var<N>(this.Key);

        public int CompareTo(IAddressable other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(other, null)) return 1;

            return string.Compare(Key, other.Key, StringComparison.InvariantCulture);
        }

        public bool Equals(IAddressable other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Key == other.Key;
        }

        public int GetConsistentHash() => hash;

        public override bool Equals(object obj) => 
            obj is IAddressable addressable && Equals(addressable);

        public override int GetHashCode() => hash;

        public override string ToString() => $"<{key}>";
    }

    interface IChannel<M> : IDisposable
    {
        bool IsDisposed { get; }
        void Send<M>(M message);
        void Signal(ISignal signal);
    }
}