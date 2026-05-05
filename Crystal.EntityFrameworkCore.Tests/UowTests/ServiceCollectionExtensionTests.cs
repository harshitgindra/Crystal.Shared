using NUnit.Framework;
using NUnit.Framework.Legacy;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class ServiceCollectionExtensionTests
    {
        [Test]
        public void ConfigureUnitOfWork_RegistersIBaseUowRepository()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TestContext>();
            services.ConfigureUnitOfWork<TestContext>();

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IBaseUowRepository>();
            ClassicAssert.IsNotNull(uow);
        }

        [Test]
        public void ConfigureUnitOfWork_ContextAlreadyRegistered_DoesNotDuplicate()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TestContext>();
            services.ConfigureUnitOfWork<TestContext>();
            services.ConfigureUnitOfWork<TestContext>(); // second call should not throw

            var provider = services.BuildServiceProvider();
            var uow = provider.GetService<IBaseUowRepository>();
            ClassicAssert.IsNotNull(uow);
        }

        [Test]
        public void ConfigureUnitOfWork_ContextNotRegistered_RegistersContextAndBaseContextMapping()
        {
            var services = new ServiceCollection();
            services.ConfigureUnitOfWork<TestContext>();

            var provider = services.BuildServiceProvider();
            var context = provider.GetService<TestContext>();
            var baseContext = provider.GetService<BaseContext>();

            ClassicAssert.IsNotNull(context);
            ClassicAssert.IsNotNull(baseContext);
            ClassicAssert.IsInstanceOf<TestContext>(baseContext);
        }

        [Test]
        public void ConfigureUnitOfWork_ContextNotRegistered_RegistersContextOnlyOnce()
        {
            var services = new ServiceCollection();
            services.ConfigureUnitOfWork<TestContext>();
            services.ConfigureUnitOfWork<TestContext>();

            var contextRegistrations = services.Count(x => x.ServiceType == typeof(TestContext));
            ClassicAssert.AreEqual(1, contextRegistrations);
        }
    }
}
