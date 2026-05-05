using NUnit.Framework;
using NUnit.Framework.Legacy;
using Crystal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Crystal.EntityFrameworkCore.Tests.DecoratorTests
{
    [TestFixture]
    public class ListDecoratorTests
    {
        [Test]
        public void IsNullOrEmpty_NullList_ReturnsTrue()
        {
            List<int>? list = null;
            ClassicAssert.IsTrue(list!.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_EmptyList_ReturnsTrue()
        {
            ClassicAssert.IsTrue(new List<int>().IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NonEmptyList_ReturnsFalse()
        {
            ClassicAssert.IsFalse(new List<int> { 1 }.IsNullOrEmpty());
        }

        [Test]
        public void PickRandom_ReturnsElementFromList()
        {
            var list = new List<int> { 10, 20, 30 };
            var result = list.PickRandom();
            ClassicAssert.IsTrue(list.Contains(result));
        }

        [Test]
        public void Shuffle_ReturnsSameElements()
        {
            var list = new List<string> { "a", "b", "c" };
            var shuffled = ListDecorator.Shuffle(list).ToList();
            ClassicAssert.AreEqual(3, shuffled.Count);
            foreach (var item in list)
                ClassicAssert.IsTrue(shuffled.Contains(item));
        }
    }
}
