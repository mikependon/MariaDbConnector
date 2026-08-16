using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbConnection : DbConnection
    {
        private readonly MySqlConnection _connection;

        public MariaDbConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

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

        protected override DbTransaction BeginDbTransaction(
            IsolationLevel isolationLevel)
        {
            return _connection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return _connection.CreateCommand();
        }
    }
}
