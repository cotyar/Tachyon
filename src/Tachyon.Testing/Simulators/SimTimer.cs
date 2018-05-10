#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimTimer.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using Tachyon.Actors;

namespace Tachyon.Testing.Simulators
{
    public class SimTimer : ITimer
    {
        public void Schedule<M>(TimeSpan delay, IVar<M> target, M message, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Schedule<M>(TimeSpan delay, TimeSpan interval, IVar<M> target, M message, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}