﻿#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Simulation.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tachyon.Core;

namespace Tachyon.Testing.Simulators
{
    /// <summary>
    /// Environment representing a state-of-a-world during a simulation. It encapsulates
    /// things like different actor runtimes, task schedulers, network access etc.
    /// </summary>
    public sealed class Simulation
    {
        #region queueing

        private sealed class ExecutionQueue
        {
            private readonly SortedList<DateTime, Queue<(SimScheduler, Task)>> timeline = new SortedList<DateTime, Queue<(SimScheduler, Task)>>();

            public void Schedule(DateTime pointInTime, [NotNull]Task task, [NotNull]SimScheduler scheduler)
            {
                if (!timeline.TryGetValue(pointInTime, out var queue))
                {
                    queue = new Queue<(SimScheduler, Task)>();
                    timeline.Add(pointInTime, queue);
                }

                queue.Enqueue((scheduler, task));
            }

            public bool TryGetNext(out WorkItem workItem)
            {
                foreach (var (time, queue) in timeline)
                {
                    if (queue.Count != 0)
                    {
                        var (scheduler, task) = queue.Dequeue();
                        workItem = new WorkItem(scheduler, time, task);

                        // cleanup
                        if (queue.Count == 0)
                        {
                            timeline.Remove(time);
                        }

                        return true;
                    }
                }

                workItem = default;
                return false;
            }
        }

        #endregion

        private readonly SimulationSettings settings;
        private readonly ExecutionQueue executionQueue = new ExecutionQueue();
        private readonly SimScheduler mainScheduler;
        private readonly TaskFactory taskFactory;

        private bool halt = false;

        public Simulation() : this(SimulationSettings.Default)
        {
        }

        public Simulation(SimulationSettings settings)
        {
            this.settings = settings;
            this.mainScheduler = new SimScheduler("sim:main", this);
            this.taskFactory = new TaskFactory(this.mainScheduler);
        }

        public DateTime CurrentTime { get; private set; } = DateTime.UtcNow;

        public void Schedule([NotNull]Task task, [NotNull]SimScheduler scheduler, [NotNull]TimeSpan delay = default)
        {
            var pointInTime = delay == Timeout.InfiniteTimeSpan
                ? DateTime.MaxValue
                : CurrentTime + delay;
            
            executionQueue.Schedule(pointInTime, task, scheduler);
        }

        public void Log(string text) => settings.Log(text);

        public void Run(Func<SimScenario, Task> plan)
        {
            Schedule(taskFactory.StartNew(async () =>
            {
                var scenario = new SimScenario(this);
                try
                {
                    await plan(scenario);
                }
                catch (Exception e)
                {
                    halt = true;
                    Log($"Plan failed: {e}");
                }
            }), mainScheduler);

            while (!halt && executionQueue.TryGetNext(out var workItem))
            {
                CurrentTime = workItem.PointInTime;
                workItem.Scheduler.Execute(workItem.Task);
            }
        }
    }

    public readonly struct WorkItem
    {
        public readonly SimScheduler Scheduler;
        public readonly DateTime PointInTime;
        public readonly Task Task;

        public WorkItem(SimScheduler scheduler, DateTime pointInTime, Task task)
        {
            Scheduler = scheduler;
            PointInTime = pointInTime;
            Task = task;
        }
    }
}