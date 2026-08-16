using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbException : DbException
    {
        private readonly MySqlException _exception;

        internal MariaDbException(MySqlException exception)
            : base(exception.Message, exception)
        {
            _exception = exception;
        }

        public override int ErrorCode => _exception.Number;

        public int Number => _exception.Number;

        public string SqlState => _exception.SqlState;
    }
}
