using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MariaDbConnector
{
    /// <summary>
    /// The exception that is thrown when MariaDB returns an error.
    /// </summary>
    public class MariaDbException : DbException
    {
        private readonly MySqlException _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="MariaDbException"/> class.
        /// </summary>
        /// <param name="exception">The underlying <see cref="MySqlException"/> to wrap.</param>
        internal MariaDbException(MySqlException exception)
            : base(exception.Message, exception)
        {
            _exception = exception;
        }

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public override int ErrorCode => _exception.Number;

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public int Number => _exception.Number;

        /// <summary>
        /// Gets the SQL state.
        /// </summary>
        public string SqlState => _exception.SqlState;
    }
}
