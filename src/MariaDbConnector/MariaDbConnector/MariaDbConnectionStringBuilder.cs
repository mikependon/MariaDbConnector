using MySql.Data.MySqlClient;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbConnectionStringBuilder : DbConnectionStringBuilder
    {
        private readonly MySqlConnectionStringBuilder _builder;

        public MariaDbConnectionStringBuilder()
        {
            _builder = new MySqlConnectionStringBuilder();
        }

        public MariaDbConnectionStringBuilder(string connectionString)
        {
            _builder = new MySqlConnectionStringBuilder(connectionString);
        }

        public new string ConnectionString { get => _builder.ConnectionString; set => _builder.ConnectionString = value; }

        public string Server { get => _builder.Server; set => _builder.Server = value; }

        public uint Port { get => _builder.Port; set => _builder.Port = value; }

        public string Database { get => _builder.Database; set => _builder.Database = value; }

        public string UserId { get => _builder.UserID; set => _builder.UserID = value; }

        public string Password { get => _builder.Password; set => _builder.Password = value; }
    }
}
