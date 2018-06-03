#region copyright
// -----------------------------------------------------------------------
//  <copyright file="AbstractActorRuntime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Tachyon.Core;
using Tachyon.Core.Async;

namespace Tachyon.Actors
{
    public abstract class AbstractActorRuntime : IActorRuntime
    {
        protected AbstractActorRuntime()
        {

        }

        public TaskScheduler TaskScheduler { get; }
        public Var<IChannel<DeadLetter>> DeadLetters { get; }
        public Random Random => SafeRandom.Current;

        public DateTime UtcNow() => DateTime.UtcNow;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public async Task DisposeAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task StartAsync(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public void ResolveBinding<B>(Var<B> var, out B binding) where B : class, IBinding
        {
            throw new NotImplementedException();
        }

        public TachyonCancellationTokenSource GetCancellationTokenSource()
        {
            throw new NotImplementedException();
        }

        public Task Delay(TimeSpan delay, CancellationToken token = default) => Task.Delay(delay, token);

        Task IHostedService.StopAsync(CancellationToken cancellationToken) => DisposeAsync(cancellationToken);

        /// <inheritdoc cref="ITimer"/>
        public void Schedule<M>(TimeSpan delay, Var<IChannel<M>> target, M message, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ITimer"/>
        public void Schedule<M>(TimeSpan delay, TimeSpan interval, Var<IChannel<M>> target, M message,
            CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ITimer"/>
        public Task Schedule<M>(string key, DateTime fireAt, Var<IChannel<M>> target, M message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ITimer"/>
        public Task<bool> Cancel(string key)
        {
            throw new NotImplementedException();
        }
    }
}