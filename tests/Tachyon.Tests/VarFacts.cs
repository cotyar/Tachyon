#region copyright
// -----------------------------------------------------------------------
//  <copyright file="VarFacts.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;
using System.Text;
using FluentAssertions;
using Tachyon.Actors;
using Tachyon.Core;
using Xunit;

namespace Tachyon.Tests
{
    public class VarFacts
    {
        [Fact]
        public void Local_Var_should_not_allow_region_key_to_be_more_than_256_bytes()
        {
            var regionKey = new string('x', 1000);
            Action action = () => Vars.Local<IChannel<int>>(Encoding.UTF8.GetBytes(regionKey), Guid.NewGuid());

            action.Should().Throw<ArgumentException>().WithMessage($"Region's key cannot be longer than {byte.MaxValue} bytes");
        }
    }
}