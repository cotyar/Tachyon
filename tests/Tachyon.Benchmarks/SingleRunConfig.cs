#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SingleRunConfig.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace Tachyon.Benchmarks
{
    public class SingleRunConfig : ManualConfig
    {
        public SingleRunConfig()
        {
            Add(Job.RyuJitX64
                .WithLaunchCount(2)
                .WithWarmupCount(20)
                .WithTargetCount(20));

            Add(MarkdownExporter.GitHub);
        }
    }
}