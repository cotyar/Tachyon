#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IAsyncDisposable.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tachyon.Core
{
    /// <summary>
    /// A variant of <see cref="IDisposable"/> interface, allows to asynchronously
    /// call resource disposal.
    /// </summary>
    public interface IAsyncDisposable : IDisposable
    {
        /// <summary>
        /// Asynchronously disposes current resource, returning task informing when
        /// a resource has been actually disposed.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DisposeAsync(CancellationToken token = default(CancellationToken));
    }
}