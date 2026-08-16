using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbTransaction : DbTransaction
    {
        private readonly MySqlTransaction _transaction;
        private readonly MariaDbConnection _connection;

        internal MariaDbTransaction(MySqlTransaction transaction, MariaDbConnection connection)
        {
            _transaction = transaction;
            _connection = connection;
        }

        internal MySqlTransaction InnerTransaction => _transaction;

        public override IsolationLevel IsolationLevel => _transaction.IsolationLevel;

        protected override DbConnection DbConnection => _connection;

        public override void Commit()
        {
            _transaction.Commit();
        }

        public override void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
