using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class MappingRepositoryTests : BaseTests
    {
        private BaseUowRepository _mappedUowRepository = null!;
        private string _dbFilePath = string.Empty;

        [SetUp]
        public async Task Setup()
        {
            Init();

            _dbFilePath = $"{Guid.NewGuid()}.mapping.test.sqlite";
            var connection = new SqliteConnection($"Filename={_dbFilePath}");
            await connection.OpenAsync();

            var cmd = connection.CreateCommand();
            cmd.CommandText =
                "CREATE TABLE Product ( ProductId INTEGER primary key AUTOINCREMENT, Name text not null, Value int not null);";
            cmd.ExecuteNonQuery();

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>()
                    .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.Name));
            }, NullLoggerFactory.Instance));

            _mappedUowRepository = new BaseUowRepository(connection, mapper);
            await _mappedUowRepository.Repository<Product>().InsertAsync(new List<Product>
            {
                new Product { Name = "Mapped 1", Value = 1 },
                new Product { Name = "Mapped 2", Value = 2 }
            });
        }

        [Test]
        public void GetAsyncTModel_WithoutMapper_Throws()
        {
            Assert.ThrowsAsync<Exception>(async () =>
                await UowRepository.Repository<Product>().GetAsync<ProductDto>());
        }

        [Test]
        public void FindAsyncTModel_WithoutMapper_Throws()
        {
            Assert.ThrowsAsync<Exception>(async () =>
                await UowRepository.Repository<Product>().FindAsync<ProductDto>(1));
        }

        [Test]
        public async Task GetAsyncTModel_WithMapper_ReturnsMappedModels()
        {
            var result = await _mappedUowRepository.Repository<Product>()
                .GetAsync<ProductDto>(orderBy: q => q.OrderByDescending(x => x.ProductId));

            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual("Mapped 2", result[0].ProductName);
            ClassicAssert.AreEqual(2, result[0].Value);
        }

        [Test]
        public async Task GetFirstOrDefaultAsyncTModel_WithMapper_ReturnsMappedModel()
        {
            var result = await _mappedUowRepository.Repository<Product>()
                .GetFirstOrDefaultAsync<ProductDto>(x => x.Name == "Mapped 1");

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Mapped 1", result.ProductName);
        }

        [Test]
        public async Task FindAsyncTModel_WithMapper_ReturnsMappedModel()
        {
            var entity = await _mappedUowRepository.Repository<Product>()
                .GetFirstOrDefaultAsync(x => x.Name == "Mapped 2");

            var result = await _mappedUowRepository.Repository<Product>()
                .FindAsync<ProductDto>(entity!.ProductId);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Mapped 2", result.ProductName);
        }

        [TearDown]
        public void TearDown()
        {
            _mappedUowRepository?.Dispose();
            UowRepository?.Dispose();

            if (!string.IsNullOrWhiteSpace(_dbFilePath) && System.IO.File.Exists(_dbFilePath))
            {
                System.IO.File.Delete(_dbFilePath);
            }
        }
    }
}
