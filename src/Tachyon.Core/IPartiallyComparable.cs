#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IPartiallyComparable.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2017 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;

namespace Tachyon.Core
{
    /// <summary>
    /// An interface used by partial comparison of types. Unlike <see cref="IComparable{T}"/>,
    /// it allows to additionally give a 4th option in case when states of two compared instances
    /// doesn't allow to perform direct comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPartiallyComparable<in T>
    {
        int? PartiallyCompareTo(T other);
    }

    /// <summary>
    /// An interface used to implement comparer allowing partial comparison of types. Unlike
    /// <see cref="IComparer{T}"/>, it allows to additionally give a 4th option in case when 
    /// states of two compared instances doesn't allow to perform direct comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPartialComparer<in T>
    {
        int? PariallyCompare(T x, T y);
    }
}