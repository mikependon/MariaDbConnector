using System.Collections;

namespace MariaDbConnector.Bulk
{
    /// <summary>
    /// Contains a collection of <see cref="MariaDbBulkColumnMapping"/> objects.
    /// </summary>
    public class MariaDbBulkCopyColumnMappingCollection : CollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbBulkCopyColumnMappingCollection"/> class.
        /// </summary>
        internal MariaDbBulkCopyColumnMappingCollection()
        {
        }

        /// <summary>
        /// Gets the <see cref="MariaDbBulkColumnMapping"/> object at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the mapping to retrieve.</param>
        public MariaDbBulkColumnMapping this[int index]
        {
            get { throw null; }
        }

        /// <summary>
        /// Adds the specified <see cref="MariaDbBulkColumnMapping"/> to the collection.
        /// </summary>
        /// <param name="bulkCopyColumnMapping">The mapping to add to the collection.</param>
        /// <returns>The <see cref="MariaDbBulkColumnMapping"/> that was added to the collection.</returns>
        public MariaDbBulkColumnMapping Add(
            MariaDbBulkColumnMapping bulkCopyColumnMapping)
        {
            throw null;
        }

        /// <summary>
        /// Creates and adds a <see cref="MariaDbBulkColumnMapping"/> using column ordinals to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="MariaDbBulkColumnMapping"/>.</returns>
        public MariaDbBulkColumnMapping Add(
            int sourceColumnIndex,
            int destinationColumnIndex)
        {
            throw null;
        }

        /// <summary>
        /// Creates and adds a <see cref="MariaDbBulkColumnMapping"/> using a column ordinal for the source column and a column name for the destination column.
        /// </summary>
        /// <param name="sourceColumnIndex">The ordinal position of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="MariaDbBulkColumnMapping"/>.</returns>
        public MariaDbBulkColumnMapping Add(
            int sourceColumnIndex,
            string destinationColumn)
        {
            throw null;
        }

        /// <summary>
        /// Creates and adds a <see cref="MariaDbBulkColumnMapping"/> using a column name for the source column and a column ordinal for the destination column.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumnIndex">The ordinal position of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="MariaDbBulkColumnMapping"/>.</returns>
        public MariaDbBulkColumnMapping Add(
            string sourceColumn,
            int destinationColumnIndex)
        {
            throw null;
        }

        /// <summary>
        /// Creates and adds a <see cref="MariaDbBulkColumnMapping"/> using column names to refer to both source and destination columns.
        /// </summary>
        /// <param name="sourceColumn">The name of the source column within the data source.</param>
        /// <param name="destinationColumn">The name of the destination column within the destination table.</param>
        /// <returns>The newly added <see cref="MariaDbBulkColumnMapping"/>.</returns>
        public MariaDbBulkColumnMapping Add(
            string sourceColumn,
            string destinationColumn)
        {
            throw null;
        }

        /// <summary>
        /// Removes all <see cref="MariaDbBulkColumnMapping"/> items from the collection.
        /// </summary>
        public new void Clear()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="MariaDbBulkColumnMapping"/> exists in the collection.
        /// </summary>
        /// <param name="value">The mapping to look for.</param>
        /// <returns>true if the mapping exists in the collection; otherwise false.</returns>
        public bool Contains(
            MariaDbBulkColumnMapping value)
        {
            throw null;
        }

        /// <summary>
        /// Copies all items from the collection into the specified array, starting at the specified index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(
            MariaDbBulkColumnMapping[] array,
            int index)
        {
        }

        /// <summary>
        /// Gets the index of the specified <see cref="MariaDbBulkColumnMapping"/> within the collection.
        /// </summary>
        /// <param name="value">The mapping to locate.</param>
        /// <returns>The zero-based index of the mapping within the collection.</returns>
        public int IndexOf(
            MariaDbBulkColumnMapping value)
        {
            throw null;
        }

        /// <summary>
        /// Inserts a <see cref="MariaDbBulkColumnMapping"/> into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the mapping should be inserted.</param>
        /// <param name="value">The mapping to insert.</param>
        public void Insert(
            int index,
            MariaDbBulkColumnMapping value)
        {
        }

        /// <summary>
        /// Removes the specified <see cref="MariaDbBulkColumnMapping"/> from the collection.
        /// </summary>
        /// <param name="value">The mapping to remove.</param>
        public void Remove(
            MariaDbBulkColumnMapping value)
        {
        }

        /// <summary>
        /// Removes the <see cref="MariaDbBulkColumnMapping"/> at the specified index from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the mapping to remove.</param>
        public new void RemoveAt(
            int index)
        {
        }
    }
}
