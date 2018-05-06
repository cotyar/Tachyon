#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SimConnection.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using Tachyon.Core.IO;

namespace Tachyon.Testing.Simulators
{
    /// <summary>
    /// Simulation of a connection between two endpoints.
    /// </summary>
    public sealed class SimConnection : IConnection
    {
        public void Dispose()
        {
        }
    }
}