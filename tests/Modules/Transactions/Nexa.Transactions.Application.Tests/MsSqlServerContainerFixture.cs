using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
namespace Nexa.Transactions.Application.Tests
{
    [SetUpFixture]

    public class MsSqlServerContainerFixture
    {
        private readonly MsSqlContainer _msSqlContainer;
        public static string ConnectionString { get; private set; }


        public MsSqlServerContainerFixture()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2017-latest")
                .WithAutoRemove(true)
                .Build();
        }

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            await _msSqlContainer.StartAsync();

            var connectionStringBuilder = new SqlConnectionStringBuilder(_msSqlContainer.GetConnectionString())
            {
                InitialCatalog = "TransactionsTestDb"
            };

            ConnectionString = connectionStringBuilder.ToString();
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            await _msSqlContainer.StopAsync();

            await _msSqlContainer.DisposeAsync();

        }

    }
}
