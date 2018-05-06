#region copyright
// -----------------------------------------------------------------------
//  <copyright file="TimeExtensions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tachyon.Core
{
    static class TimeExtensions
    {
        public static DateTime Max(DateTime x, DateTime y) => new DateTime(Math.Max(x.Ticks, y.Ticks));

        public static TimeSpan Multiply(this TimeSpan time, int factor) => new TimeSpan(time.Ticks * factor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Seconds(this int value) => TimeSpan.FromSeconds(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Milliseconds(this int value) => TimeSpan.FromMilliseconds(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Minutes(this int value) => TimeSpan.FromMinutes(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Hours(this int value) => TimeSpan.FromHours(value);
    }

    static class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> entry, out TKey key, out TValue value)
        {
            key = entry.Key;
            value = entry.Value;
        }
    }
}