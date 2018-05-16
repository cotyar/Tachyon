#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Regions.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Actors
{
    interface IBoundAs<B> where B : IBinding
    {
        
    }

    /// <summary>
    /// An interface, which allows to bind a a specific resources and
    /// make them accessible from either local or global keyspace.
    /// scope.
    /// </summary>
    interface IBinder
    {
        /// <summary>
        /// Defines a global behavior in context of a current actor runtime. It can be
        /// instantiated and accessed later on using <see cref="Global{B}"/> vars.
        /// </summary>
        void Define<B>(IBoundAs<B> bindable) where B : IBinding;

        /// <summary>
        /// Binds a defined <paramref name="behavior"/> to a locally scoped
        /// <paramref name="var"/>.
        /// </summary>
        void Bind<B>(IBoundAs<B> behavior, Local<B> var) where B : IBinding;
    }

    public abstract class Region
    {
        
    }
}