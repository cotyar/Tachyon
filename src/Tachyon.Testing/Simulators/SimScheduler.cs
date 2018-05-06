#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimScheduler.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Tachyon.Testing.Simulators
{
    /// <summary>
    /// Simulated <see cref="TaskScheduler"/> implementation which
    /// </summary>
    public sealed class SimScheduler : TaskScheduler
    {
        private readonly string name;
        private readonly SimEnvironment env;

        public SimScheduler(string name, SimEnvironment env)
        {
            this.name = name;
            this.env = env;
        }

        public void Execute([NotNull]Task task)
        {
            if (task.Status == TaskStatus.RanToCompletion) return;

            try
            {
                if (!TryExecuteTask(task))
                {
                    env.Log($"Couldn't execute [{task}] on [{name}]");
                }
            }
            catch (Exception error)
            {
                env.Log($"Failed executing [{task}] on [{name}]: {error}");
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks() => 
            throw new NotSupportedException($"{nameof(SimScheduler)} executes tasks immediatelly.");

        protected override void QueueTask(Task task)
        {
            env.Schedule(task, this);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
    }
}