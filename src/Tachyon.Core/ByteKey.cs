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
using JetBrains.Annotations;

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
    public readonly struct ByteKey : IEquatable<ByteKey>, IConsistentlyHashable, IComparable<ByteKey>, IComparable
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
        public static readonly ByteKey Empty = new ByteKey(new byte[0]);

        private readonly int hash;
        private readonly byte[] bytes;

        public ByteKey([NotNull]byte[] bytes)
        {
            Require.NotNull(bytes);

            this.bytes = bytes;
            this.hash = Murmur.Hash(bytes);
        }

        public ReadOnlySpan<byte> Bytes => bytes;

        public bool Equals(ByteKey other)
        {
            if (hash != other.hash) return false;
            return ByteArrayHelper.StructuralyEquals(bytes, other.bytes);
        }

        public override bool Equals(object obj) => obj is ByteKey key && Equals(key);

        public int CompareTo(ByteKey other)
        {
            var result = hash.CompareTo(other.hash);
            if (result == 0)
            {
                result = bytes.Length.CompareTo(other.bytes.Length);
                if (result == 0)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        result = bytes[i].CompareTo(other.bytes[i]);
                        if (result != 0)
                            return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Keep for compatibility with F# Map.
        /// </summary>
        int IComparable.CompareTo(object obj)
        {
            if (obj is ByteKey other) return CompareTo(other);
            else
            {
                Raise.InvalidOperationException($"Cannot compare instance of '{nameof(ByteKey)}' to '{obj?.GetType().FullName ?? "null"}'");
                return -1;
            }
        }

        public override int GetHashCode() => GetConsistentHash();

        [Pure]
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