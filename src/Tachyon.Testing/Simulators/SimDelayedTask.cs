#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimDelayedTask.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tachyon.Testing.Simulators
{
    interface INotifyCancellation
    {
        bool IsCancellationRequested { get; }
    }

    interface IDelayedExecution : INotifyCancellation
    {
        TimeSpan Deadline { get; }
    }

    sealed class SimDelayedTask : Task, IDelayedExecution
    {
        public TimeSpan Deadline { get; }
        public CancellationToken CancellationToken { get; }
        public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

        public SimDelayedTask(TimeSpan deadline, CancellationToken cancellationToken)
            : base(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();
            })
        {
            Deadline = deadline;
            CancellationToken = cancellationToken;
        }
    }
}