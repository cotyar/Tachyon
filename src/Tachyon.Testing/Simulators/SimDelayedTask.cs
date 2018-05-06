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
    sealed class SimDelayedTask : Task
    {
        public TimeSpan Delay { get; }
        public CancellationToken CancellationToken { get; }

        public SimDelayedTask(TimeSpan delay, CancellationToken cancellationToken)
            : base(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();
            })
        {
            Delay = delay;
            CancellationToken = cancellationToken;
        }
    }
}