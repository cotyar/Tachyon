#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IAsyncEnumerator.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tachyon.Core.Streams
{
    public interface IAsyncEnumerator<out T> : IDisposable
    {
        T Current { get; }
        Task<bool> MoveNextAsync(CancellationToken token = default(CancellationToken));
    }
}