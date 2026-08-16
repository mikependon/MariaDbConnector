namespace MariaDbConnector.Bulk
{
    /// <summary>
    /// Defines the mapping between a column in a data source and a column in a destination table.
    /// </summary>
    public class MariaDbBulkColumnMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkColumnMapping"/> class, using ordinals to refer to source and destination columns.
        /// </summary>
        /// <param name="sourceColumnOrdinal">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationOrdinal">The ordinal position of the destination column within the destination table.</param>
        public MariaDbBulkColumnMapping(
            int sourceColumnOrdinal,
            int destinationOrdinal) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkColumnMapping"/> class, using an ordinal to refer to the source column and a name for the destination column.
        /// </summary>
        /// <param name="sourceColumnOrdinal">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        public MariaDbBulkColumnMapping(
            int sourceColumnOrdinal,
            string destinationColumn) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkColumnMapping"/> class, using a name to refer to the source column and an ordinal for the destination column.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationOrdinal">The ordinal position of the destination column within the destination table.</param>
        public MariaDbBulkColumnMapping(
            string sourceColumn,
            int destinationOrdinal) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkColumnMapping"/> class, using names to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        public MariaDbBulkColumnMapping(
            string sourceColumn,
            string destinationColumn) {
        }

        /// <summary>
        /// Gets or sets the name of the column being mapped in the destination table.
        /// </summary>
        public string DestinationColumn { get; set; }

        /// <summary>
        /// Gets or sets the ordinal value of the destination column within the destination table.
        /// </summary>
        public int DestinationOrdinal { get; set; }

        /// <summary>
        /// Gets or sets the name of the column being mapped in the source table.
        /// </summary>
        public string SourceColumn { get; set; }

        /// <summary>
        /// Gets or sets the ordinal position of the source column within the data source.
        /// </summary>
        public int SourceOrdinal { get; set; }
    }
}
