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

    /// <summary>
    /// Streams are binding versions working in the context of reactive
    /// streams.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public interface IStream<M> : IBinding
    {

    }

    /// <summary>
    /// Keys are binding versions working in the context of Conflict-free
    /// Replicated Data Types (CRDTs) accessible from tachyon cluster.
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public interface IKey<V> : IBinding
    {

    }

    /// <summary>
    /// Channels are binding versions working in the context of actors.
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public interface IChannel<M> : IBinding
    {
        void Send<M>(M message);
        void Signal(ISignal signal);
    }
}