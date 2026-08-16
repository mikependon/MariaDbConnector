using MariaDbConnector;
using System.Data;

namespace RepoDb.MariaDb.BulkOperations.IntegrationTests
{
    /// <summary>
    /// A helper class for the integration testing.
    /// </summary>
    public static class Helper
    {
        static Helper()
        {
            EpocDate = new DateTime(1970, 1, 1, 0, 0, 0);
        }

        #region Properties

        /// <summary>
        /// Gets the value of the Epoc date.
        /// </summary>
        public static DateTime EpocDate { get; }

        #endregion

        #region InsertModel

        /*
         * Actual Class
         */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="hasId"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(int count,
            bool hasId = false)
        {
            // TODO: Compose a table based on the properties of the InsertModel
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="commandText"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal static void ExecuteNonQuery(
            MariaDbConnection connect,
            string commandText)
        {
            // TODO: Execute the command text
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static int CountRows(
            MariaDbConnection connect,
            string tableName)
        {
            // TODO: Count the number of records of the table
            throw new NotImplementedException();
        }

        #endregion
    }
}
