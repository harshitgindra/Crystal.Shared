using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Crystal.Dapper.Tests.UowTests
{
    public class BaseTests
    {
        private string _fileNameTemplate = "{0}.test.sqlite";
        protected IBaseUowRepository UowRepository { get; private set; }

        protected void Init()
        {
            string guid = Guid.NewGuid().ToString();
            string fileName  = string.Format(_fileNameTemplate, guid);

            IServiceCollection serviceCollection = new ServiceCollection()
                .ConfigureUnitOfWork<SqliteConnection>($"filename={fileName}");

            UowRepository = serviceCollection.BuildServiceProvider().GetService<IBaseUowRepository>();

            var cmd = UowRepository.Connection.CreateCommand();
            cmd.CommandText =
                "CREATE TABLE Product ( ProductId INTEGER primary key AUTOINCREMENT, Name text not null, Value int not null);";
            cmd.ExecuteNonQuery();
        }
    }
}
