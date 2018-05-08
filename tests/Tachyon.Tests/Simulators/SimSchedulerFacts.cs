#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimSchedulerFacts.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Tachyon.Testing.Simulators;
using Xunit;
using Xunit.Abstractions;

namespace Tachyon.Tests.Simulators
{
    public class SimSchedulerFacts
    {
        private readonly Simulation env;

        public SimSchedulerFacts(ITestOutputHelper output)
        {
            env = new Simulation(SimulationSettings.Default.WithLog(output.WriteLine));
        }

        [Fact]
        public void SimScheduler_should_run_tasks_one_after_another() => env.Run(async scenario =>
        {
            var queue = new Queue<int>();

            var t1 = new Task(() => queue.Enqueue(1));
            var t2 = new Task(() => queue.Enqueue(2));
            var t3 = new Task(() => queue.Enqueue(3));

            await Task.WhenAll(t1, t2, t3);

            queue.Should().ContainInOrder(1, 2, 3);
        });

        [Fact]
        public void SimScheduler_should_run_delays_without_affecting_physical_time()
        {
            env.Run(async scenario =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await scenario.Delay(1.Hours());
                await scenario.Delay(1.Hours());
                await scenario.Delay(1.Hours());

                stopwatch.Stop();
                stopwatch.ElapsedMilliseconds.Should().BeLessThan(100);
            });
        }
    }
}