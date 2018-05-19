#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ActorRuntime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Tachyon.Core;

namespace Tachyon.Actors
{
    public sealed class ActorRuntime : IActorRuntime
    {
        public ITimer Timer { get; }
        public TaskScheduler TaskScheduler { get; }
        public Var<IChannel<DeadLetter>> DeadLetters { get; }
        public Random Random => SafeRandom.Current;

        public DateTime UtcNow()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public async Task DisposeAsync(CancellationToken token = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task StartAsync(CancellationToken token = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public void ResolveBinding<B>(Var<B> var, out B binding) where B : class, IBinding
        {
            throw new NotImplementedException();
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken) => DisposeAsync(cancellationToken);
        public void Schedule<M>(TimeSpan delay, Var<IChannel<M>> target, M message, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Schedule<M>(TimeSpan delay, TimeSpan interval, Var<IChannel<M>> target, M message,
            CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Schedule<M>(string key, DateTime fireAt, Var<IChannel<M>> target, M message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Cancel(string key)
        {
            throw new NotImplementedException();
        }
    }
}