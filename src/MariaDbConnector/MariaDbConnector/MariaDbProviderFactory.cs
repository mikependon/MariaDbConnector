using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbProviderFactory : DbProviderFactory
    {
        public static readonly MariaDbProviderFactory Instance = new MariaDbProviderFactory();

        public override DbConnection CreateConnection()
        {
            return new MariaDbConnection();
        }

        public override DbCommand CreateCommand()
        {
            return new MariaDbCommand();
        }

        public override DbParameter CreateParameter()
        {
            return new MariaDbParameter();
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new MariaDbConnectionStringBuilder();
        }
    }
}
