#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimulationSettings.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;

namespace Tachyon.Testing.Simulators
{
    public class SimulationSettings
    {
        public static readonly SimulationSettings Default = new SimulationSettings(Console.WriteLine);

        public Action<string> Log { get; }

        public SimulationSettings([NotNull]Action<string> log)
        {
            Log = log;
        }

        public SimulationSettings WithLog([NotNull] Action<string> log) => Copy(log: log);

        private SimulationSettings Copy(Action<string> log = null) =>
            new SimulationSettings(
                log: log ?? Log);
    }
}