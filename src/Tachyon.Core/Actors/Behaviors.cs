#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Behaviors.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Threading.Tasks;

namespace Tachyon.Actors
{
    public delegate ValueTask<IBehavior<S, M>> Deferred<S, M>(ICell<S, M> cell);

    public delegate ValueTask<IBehavior<S, M>> Receive<S, M>(ICell<S, M> cell, S state, M message);
    public delegate ValueTask<IBehavior<S, M>> Signalize<S, M>(ICell<S, M> cell, S state, ISignal signal);

    public interface IBehavior<S, M> { }

    /// <summary>
    /// Behavior which determines a custom reaction over incoming message or signal.
    /// </summary>
    /// <typeparam name="S">Type of an actor state.</typeparam>
    /// <typeparam name="M">Type of a message handled by the actor.</typeparam>
    sealed class Immutable<S, M> : IBehavior<S, M>
    {
        public static readonly Signalize<S, M> DefaultOnSignal = (cell, s, sig) => new ValueTask<IBehavior<S, M>>(Same<S, M>.Instance);

        readonly S state;
        readonly Receive<S, M> onMessage;
        readonly Signalize<S, M> onSignal;

        public Immutable(S state, Receive<S, M> onMessage)
        {
            this.state = state;
            this.onMessage = onMessage;
            this.onSignal = DefaultOnSignal;
        }

        public Immutable(S state, Receive<S, M> onMessage, Signalize<S, M> onSignal)
        {
            this.state = state;
            this.onMessage = onMessage;
            this.onSignal = onSignal;
        }

        public Immutable<S, M> OnSignal(Signalize<S, M> signalize) => new Immutable<S, M>(this.state, this.onMessage, signalize);
    }

    /// <summary>
    /// Behavior which tells an actor, that it's handler should remain unchanged.
    /// </summary>
    /// <typeparam name="S">Type of an actor state.</typeparam>
    /// <typeparam name="M">Type of a message handled by the actor.</typeparam>
    sealed class Same<S, M> : IBehavior<S, M>
    {
        public static readonly Same<S,M> Instance = new Same<S, M>();
        private Same() { }
    }

    /// <summary>
    /// Behavior which enables to initialize an actor with a given <see cref="Deferred{S,M}"/> function
    /// right after its creation, before any message has been send to it.
    /// </summary>
    /// <typeparam name="S">Type of an actor state.</typeparam>
    /// <typeparam name="M">Type of a message handled by the actor.</typeparam>
    sealed class Defer<S, M> : IBehavior<S, M>
    {
        readonly Deferred<S, M> func;

        public Defer(Deferred<S, M> func)
        {
            this.func = func;
        }
    }

    /// <summary>
    /// Behavior which tells an actor, that the last received message was not handled correctly.
    /// </summary>
    /// <typeparam name="S">Type of an actor state.</typeparam>
    /// <typeparam name="M">Type of a message handled by the actor.</typeparam>
    sealed class Unhandled<S, M> : IBehavior<S, M>
    {
        public static readonly Unhandled<S, M> Instance = new Unhandled<S, M>();
        private Unhandled() { }
    }

    public static class Behaviors
    {
        public static IBehavior<S,M> Immutable<S,M>(S state, Receive<S, M> onMessage) => new Immutable<S, M>(state, onMessage);
        public static IBehavior<S, M> Immutable<S, M>(S state, Receive<S, M> onMessage, Signalize<S, M> onSignal) => new Immutable<S, M>(state, onMessage, onSignal);
    }
}