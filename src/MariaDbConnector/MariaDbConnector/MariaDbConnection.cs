using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbConnection : DbConnection
    {
        private readonly MySqlConnection _connection;

        public MariaDbConnection()
        {
            _connection = new MySqlConnection();
        }

        public MariaDbConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        internal MySqlConnection InnerConnection => _connection;

        public override string ConnectionString {get => _connection.ConnectionString; set => _connection.ConnectionString = value; }

        public override string Database => _connection.Database;

        public override string DataSource => _connection.DataSource;

        public override string ServerVersion => _connection.ServerVersion;

        public override ConnectionState State => _connection.State;

        public override void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _connection.Close();
        }

        public override void Open()
        {
            _connection.Open();
        }

        public new MariaDbCommand CreateCommand()
        {
            return (MariaDbCommand)CreateDbCommand();
        }

        protected override DbTransaction BeginDbTransaction(
            IsolationLevel isolationLevel)
        {
            return new MariaDbTransaction((MySqlTransaction)_connection.BeginTransaction(isolationLevel), this);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MariaDbCommand((MySqlCommand)_connection.CreateCommand(), this);
        }
    }
}
