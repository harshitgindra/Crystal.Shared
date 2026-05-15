using Crystal.Shared;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests.DecoratorTests
{
    [TestFixture]
    public class StringDecoratorTests
    {
        [Test]
        public void IsNullEmptyWhiteSpace_NullString_ReturnsTrue()
        {
            string? s = null;
            ClassicAssert.IsTrue(s!.IsNullEmptyWhiteSpace());
        }

        [Test]
        public void IsNullEmptyWhiteSpace_EmptyString_ReturnsTrue()
        {
            ClassicAssert.IsTrue("".IsNullEmptyWhiteSpace());
        }

        [Test]
        public void IsNullEmptyWhiteSpace_WhiteSpace_ReturnsTrue()
        {
            ClassicAssert.IsTrue("   ".IsNullEmptyWhiteSpace());
        }

        [Test]
        public void IsNullEmptyWhiteSpace_NonEmptyString_ReturnsFalse()
        {
            ClassicAssert.IsFalse("hello".IsNullEmptyWhiteSpace());
        }
    }
}
