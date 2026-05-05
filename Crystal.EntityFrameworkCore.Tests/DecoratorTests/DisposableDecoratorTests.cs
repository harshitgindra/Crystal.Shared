using NUnit.Framework;
using NUnit.Framework.Legacy;
using Crystal.Shared;
using System;

namespace Crystal.EntityFrameworkCore.Tests.DecoratorTests
{
    [TestFixture]
    public class DisposableDecoratorTests
    {
        private class DisposableObject : IDisposable
        {
            public bool Disposed { get; private set; }
            public void Dispose() => Disposed = true;
        }

        private class NonDisposableObject { }

        [Test]
        public void TryDispose_DisposableObject_CallsDispose()
        {
            var obj = new DisposableObject();
            obj.TryDispose();
            ClassicAssert.IsTrue(obj.Disposed);
        }

        [Test]
        public void TryDispose_NonDisposableObject_DoesNotThrow()
        {
            var obj = new NonDisposableObject();
            Assert.DoesNotThrow(() => obj.TryDispose());
        }

        [Test]
        public void TryDispose_NullObject_DoesNotThrow()
        {
            object? obj = null;
            Assert.DoesNotThrow(() => obj!.TryDispose());
        }
    }
}
