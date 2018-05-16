#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Regions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Actors
{
    /// <summary>
    /// Actor provider exposes an API, which allows to bind a a specific
    /// actor behaviors to later be able to access them from local or global
    /// scope.
    /// </summary>
    interface IActorProvider
    {
        /// <summary>
        /// Binds a global behavior for current actor runtime. It can be
        /// instantiated and accessed later on using <see cref="Global{M}"/> vars.
        /// </summary>
        void Bind<S, M>(IBehavior<S, M> globalBehavior);

        /// <summary>
        /// Binds a defined <paramref name="behavior"/> to a locally scoped
        /// <paramref name="var"/>.
        /// </summary>
        void Bind<S, M>(IBehavior<S, M> behavior, Local<M> var);
    }

    public abstract class Region
    {
        
    }
}