#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimScenario.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tachyon.Testing.Simulators
{
    /// <summary>
    /// Conductor used to define a test case scenario.
    /// </summary>
    public sealed class SimScenario
    {
        private readonly SimEnvironment env;

        internal SimScenario(SimEnvironment env)
        {
            this.env = env;
        }

        public DateTime CurrentTime => env.CurrentTime;

        public void Log(string message) => env.Log(message);

        public Task Delay(TimeSpan delay, CancellationToken token = default)
        {
            return new SimDelayedTask(delay, token);
        }
    }
}