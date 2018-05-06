#region copyright
// -----------------------------------------------------------------------
//  <copyright file="AffinityTaskScheduler.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tachyon.Core.Async.Schedulers
{
    /// <summary>
    /// Affinitiy-based task scheduler can pin tasks in context of actors, which work they execute,
    /// to a particular thread executing on a particular machine core. Therefore local actor state
    /// doesn't have to be moved between local cache of different cores during actor logic execution.
    /// </summary>
    public sealed class AffinityTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new System.NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new System.NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new System.NotImplementedException();
        }
    }
}