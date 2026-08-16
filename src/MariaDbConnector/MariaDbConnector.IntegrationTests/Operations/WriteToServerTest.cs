using MariaDbConnector;
using MariaDbConnector.Bulk;
using RepoDb.IntegrationTests.Setup;

namespace RepoDb.MariaDb.BulkOperations.IntegrationTests.Operations
{
    [TestClass]
    public class WriteToServerTest
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
        public void TestMariaDbBulkCopyWriteToServerTest()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new MariaDbBulkCopy(connection);

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
            }
        }
    }
}
