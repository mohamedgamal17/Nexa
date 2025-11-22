using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using Testcontainers.MsSql;

namespace Nexa.Accounting.Application.Tests
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
                InitialCatalog = "AccountingTestDb"
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
