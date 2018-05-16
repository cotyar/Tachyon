#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Bindings.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Actors
{
    public interface IBinding : IDisposable
    {
        bool IsDisposed { get; }
    }

    public interface IChannel<M> : IBinding
    {
        void Send<M>(M message);
        void Signal(ISignal signal);
    }
}