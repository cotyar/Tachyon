#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ByteKey.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tachyon.Core
{
    /// <summary>
    /// <see cref="ByteKey"/> is an immutable data structure, which spans over an array of bytes 
    /// and allows for fast hash-based and equality-based operations (i.e. using byte arrays in 
    /// <see cref="HashSet{T}"/> or as a key in <see cref="Dictionary{TKey,TValue}"/>).
    /// <see cref="ByteKey"/> hash is a consistent hash.
    /// </summary>
    [Immutable]
    [Serializable]
    public readonly struct ByteKey : IEquatable<ByteKey>, IConsistentlyHashable
    {
        #region internal classes

        public struct ByteKeyComparer : IEqualityComparer<ByteKey>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Equals(ByteKey x, ByteKey y) => x == y;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int GetHashCode(ByteKey obj) => obj.GetHashCode();
        }

        #endregion

        public static ByteKeyComparer EqualityComparer = new ByteKeyComparer();

        private readonly int hash;
        private readonly byte[] bytes;

        public ByteKey(byte[] bytes)
        {
            this.bytes = bytes ?? throw new ArgumentNullException();
            this.hash = Murmur.Hash(bytes);
        }

        public bool Equals(ByteKey other)
        {
            if (hash != other.hash) return false;
            return ByteArrayHelper.StructuralyEquals(bytes, other.bytes);
        }

        public override bool Equals(object obj) => obj is ByteKey key && Equals(key);

        public override int GetHashCode() => GetConsistentHash();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetConsistentHash() => hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ByteKey x, ByteKey y) => x.Equals(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ByteKey x, ByteKey y) => !(x == y);

        public static implicit operator ReadOnlySpan<byte>(ByteKey x) => x.bytes;
        public static implicit operator ByteKey (byte[] x) => new ByteKey(x);
    }
}