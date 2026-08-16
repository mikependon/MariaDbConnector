using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbParameter : DbParameter
    {
        private readonly MySqlParameter _parameter;

        public MariaDbParameter()
        {
            _parameter = new MySqlParameter();
        }

        internal MariaDbParameter(MySqlParameter parameter)
        {
            _parameter = parameter;
        }

        internal MySqlParameter InnerParameter => _parameter;

        public override DbType DbType { get => _parameter.DbType; set => _parameter.DbType = value; }

        public override ParameterDirection Direction { get => _parameter.Direction; set => _parameter.Direction = value; }

        public override bool IsNullable { get => _parameter.IsNullable; set => _parameter.IsNullable = value; }

        public override string ParameterName { get => _parameter.ParameterName; set => _parameter.ParameterName = value; }

        public override int Size { get => _parameter.Size; set => _parameter.Size = value; }

        public override string SourceColumn { get => _parameter.SourceColumn; set => _parameter.SourceColumn = value; }

        public override bool SourceColumnNullMapping { get => _parameter.SourceColumnNullMapping; set => _parameter.SourceColumnNullMapping = value; }

        public override DataRowVersion SourceVersion { get => _parameter.SourceVersion; set => _parameter.SourceVersion = value; }

        public override object Value { get => _parameter.Value; set => _parameter.Value = value; }

        public override void ResetDbType()
        {
            _parameter.ResetDbType();
        }
    }
}
