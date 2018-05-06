#region copyright
// -----------------------------------------------------------------------
//  <copyright file="VectorTime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Tachyon.Core
{
    using NodeId = Int32;

    /// <summary>
    /// Vector time, which allows for partial comparison in intend to detect concurrent updates occurring in distributed systems.
    /// </summary>
    [Immutable]
    [Serializable]
    public readonly struct VectorTime : IPartiallyComparable<VectorTime>, IEquatable<VectorTime>, IEnumerable<KeyValuePair<NodeId, uint>>, IConvergent<VectorTime>
    {
        #region internal classes

        public struct VectorTimeComparer : IPartialComparer<VectorTime>
        {
            public int? PariallyCompare(VectorTime x, VectorTime y) => x.PartiallyCompareTo(y);
        }

        #endregion

        /// <summary>
        /// Comparer instance used for partial comparison of <see cref="VectorTime"/>s.
        /// </summary>
        public static readonly VectorTimeComparer PartialComparer = new VectorTimeComparer();

        public static readonly VectorTime Zero = new VectorTime(new NodeId[0], new uint[0]);

        private readonly NodeId[] nodes;
        private readonly uint[] values;

        internal VectorTime(NodeId[] nodes, uint[] values)
        {
            this.nodes = nodes;
            this.values = values;
        }

        public int? PartiallyCompareTo(VectorTime other)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(VectorTime other)
        {
            if (ReferenceEquals(nodes, other.nodes) && ReferenceEquals(values, other.values)) return true;
            if (nodes.Length != other.nodes.Length) return false;

            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<NodeId, uint>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public VectorTime MergeMax(VectorTime other)
        {
            throw new NotImplementedException();
        }

        public VectorTime MergeMin(VectorTime other)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VectorTime Merge(VectorTime other) => MergeMax(other);

        public override bool Equals(object obj) => obj is VectorTime vt && Equals(vt);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(VectorTime x, VectorTime y) => x.Equals(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(VectorTime x, VectorTime y) => !(x == y);

        public static bool operator >(VectorTime x, VectorTime y) => x.PartiallyCompareTo(y) == 1;
        public static bool operator <(VectorTime x, VectorTime y) => x.PartiallyCompareTo(y) == -1;
        public static bool operator >=(VectorTime x, VectorTime y) => x.PartiallyCompareTo(y) >= 0;
        public static bool operator <=(VectorTime x, VectorTime y) => x.PartiallyCompareTo(y) <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorTime operator +(VectorTime x, VectorTime y) => x.MergeMax(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorTime operator -(VectorTime x, VectorTime y) => x.MergeMin(y);
    }
}