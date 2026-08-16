using MariaDbConnector.IntegrationTests.Setup;

namespace MariaDbConnector.IntegrationTests.Operations
{
    [TestClass]
    public class ExecuteReaderTest
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
        public void TestMariaDbExecuteReaderTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                connection.Open();
                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText =
                        "INSERT INTO `InsertModel` (`RowGuid`, `ColumnNVarChar`) VALUES " +
                        "(UUID(), 'ExecuteReaderTest'), (UUID(), 'ExecuteReaderTest');";
                    insertCommand.ExecuteNonQuery();
                }

                // Act
                var rowCount = 0;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM `InsertModel` WHERE `ColumnNVarChar` = 'ExecuteReaderTest';";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rowCount++;
                        }
                    }
                }

                // Assert
                Assert.AreEqual(2, rowCount);
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteReaderWithInvalidTable()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELECT * FROM `InvalidTable`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteReader());
                }
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbExecuteReaderWithInvalidSyntax()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var commandText = "SELEC * FROM `InsertModel`;";

                // Act
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    Assert.Throws<MariaDbException>(() => command.ExecuteReader());
                }
            }
        }
    }
}
