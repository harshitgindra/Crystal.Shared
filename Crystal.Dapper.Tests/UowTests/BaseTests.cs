using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Crystal.Dapper.Tests.UowTests
{
    public class BaseTests
    {
        private readonly string _filename = "test.sqlite";
        public IServiceCollection ServiceCollection { get; private set; }
        public IBaseUowRepository UowRepository { get; private set; }

        protected void Init()
        {
            if (File.Exists(_filename))
            {
                File.Delete(_filename);
            }

            ServiceCollection = new ServiceCollection()
                .ConfigureUnitOfWork<SqliteConnection>($"filename={_filename}");

            UowRepository = this.ServiceCollection.BuildServiceProvider().GetService<IBaseUowRepository>();

            var cmd = UowRepository.Connection.CreateCommand();
            cmd.CommandText =
                "CREATE TABLE Product ( ProductId INTEGER primary key AUTOINCREMENT, Name text not null, Value int not null);";
            cmd.ExecuteNonQuery();
        }
    }
}
