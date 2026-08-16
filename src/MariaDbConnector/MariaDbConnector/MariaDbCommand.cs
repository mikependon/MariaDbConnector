using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbCommand : DbCommand
    {
        private readonly MySqlCommand _command;
        private readonly MariaDbParameterCollection _parameters;
        private MariaDbConnection _connection;
        private MariaDbTransaction _transaction;

        public MariaDbCommand()
        {
            _command = new MySqlCommand();
            _parameters = new MariaDbParameterCollection(_command.Parameters);
        }

        public MariaDbCommand(string commandText)
        {
            _command = new MySqlCommand(commandText);
            _parameters = new MariaDbParameterCollection(_command.Parameters);
        }

        public MariaDbCommand(string commandText, MariaDbConnection connection)
        {
            _command = new MySqlCommand(commandText, connection.InnerConnection);
            _parameters = new MariaDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        internal MariaDbCommand(MySqlCommand command, MariaDbConnection connection)
        {
            _command = command;
            _parameters = new MariaDbParameterCollection(_command.Parameters);
            _connection = connection;
        }

        public new MariaDbParameterCollection Parameters => _parameters;

        public override string CommandText { get => _command.CommandText; set => _command.CommandText = value; }

        public override int CommandTimeout { get => _command.CommandTimeout; set => _command.CommandTimeout = value; }

        public override CommandType CommandType { get => _command.CommandType; set => _command.CommandType = value; }

        public override bool DesignTimeVisible { get => _command.DesignTimeVisible; set => _command.DesignTimeVisible = value; }

        public override UpdateRowSource UpdatedRowSource { get => _command.UpdatedRowSource; set => _command.UpdatedRowSource = value; }

        protected override DbConnection DbConnection
        {
            get => _connection;
            set
            {
                _connection = (MariaDbConnection)value;
                _command.Connection = _connection?.InnerConnection;
            }
        }

        protected override DbParameterCollection DbParameterCollection => _parameters;

        protected override DbTransaction DbTransaction
        {
            get => _transaction;
            set
            {
                _transaction = (MariaDbTransaction)value;
                _command.Transaction = _transaction?.InnerTransaction;
            }
        }

        public override void Cancel()
        {
            _command.Cancel();
        }

        public override int ExecuteNonQuery()
        {
            return _command.ExecuteNonQuery();
        }

        public override object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }

        public override void Prepare()
        {
            _command.Prepare();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new MariaDbParameter(_command.CreateParameter());
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new MariaDbDataReader(_command.ExecuteReader(behavior));
        }
    }
}
