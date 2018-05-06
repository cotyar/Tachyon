#region copyright
// -----------------------------------------------------------------------
//  <copyright file="HybridTime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core
{
    /// <summary>
    /// Hybrid logical time, used to enhance partially comparable <see cref="VectorTime"/> mechanics with ability to 
    /// use local system clock in order to resolve potential happen-before relationship, once a concurrent update has been detected.
    /// </summary>
    [Immutable]
    [Serializable]
    public readonly struct HybridTime : IComparable<HybridTime>, IEquatable<HybridTime>, IConvergent<HybridTime>
    {
        public static readonly HybridTime Zero = new HybridTime(DateTime.MinValue, VectorTime.Zero);

        public readonly DateTime SystemTime;
        public readonly VectorTime VectorTime;

        public HybridTime(DateTime systemTime, VectorTime vectorTime)
        {
            SystemTime = systemTime;
            VectorTime = vectorTime;
        }

        public int CompareTo(HybridTime other)
        {
            var result = VectorTime.PartiallyCompareTo(other.VectorTime);
            return result ?? SystemTime.CompareTo(other.SystemTime);
        }

        public bool Equals(HybridTime other) => SystemTime == other.SystemTime && VectorTime == other.VectorTime;

        public HybridTime Merge(HybridTime other) => new HybridTime(TimeExtensions.Max(SystemTime, other.SystemTime), VectorTime + other.VectorTime);

        public override bool Equals(object obj) => obj is HybridTime time && Equals(time);

        public override int GetHashCode()
        {
            unchecked
            {
                return (SystemTime.GetHashCode() * 397) ^ VectorTime.GetHashCode();
            }
        }
    }
}