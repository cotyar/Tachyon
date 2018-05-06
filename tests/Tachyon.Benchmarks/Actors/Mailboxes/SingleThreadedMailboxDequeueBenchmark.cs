#region copyright
// Based on MPMCQueue.NET by Alexandr Nikitin: https://github.com/alexandrnikitin/MPMCQueue.NET
//
// Original license:
//
// MIT License
//
// Copyright(c) 2016 Alexandr Nikitin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using BenchmarkDotNet.Attributes;
using Tachyon.Actors;
using Tachyon.Core.Actors.Mailboxes;

namespace Tachyon.Benchmarks.Actors.Mailboxes
{
    [Config(typeof(TachyonConfig))]
    public class SingleThreadedMailboxDequeueBenchmark
    {
        private const int OPERATIONS = 1 << 23;

        private BoundedQueue boundedQueue;
        private UnboundedQueue<ISignal> unboundedQueue;

        [IterationSetup]
        public void Setup()
        {
            boundedQueue = new BoundedQueue(OPERATIONS);
            unboundedQueue = new UnboundedQueue<ISignal>();

            var msg = Activated.Instance;
            for (int i = 0; i < OPERATIONS; i++)
            {
                boundedQueue.TryPush(msg);
                unboundedQueue.TryPush(msg);
            }
        }

        [Benchmark(OperationsPerInvoke = OPERATIONS)]
        public void Unbounded_queue_dequeue()
        {
            ISignal message;
            for (int i = 0; i < OPERATIONS; i++)
            {
                unboundedQueue.TryPop(out message);
            }
        }

        [Benchmark(OperationsPerInvoke = OPERATIONS)]
        public void Bounded_queue_dequeue()
        {
            ISignal message;
            for (int i = 0; i < OPERATIONS; i++)
            {
                boundedQueue.TryPop(out message);
            }
        }
    }
}