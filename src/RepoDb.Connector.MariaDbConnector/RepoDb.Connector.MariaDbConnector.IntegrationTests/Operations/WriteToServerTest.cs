using RepoDb.Connector.MariaDbConnector.Bulk;
using RepoDb.Connector.MariaDbConnector.IntegrationTests.Setup;
using System.Data;

namespace RepoDb.Connector.MariaDbConnector.IntegrationTests.Operations
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

        [TestMethod]
        public void TestMariaDbBulkCopyWriteToServerWithSourceColumnAndDestinationColumnMappings()
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
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "`ColumnInt` = 5 AND `ColumnNVarChar` = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestMariaDbBulkCopyWriteToServerWithSourceOrdinalAndDestinationColumnMappings()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new MariaDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(i, table.Columns[i].ColumnName);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "`ColumnInt` = 5 AND `ColumnNVarChar` = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestMariaDbBulkCopyWriteToServerWithSourceColumnAndDestinationOrdinalMappings()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new MariaDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(table.Columns[i].ColumnName, i + 1);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "`ColumnInt` = 5 AND `ColumnNVarChar` = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void TestMariaDbBulkCopyWriteToServerWithSourceOrdinalAndDestinationOrdinalMappings()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new MariaDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    bulkCopy.ColumnMappings.Add(i, i + 1);
                }

                // Act
                bulkCopy.WriteToServer(table);

                // Assert
                Assert.AreEqual(table.Rows.Count, Helper.CountRows(connection, table.TableName));
                Assert.AreEqual(1, Helper.CountRowsWhere(connection, table.TableName,
                    "`ColumnInt` = 5 AND `ColumnNVarChar` = 'ColumnNVarChar5'"));
            }
        }

        [TestMethod]
        public void ThrowOnMariaDbBulkCopyWriteToServerWithOutOfRangeDestinationOrdinal()
        {
            using (var connection = new MariaDbConnection(Database.ConnectionString))
            {
                // Setup
                var table = Helper.CreateDataTable(10);
                var bulkCopy = new MariaDbBulkCopy(connection)
                {
                    DestinationTableName = table.TableName
                };
                bulkCopy.ColumnMappings.Add(0, 9);

                // Act & Assert
                Assert.Throws<IndexOutOfRangeException>(() => bulkCopy.WriteToServer(table));
            }
        }
    }
}
