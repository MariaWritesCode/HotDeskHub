using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Persistence.IntegrationTests
{
    public abstract class SqliteTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<DataContext> _contextOptions;

        public SqliteTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new DataContext(_contextOptions);

            context.Database.EnsureCreated();

            SeedDatabase(context);

            context.SaveChanges();
        }

        protected virtual void SeedDatabase(DataContext context) {}

        protected DataContext CreateContext() => new DataContext(_contextOptions);
        
        public void Dispose() => _connection.Dispose();
    }
}