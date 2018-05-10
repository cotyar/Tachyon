#region copyright
// -----------------------------------------------------------------------
//  <copyright file="ActorRuntime.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tachyon.Actors
{
    public sealed class ActorRuntime : IActorRuntime
    {
        public ITimer Timer { get; }
        public TaskScheduler TaskScheduler { get; }
        public IVar<DeadLetter> DeadLetters { get; }

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

        Task IHostedService.StopAsync(CancellationToken cancellationToken) => DisposeAsync(cancellationToken);
    }
}