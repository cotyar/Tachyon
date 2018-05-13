#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ConsistentHashBenchmark.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using Tachyon.Benchmarks.Core.ComparedHashes;
using Tachyon.Core;

namespace Tachyon.Benchmarks.Core
{
    [Config(typeof(TachyonConfig))]
    public class ConsistentHashBenchmark
    {
        private string testString;
        private byte[] testBinary;

        [GlobalSetup]
        public void Setup()
        {
            testString = Guid.NewGuid().ToString("D");
            testBinary = new byte[100];
            
            SafeRandom.Current.NextBytes(testBinary);
        }

        [Benchmark]
        public int Murmur_string_hash() => Murmur.Hash(testString);

        [Benchmark]
        public uint Jenkins_string_hash() => Jenkins.Hash(testString);

        [Benchmark]
        public uint xxHash32_string_hash() => XXHash32.Hash(testString);

        [Benchmark]
        public int Murmur_binary_hash() => Murmur.Hash(testBinary);

        [Benchmark]
        public uint Jenkins_binary_hash() => Jenkins.Hash(testBinary);

        [Benchmark]
        public uint xxHash32_binary_hash() => XXHash32.Hash(testBinary);
    }
}