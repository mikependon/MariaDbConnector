using MariaDbConnector.IntegrationTests.Setup;

namespace MariaDbConnector.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteNonQueryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        [TestMethod]
        public void TestMariaDbExecuteNonQueryTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE `InsertModel`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteNonQueryWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE `InvalidTable`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteNonQuery());
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteNonQueryWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "TRUNCATE TABLE Invalid Table;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteNonQuery());
                }
            }
        }
    }
}
