using MariaDbConnector.IntegrationTests.Setup;

namespace MariaDbConnector.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteScalarTest
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
        public void TestMariaDbExecuteScalarTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteScalarTest'), (UUID(), 'ExecuteScalarTest'), (UUID(), 'ExecuteScalarTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                object result;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteScalarTest';";
                    result = command.ExecuteScalar();
                }

                // Assert
                Assert.AreEqual(3L, result);
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteScalarWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT COUNT(*) FROM `InvalidTable`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteScalar());
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteScalarWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC COUNT(*) FROM `InsertModel`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteScalar());
                }
            }
        }
    }
}
