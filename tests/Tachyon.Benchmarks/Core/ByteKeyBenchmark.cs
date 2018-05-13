#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ByteKeyBenchmark.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using BenchmarkDotNet.Attributes;
using Tachyon.Core;

namespace Tachyon.Benchmarks.Core
{
    [Config(typeof(TachyonConfig))]
    public class ByteKeyBenchmark
    {
        private ByteKey key1;
        private ByteKey key2;

        [GlobalSetup]
        public void Setup()
        {
            var binary1 = new byte[100];
            var binary2 = new byte[100];

            SafeRandom.Current.NextBytes(binary1);
            // change binary2 on the last position
            binary1.CopyTo(binary2, 0);
            binary2[binary2.Length - 1] ^= binary2[binary2.Length - 1];

            key1 = binary1;
            key2 = binary2;
        }

        [Benchmark]
        public bool ByteKey_structural_equality() => key1 == key2;
    }
}