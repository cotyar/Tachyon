#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimEnvironmentSettings.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;

namespace Tachyon.Testing.Simulators
{
    public class SimEnvironmentSettings
    {
        public static readonly SimEnvironmentSettings Default = new SimEnvironmentSettings(Console.WriteLine);

        public Action<string> Log { get; }

        public SimEnvironmentSettings([NotNull]Action<string> log)
        {
            Log = log;
        }

        public SimEnvironmentSettings WithLog([NotNull] Action<string> log) => Copy(log: log);

        private SimEnvironmentSettings Copy(Action<string> log = null) =>
            new SimEnvironmentSettings(
                log: log ?? Log);
    }
}