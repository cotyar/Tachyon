#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Cell.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Threading;
using System.Threading.Tasks;
using Tachyon.Core;
using Tachyon.Core.Actors.Mailboxes;

namespace Tachyon.Actors
{
    /// <summary>
    /// An implementation of <see cref="ICell{S,M}"/> interface living on the local machine.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="M"></typeparam>
    public sealed class Cell<S, M> : ICell<S, M>, IChannel<M>
    {
        private int status;

        private readonly IMailboxQueue<M> messages;
        private readonly IMailboxQueue<ISignal> signals;

        private readonly IActorRuntime runtime;
        private readonly Region region;

        private readonly IBehavior<S, M> behavior;

        public Cell(IBehavior<S, M> initBehavior)
        {
        }

        #region ICell impl

        public MailboxAwaiter Send<M1>(Var<IChannel<M1>> target, M1 message)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySend<M1>(Var<IChannel<M1>> target, M1 message)
        {
            throw new System.NotImplementedException();
        }

        public MailboxAwaiter Signal(IAddressable target, ISignal signal)
        {
            throw new System.NotImplementedException();
        }

        public bool TrySignal(IAddressable target, ISignal signal)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task DisposeAsync(CancellationToken token = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        IAddressable ICell.Self => Self;

        public S State { get; }
        public Var<IChannel<M>> Self { get; }
        public IActorRuntime Runtime { get; }
        public ValueTask<Unit> Stop()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IChannel impl

        public bool IsDisposed { get; }
        public void Send(M message)
        {
            throw new System.NotImplementedException();
        }

        public void Signal(ISignal signal)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}