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
using Tachyon.Core.Async;

namespace Tachyon.Actors
{
    public sealed class ActorRuntime : AbstractActorRuntime
    {
        public static async Task<ActorRuntime> StartAsync(Func<ActorRuntimeBuilder, ActorRuntimeBuilder> configure, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
                throw new TaskCanceledException($"Couldn't start the instance of {nameof(ActorRuntime)}. Cancellation has been requested.");

            var builder = configure(ActorRuntimeBuilder.Default);

            throw new NotImplementedException();
        }


        internal ActorRuntime()
        {
        }

    }
}