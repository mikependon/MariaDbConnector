using System.Data;

namespace RepoDb.Adapter.MariaDb.UnitTests
{
    [TestClass]
    public sealed class MariaDbConnectionTest
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=TestDb;User ID=root;Password=password;";

        [TestMethod]
        public void TestMariaDbConnectionDataSourceForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.DataSource;

            // Assert
            Assert.AreEqual("localhost", output);
        }

        [TestMethod]
        public void TestMariaDbConnectionDatabaseForConstructorWithConnectionString()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.Database;

            // Assert
            Assert.AreEqual("TestDb", output);
        }

        [TestMethod]
        public void TestMariaDbConnectionStateForNewConnection()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            var output = connection.State;

            // Assert
            Assert.AreEqual(ConnectionState.Closed, output);
        }

        [TestMethod]
        public void TestMariaDbConnectionCreateCommandForReturnsMariaDbCommand()
        {
            // Setup
            using var connection = new MariaDbConnection(ConnectionString);

            // Act
            using var output = connection.CreateCommand();

            // Assert
            Assert.IsInstanceOfType<MariaDbCommand>(output);
        }
    }
}
