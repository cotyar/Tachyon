﻿#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Regions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Collections.Immutable;
using Tachyon.Core;

namespace Tachyon.Actors
{
    public abstract class Region
    {
        private ImmutableDictionary<ByteKey, ICell> activeCells = ImmutableDictionary<ByteKey, ICell>.Empty;
    }
}