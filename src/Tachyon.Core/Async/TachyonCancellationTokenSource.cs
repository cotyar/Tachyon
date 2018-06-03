#region copyright
// -----------------------------------------------------------------------
//  <copyright file="TachyonCancellationTokenSource.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Threading;

namespace Tachyon.Core.Async
{
    public sealed class TachyonCancellationTokenSource
    {
        public CancellationToken Token { get; }
    }
}