using MariaDbConnector.Bulk;
using MariaDbConnector.IntegrationTests.Setup;
using System.Data;

namespace MariaDbConnector.IntegrationTests.Operations
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
                var bulkCopy = new MariaDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                foreach (DataColumn column in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
            }
        }
    }
}
