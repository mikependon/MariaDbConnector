namespace RepoDb.Adapter.MariaDb.Bulk
{
    /// <summary>
    /// Represents the behavior when conflicts arise during bulk loading operations.
    /// </summary>
    public enum MariaDbBulkLoaderConflictOption
    {
        /// <summary>
        /// This is the default and indicates normal operation. A key conflict will raise an error and the load operation is aborted.
        /// </summary>
        None,

        /// <summary>
        /// Replace column values when a key conflict occurs.
        /// </summary>
        Replace,

        /// <summary>
        /// Ignore any rows where the primary key conflicts.
        /// </summary>
        Ignore,
    }
}
