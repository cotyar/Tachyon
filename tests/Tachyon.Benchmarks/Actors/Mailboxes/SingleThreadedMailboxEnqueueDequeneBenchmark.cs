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
    public class SingleThreadedMailboxEnqueueDequeneBenchmark
    {
        private BoundedQueue boundedQueue;
        private UnboundedQueue<ISignal> unboundedQueue;
        private static readonly ISignal msg = Activated.Instance;

        [GlobalSetup]
        public void Setup()
        {
            boundedQueue = new BoundedQueue(128);
            unboundedQueue = new UnboundedQueue<ISignal>();
        }

        [Benchmark]
        public void UnboundedQueue_enqueue_dequeue()
        {
            unboundedQueue.TryPush(msg);
            unboundedQueue.TryPop(out var signal);
        }

        [Benchmark]
        public void BoundedQueue_enqueue_dequeue()
        {
            boundedQueue.TryPush(msg);
            boundedQueue.TryPop(out var signal);
        }
    }
}