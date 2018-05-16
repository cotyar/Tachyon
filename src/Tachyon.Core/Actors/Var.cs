#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Var.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Text;
using JetBrains.Annotations;
using Tachyon.Core;
using Tachyon.Core.Actors.Mailboxes;

namespace Tachyon.Actors
{
    /// <summary>
    /// Helper methods for building <see cref="Var{M}"/> from values of different types.
    /// </summary>
    public static class Vars
    {
        public static Local<M> Local<M>(ByteKey regionKey, int key) => new Local<M>(regionKey, BitConverter.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, uint key) => new Local<M>(regionKey, BitConverter.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, long key) => new Local<M>(regionKey, BitConverter.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, ulong key) => new Local<M>(regionKey, BitConverter.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, Guid key) => new Local<M>(regionKey, key.ToByteArray());
        public static Local<M> Local<M>(ByteKey regionKey, [NotNull]string key) => new Local<M>(Encoding.UTF8.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, [NotNull]string key, Encoding encoding) => new Local<M>(encoding.GetBytes(key));
        public static Local<M> Local<M>(ByteKey regionKey, ByteKey key) => new Local<M>(regionKey, key);

        public static Global<M> Global<M>(int key) => new Global<M>(BitConverter.GetBytes(key));
        public static Global<M> Global<M>(uint key) => new Global<M>(BitConverter.GetBytes(key));
        public static Global<M> Global<M>(long key) => new Global<M>(BitConverter.GetBytes(key));
        public static Global<M> Global<M>(ulong key) => new Global<M>(BitConverter.GetBytes(key));
        public static Global<M> Global<M>(Guid key) => new Global<M>(key.ToByteArray());
        public static Global<M> Global<M>([NotNull]string key) => new Global<M>(Encoding.UTF8.GetBytes(key));
        public static Global<M> Global<M>([NotNull]string key, Encoding encoding) => new Global<M>(encoding.GetBytes(key));
        public static Global<M> Global<M>(ByteKey key) => new Global<M>(key);
    }

    /// <summary>
    /// A reference to an addressable resource accessible in distributed ecosystem:
    /// an actor, stream, key-value store entry etc.
    ///
    /// Note: at the moment type parameters is not used in order to determine the
    /// uniqueness of the <see cref="Var{M}"/> having the same key.
    /// </summary>
    /// <remarks>
    /// Do not confuse this with IVar data type from Concurrent ML.
    /// </remarks>
    /// <typeparam name="M"></typeparam>
    [Immutable]
    public abstract class Var<M> : IAddressable
    {
        private readonly KeyspaceType keyspace;
        protected readonly ByteKey key;

        /// <summary>
        /// Instead of sending the message through actor runtime every time,
        /// let's cache the direct pipe to a corresponding channel to either
        /// local or global resource and use it directly in subsequent tries.
        /// </summary>
        protected IChannel<M> channel;

        internal Var(KeyspaceType keyspace, ByteKey key)
        {
            this.keyspace = keyspace;
            this.key = key;
        }

        public KeyspaceType Keyspace => keyspace;
        public ReadOnlySpan<byte> Key => key;

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

        /// <summary>
        /// Returns a new instance of a <see cref="Var{N}"/> value,
        /// with a type parameter narrowed to a subtype of <typeparamref name="M"/>.
        /// </summary>
        /// <typeparam name="N">Subtype of <typeparamref name="M"/>.</typeparam>
        public abstract Var<N> Narrow<N>() where N : M;

        public int CompareTo(IAddressable other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(other, null)) return 1;

            var result = keyspace.CompareTo(other.Keyspace);

            return result == 0 ? Key.SequenceCompareTo(other.Key) : result;
        }

        public bool Equals(IAddressable other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            return keyspace == other.Keyspace && Key == other.Key;
        }

        public int GetConsistentHash() => key.GetConsistentHash();

        public override bool Equals(object obj) => 
            obj is IAddressable addressable && Equals(addressable);

        public override int GetHashCode() => key.GetHashCode();

        public override string ToString() => $"<{key}>";
    }

    /// <summary>
    /// Represents a locally-scoped variable. It's well known only in actor's region scope.
    /// It can be passed and accessed over the network boundaries, however two
    /// <see cref="Local{M}"/> vars with the same keys instantiated on different regions
    /// will not be equal to each other.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    [Immutable]
    public sealed class Local<M> : Var<M>
    {
        private static ByteKey ConstructKey(ByteKey region, ByteKey key)
        {
            var regionLength = region.Bytes.Length;
            Require.True(regionLength <= byte.MaxValue, $"Region's key cannot be longer than {byte.MaxValue} bytes");

            var buffer = new byte[1 + regionLength + key.Bytes.Length];
            Span<byte> span = buffer;
            buffer[0] = (byte)regionLength;

            region.Bytes.CopyTo(span.Slice(1, regionLength));

            key.Bytes.CopyTo(span.Slice(1 + regionLength));

            return new ByteKey(buffer);
        }

        internal Local(ByteKey key) : base(KeyspaceType.Local, key)
        {
        }

        public Local(ByteKey region, ByteKey key) 
            : base(KeyspaceType.Local, ConstructKey(region, key))
        {

        }

        /// <inheritdoc cref="Var{M}"/>
        public override Var<N> Narrow<N>() => new Local<N>(key);
    }

    /// <summary>
    /// Globally scoped vars - unique in scope of an entire cluster. Because of that,
    /// their resolution time may be longer than for <see cref="Local{M}"/> vars. They
    /// may be assigned globally and will always be equal to each other.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    [Immutable]
    public sealed class Global<M> : Var<M>
    {
        public Global(ByteKey key) : base(KeyspaceType.Global, key)
        {
        }

        /// <inheritdoc cref="Var{M}"/>
        public override Var<N> Narrow<N>() => new Global<N>(key);
    }
}