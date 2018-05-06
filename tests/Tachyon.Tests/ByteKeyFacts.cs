using System;
using FluentAssertions;
using FsCheck.Xunit;
using Tachyon.Core;
using Xunit;

namespace Tachyon.Tests
{
    public class ByteKeyFacts
    {
        [Property]
        public void ByteKey_should_be_structurally_different_for_different_arrays(Guid x, Guid y)
        {
            ByteKey key1 = x.ToByteArray();
            ByteKey key2 = y.ToByteArray();

            (key1 == key2).Should().BeFalse();
            (key2 == key1).Should().BeFalse();
        }

        [Property]
        public void ByteKey_should_be_structurally_comparable_when_arrays_have_the_same_content_but_different_references(byte[] expected)
        {
            var other = new byte[expected.Length];
            expected.CopyTo(other, 0);

            ByteKey key1 = expected;
            ByteKey key2 = other;

            ReferenceEquals(other, expected).Should().BeFalse();

            (key1 == key2).Should().BeTrue();
            (key2 == key1).Should().BeTrue();
        }
    }
}
