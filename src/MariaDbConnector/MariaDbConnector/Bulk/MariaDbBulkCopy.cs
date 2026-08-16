using System;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector.Bulk
{
    /// <summary>
    /// Lets you efficiently bulk load a MariaDB table with data from another source.
    /// </summary>
    public class MariaDbBulkCopy : IDisposable
    {
        private readonly MariaDbConnection _connection;

        // TODO:    Let us not support the MariaDbBulkCopyOptions for now.
        //          We can later extend this when there is a need.

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkCopy"/> class using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the destination database.</param>
        public MariaDbBulkCopy(
            string connectionString)
            : this(new MariaDbConnection(connectionString))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkCopy"/> class using the specified <see cref="MariaDbConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="MariaDbConnection"/> to the destination database.</param>
        public MariaDbBulkCopy(
            MariaDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbBulkCopy"/>.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the number of rows in each batch. At the end of each batch, the rows are sent to the server.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds for the operation to complete before it times out.
        /// </summary>
        public int BulkCopyTimeout { get; set; }

        /// <summary>
        /// Gets the number of rows copied during the current bulk copy operation.
        /// </summary>
        public int RowsCopied { get; private set; }

        /// <summary>
        /// Copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            IDataReader reader)
        {
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DbDataReader reader)
        {
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DataTable table)
        {
        }

        /// <summary>
        /// Copies only rows that match the supplied row state in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        /// <param name="rowState">A value from the <see cref="DataRowState"/> enumeration used to filter which rows are copied.</param>
        public void WriteToServer(
            DataTable table,
            DataRowState rowState)
        {
        }

        /// <summary>
        /// Copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        public void WriteToServer(
            DataRow[] rows)
        {
        }
    }
}
