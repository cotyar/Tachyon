#region copyright
// -----------------------------------------------------------------------
//  <copyright file="Unit.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

namespace Tachyon.Core
{
    /// <summary>
    /// Unit type, which denotes no value picked or returned.
    /// </summary>
    public readonly struct Unit
    {
        public static readonly Unit Default = new Unit();
    }
}