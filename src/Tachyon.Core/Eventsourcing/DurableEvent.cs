#region copyright
// -----------------------------------------------------------------------
//  <copyright file="DurableEvent.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Core.Eventsourcing
{
    public sealed class DurableEvent<TData>
    {
        public HybridTime Timestamp { get; }
        public TData Data { get; }
    }
}