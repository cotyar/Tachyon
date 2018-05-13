#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimRuntime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Tachyon.Actors;

namespace Tachyon.Testing.Simulators
{
    public sealed class SimRuntime : IActorRuntime
    {
        public ITimer Timer { get; }
        public TaskScheduler TaskScheduler { get; }
        public Var<DeadLetter> DeadLetters { get; }
        public Random Random { get; }
        public DateTime UtcNow()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task DisposeAsync(CancellationToken token = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        Task IActorRuntime.StartAsync(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}