using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MariaDbConnector.Bulk
{
    /// <summary>
    /// Lets you efficiently bulk load a MariaDB table with data from another source.
    /// </summary>
    public class MariaDbBulkCopy : IDisposable
    {
        private readonly MariaDbConnection _connection;
        private const string _fieldTerminator = "\t";
        private const string _lineTerminator = "\n";

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
            ColumnMappings = new MariaDbBulkCopyColumnMappingCollection();
            _connection = connection;
        }

        #region IDisposable

        /// <summary>
        /// Releases the resources used by the <see cref="MariaDbBulkCopy"/>.
        /// </summary>
        public void Dispose() => _connection.Dispose();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MariaDbBulkCopyColumnMappingCollection ColumnMappings { get; private set; }

        /// <summary>
        /// Gets or sets the number of rows in each batch. At the end of each batch, the rows are sent to the server.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds for the operation to complete before it times out.
        /// </summary>
        public int BulkCopyTimeout { get; set; }

        /// <summary>
        /// Gets or sets the destination table name to where to bulk copy the data.
        /// </summary>
        public string DestinationTableName { get; set; }

        /// <summary>
        /// Gets the number of rows copied during the current bulk copy operation.
        /// </summary>
        public int RowsCopied { get; private set; }

        #endregion

        #region WriteToServer

        /// <summary>
        /// Copies all rows in the supplied <see cref="IDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            IDataReader reader)
        {
            RowsCopied = WriteToServerInternal(reader);
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DbDataReader"/> to the destination table.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DbDataReader reader)
        {
            RowsCopied = WriteToServerInternal(reader);
        }

        /// <summary>
        /// Copies all rows in the supplied <see cref="DataTable"/> to the destination table.
        /// </summary>
        /// <param name="table">The <see cref="DataTable"/> that provides the rows to copy.</param>
        public void WriteToServer(
            DataTable table)
        {
            var rows = new DataRow[table.Rows.Count];
            table.Rows.CopyTo(rows, 0);
            RowsCopied = WriteToServerInternal(rows, table.Columns.Count);
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
            var rows = SelectRows(table, rowState);
            RowsCopied = WriteToServerInternal(rows, table.Columns.Count);
        }

        /// <summary>
        /// Copies all rows in the supplied array of <see cref="DataRow"/> objects to the destination table.
        /// </summary>
        /// <param name="rows">The array of <see cref="DataRow"/> objects that provide the rows to copy.</param>
        public void WriteToServer(
            DataRow[] rows)
        {
            var columnCount = rows != null && rows.Length > 0 ? rows[0].Table.Columns.Count : 0;
            RowsCopied = WriteToServerInternal(rows, columnCount);
        }

        #endregion

        #region Internals

        #region WriteToServer

        /// <summary>
        /// Writes every remaining row of <paramref name="reader"/> to <see cref="DestinationTableName"/>.
        /// </summary>
        /// <param name="reader">
        /// The source reader. Read positionally via each mapping's <see cref="MySqlBulkCopyColumnMapping.SourceOrdinal"/> -
        /// see the remarks on <see cref="MySqlBulkCopy"/> for why no other ordinal of this reader is ever touched.
        /// </param>
        /// <returns>The number of rows <see cref="MySqlBulkLoader"/> reports having loaded.</returns>
        private int WriteToServerInternal(IDataReader reader) =>
            WriteToServerInternalAsync(reader, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronous counterpart of <see cref="WriteToServer(IDataReader)"/>.
        /// </summary>
        public async Task<int> WriteToServerInternalAsync(IDataReader reader,
            CancellationToken cancellationToken = default)
        {
            var filePath = GetTempFilePath();
            try
            {
                using (var writer = CreateFileWriter(filePath))
                {
                    while (reader.Read())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        WriteRow(writer, ordinal => reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal));
                    }
                }
                return await LoadAsync(filePath, cancellationToken);
            }
            finally
            {
                DeleteTempFile(filePath);
            }
        }

        /// <summary>
        /// Writes <paramref name="rows"/> to <see cref="DestinationTableName"/>.
        /// </summary>
        /// <param name="rows">The source rows, read positionally via each mapping's <see cref="MySqlBulkCopyColumnMapping.SourceOrdinal"/> - i.e. the real <see cref="DataTable"/> column index, not the mapping's position in <see cref="ColumnMappings"/> (those can legitimately diverge - see <c>WriteToServer.cs</c>'s remarks on why the ordinal must come from <c>DataTable.Columns.IndexOf</c>).</param>
        /// <param name="columnCount">Unused beyond a defensive bounds check - <see cref="ColumnMappings"/> alone already determines exactly which columns get written.</param>
        /// <returns>The number of rows <see cref="MySqlBulkLoader"/> reports having loaded.</returns>
        public int WriteToServerInternal(DataRow[] rows,
            int columnCount) =>
            WriteToServerInternalAsync(rows, columnCount, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronous counterpart of <see cref="WriteToServer(DataRow[], int)"/>.
        /// </summary>
        public async Task<int> WriteToServerInternalAsync(DataRow[] rows,
            int columnCount,
            CancellationToken cancellationToken = default)
        {
            var filePath = GetTempFilePath();
            try
            {
                using (var writer = CreateFileWriter(filePath))
                {
                    foreach (var row in rows ?? Array.Empty<DataRow>())
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        WriteRow(writer, ordinal => ordinal < columnCount && ordinal < row.Table.Columns.Count && !row.IsNull(ordinal) ? row[ordinal] : null);
                    }
                }
                return await LoadAsync(filePath, cancellationToken);
            }
            finally
            {
                DeleteTempFile(filePath);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Writes one row to <paramref name="writer"/> - one tab-separated field per entry in
        /// <see cref="ColumnMappings"/>, in that order, terminated by <see cref="LineTerminator"/>.
        /// </summary>
        /// <param name="writer">The temp file writer.</param>
        /// <param name="getValue">Resolves a source ordinal to its raw CLR value (or <see langword="null"/> for a database <c>NULL</c>).</param>
        private void WriteRow(StreamWriter writer,
            Func<int, object> getValue)
        {
            for (var i = 0; i < ColumnMappings.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(_fieldTerminator);
                }
                writer.Write(FormatValue(getValue(ColumnMappings[i].SourceOrdinal)));
            }
            writer.Write(_lineTerminator);
        }

        /// <summary>
        /// Formats a single CLR value for the temp file, using <c>LOAD DATA</c>'s own documented escape
        /// sequences (<c>\N</c> for <c>NULL</c>, <c>\\</c>/<c>\t</c>/<c>\n</c>/<c>\r</c> for the literal
        /// characters that would otherwise be misread as field/line structure).
        /// </summary>
        private static string FormatValue(object value)
        {
            if (value == null || value is DBNull)
            {
                return "\\N";
            }

            return value switch
            {
                Guid guid => guid.ToString("D", CultureInfo.InvariantCulture),
                DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture),
                bool boolean => boolean ? "1" : "0",
                byte[] bytes => BitConverter.ToString(bytes).Replace("-", string.Empty),
                IFormattable formattable => Escape(formattable.ToString(null, CultureInfo.InvariantCulture)),
                _ => Escape(value.ToString()),
            };
        }

        /// <summary>
        /// Backslash-escapes the handful of characters <c>LOAD DATA</c> would otherwise misinterpret as field
        /// structure: a literal backslash, tab (the field terminator), and CR/LF (the line terminator).
        /// </summary>
        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value ?? string.Empty;
            }

            StringBuilder builder = null;
            for (var i = 0; i < value.Length; i++)
            {
                var ch = value[i];
                var escaped = ch switch
                {
                    '\\' => "\\\\",
                    '\t' => "\\t",
                    '\n' => "\\n",
                    '\r' => "\\r",
                    _ => null,
                };
                if (escaped != null)
                {
                    builder ??= new StringBuilder(value, 0, i, value.Length + 8);
                    builder.Append(escaped);
                }
                else
                {
                    builder?.Append(ch);
                }
            }
            return builder?.ToString() ?? value;
        }

        /// <summary>
        /// Runs the configured <see cref="MySqlBulkLoader"/> against the temp file built by <see cref="WriteRow"/>, back-tick-quoting each mapped destination column along the way - the
        /// counterpart to <see cref="DestinationTableName"/> already arriving pre-quoted (see its remarks).
        /// </summary>
        private async Task<int> LoadAsync(string filePath,
            CancellationToken cancellationToken)
        {
            var bulkLoader = new MariaDbBulkLoader(_connection)
            {
                TableName = DestinationTableName,
                FileName = filePath,
                Local = true,
                FieldTerminator = _fieldTerminator,
                LineTerminator = _lineTerminator,
                NumberOfLinesToSkip = 0,
                CharacterSet = "utf8mb4",
                Timeout = BulkCopyTimeout,
            };
            foreach (var mapping in ColumnMappings)
            {
                bulkLoader.Columns.Add(QuoteIdentifier(((MariaDbBulkColumnMapping)mapping).DestinationColumn));
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await bulkLoader.LoadAsync();
        }

        /// <summary>
        /// Back-tick-quotes a raw identifier, doubling any embedded back-tick per MySQL's identifier-quoting
        /// escape rule.
        /// </summary>
        private static string QuoteIdentifier(string identifier) =>
            $"`{identifier?.Replace("`", "``")}`";

        /// <summary>
        /// Selects the rows of <paramref name="table"/> whose <see cref="DataRow.RowState"/> matches any of the
        /// flags set in <paramref name="rowState"/>, mirroring <see cref="DataRowState"/>'s flags semantics.
        /// </summary>
        /// <param name="table">The table to select rows from.</param>
        /// <param name="rowState">The row state flags to match.</param>
        private static DataRow[] SelectRows(DataTable table, DataRowState rowState)
        {
            var rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                if ((row.RowState & rowState) != 0)
                {
                    rows.Add(row);
                }
            }
            return rows.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetTempFilePath() => Path.GetTempFileName();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static StreamWriter CreateFileWriter(string filePath) =>
            new(filePath, false, new UTF8Encoding(false));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private static void DeleteTempFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (IOException)
            {
                // Best-effort cleanup - a lingering temp file is a minor annoyance, not worth failing an
                // otherwise-successful (or already-failed) bulk load over.
            }
        }

        #endregion
        #endregion
    }
}
