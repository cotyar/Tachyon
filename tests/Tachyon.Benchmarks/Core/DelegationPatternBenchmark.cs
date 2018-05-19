#region copyright
// -----------------------------------------------------------------------
//  <copyright file="DelegationPatternBenchmark.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using BenchmarkDotNet.Attributes;
using Tachyon.Core;

namespace Tachyon.Benchmarks.Core
{
    /// <summary>
    /// Last check:
    /// 
    /// ```ini
    /// BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
    /// Intel Core i5-4430 CPU 3.00GHz(Haswell), 1 CPU, 4 logical and 4 physical cores
    /// .NET Core SDK = 2.1.300-preview1-008174
    /// 
    /// [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
    /// DefaultJob : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
    /// ```
    /// 
    /// |       Method      |     Mean   |      Error |     StdDev |  Scaled  | ScaledSD |    Gen 0   |  Allocated |
    /// |------------------ |-----------:|-----------:|-----------:|---------:|---------:|-----------:|-----------:|
    /// | Call_direct       |   670.5 us |   3.196 us |   2.990 us |     1.00 |     0.00 |          - |        0 B |
    /// | Call_via_struct   |   669.3 us |   1.774 us |   1.659 us |     1.00 |     0.00 |          - |        0 B |
    /// | Call_via_delegate | 9,832.2 us | 196.651 us | 193.137 us |    14.66 |     0.29 | 20343.7500 | 64000024 B |
    /// 
    /// </summary>
    [Config(typeof(TachyonConfig))]
    public class DelegationPatternBenchmark
    {
        public const int OPS = 1_000_000;
        public readonly int Context = 10;

        [Benchmark(Baseline = true)]
        public int Call_direct()
        {
            var sum = 0;
            for (int i = 0; i < OPS; i++)
            {
                sum += Context + i;
            }

            return sum;
        }

        [Benchmark]
        public int Call_via_struct()
        {
            var sum = 0;
            for (int i = 0; i < OPS; i++)
            {
                var r = new Runner(i);
                sum += RunStructure(r);
            }

            return sum;
        }

        [Benchmark]
        public int Call_via_delegate()
        {
            var sum = 0;
            for (int i = 0; i < OPS; i++)
            {
                sum += RunDelegate(context => i + context);
            }

            return sum;
        }

        internal readonly struct Runner : IFunc<int, int>
        {
            public readonly int message;

            public Runner(int message)
            {
                this.message = message;
            }

            public int Invoke(int context) => context + message;
        }

        private int RunStructure<T>(in T call) where T : struct, IFunc<int, int> => 
            call.Invoke(Context);

        private int RunDelegate(Func<int, int> call) => 
            call(Context);
    }
}