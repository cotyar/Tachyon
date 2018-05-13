#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Var.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using JetBrains.Annotations;
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
    public sealed class Var<M>
    {
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

        public Var<N> Narrow<N>() where N : M
        {
            throw new System.NotImplementedException();
        }

        public int CompareTo(IAddressable other)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(IAddressable other)
        {
            throw new System.NotImplementedException();
        }

        public int GetConsistentHash()
        {
            throw new System.NotImplementedException();
        }
    }

    interface IChannel<M>
    {
        bool IsDisposed { get; }
        void Send<M>(M message);
        void Signal(ISignal signal);
    }
}