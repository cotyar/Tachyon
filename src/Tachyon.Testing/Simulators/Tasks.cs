#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Tasks.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tachyon.Testing.Simulators
{
    public interface IManagedTime
    {
        TimeSpan Deadline { get; }
        bool PastDeadline { get; }
    }

    public sealed class SimTask : Task, IManagedTime
    {
        private readonly CancellationToken token;

        public SimTask(TimeSpan delay, CancellationToken token) : base(() =>
        {
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();
        })
        {
            // We don't pass the token to the base task. Just like with Task.Delay
            // we want the task to blow up on awaiter, instead of simply
            // stopping further execution.
            this.token = token;
            this.Deadline = delay;
        }

        public TimeSpan Deadline { get; }
        public bool PastDeadline => token.IsCancellationRequested;
    }
}