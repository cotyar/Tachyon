#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimSchedulerFacts.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
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
        public void SimScheduler_should_run_tasks_one_after_another()
        {
            env.Run(async scenario =>
            {
                //TODO:
            });
        }

        private async Task Add(List<int> q, int i)
        {
            q.Add(i);
        }
    }
}