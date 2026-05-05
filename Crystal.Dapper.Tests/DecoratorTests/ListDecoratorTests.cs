using NUnit.Framework;
using NUnit.Framework.Legacy;
using Crystal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Crystal.Dapper.Tests.DecoratorTests
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
        public void PickRandom_ReturnsOneElement()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var result = list.PickRandom();
            ClassicAssert.IsTrue(list.Contains(result));
        }

        [Test]
        public void PickRandom_WithCount_ReturnsCorrectCount()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var result = list.PickRandom(3).ToList();
            ClassicAssert.AreEqual(3, result.Count);
        }

        [Test]
        public void Shuffle_ReturnsSameElements()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var shuffled = ListDecorator.Shuffle(list).ToList();
            ClassicAssert.AreEqual(list.Count, shuffled.Count);
            foreach (var item in list)
            {
                ClassicAssert.IsTrue(shuffled.Contains(item));
            }
        }
    }
}
