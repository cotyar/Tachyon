#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Signals.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Actors
{
    /// <summary>
    /// Send send when an actor instance has been created or recreated.
    /// </summary>
    public sealed class Activated : ISignal
    {
        public static readonly Activated Instance = new Activated();
        private Activated() { }
    }

    /// <summary>
    /// Signals send when an actor instance was subject of a crash or kill signal
    /// and it's going to be stopped or rerunned.
    /// </summary>
    public sealed class Deactivated : ISignal
    {
        public static readonly Deactivated NoError = new Deactivated();

        private readonly Exception cause;

        public Deactivated(Exception cause = null)
        {
            this.cause = cause;
        }
    }
}