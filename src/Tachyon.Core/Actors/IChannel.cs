#region copyright
// -----------------------------------------------------------------------
//  <copyright file="IChannel.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Actors
{
    public interface IChannel<M> : IDisposable
    {
        bool IsDisposed { get; }
        void Send<M>(M message);
        void Signal(ISignal signal);
    }
}