using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data.Common;

namespace MariaDbConnector
{
    public class MariaDbParameterCollection : DbParameterCollection
    {
        private readonly MySqlParameterCollection _parameters;

        internal MariaDbParameterCollection(MySqlParameterCollection parameters)
        {
            _parameters = parameters;
        }

        public override int Count => _parameters.Count;

        public override bool IsFixedSize => _parameters.IsFixedSize;

        public override bool IsReadOnly => _parameters.IsReadOnly;

        public override bool IsSynchronized => _parameters.IsSynchronized;

        public override object SyncRoot => _parameters.SyncRoot;

        public MariaDbParameter AddWithValue(string parameterName, object value)
        {
            return new MariaDbParameter(_parameters.AddWithValue(parameterName, value));
        }

        public override int Add(object value)
        {
            var parameter = ((MariaDbParameter)value).InnerParameter;
            _parameters.Add(parameter);
            return _parameters.IndexOf(parameter);
        }

        public override void AddRange(Array values)
        {
            foreach (var value in values)
            {
                Add(value);
            }
        }

        public override void Clear()
        {
            _parameters.Clear();
        }

        public override bool Contains(object value)
        {
            return _parameters.Contains(((MariaDbParameter)value).InnerParameter);
        }

        public override bool Contains(string value)
        {
            return _parameters.Contains(value);
        }

        public override void CopyTo(Array array, int index)
        {
            _parameters.CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (MySqlParameter parameter in _parameters)
            {
                yield return new MariaDbParameter(parameter);
            }
        }

        public override int IndexOf(object value)
        {
            return _parameters.IndexOf(((MariaDbParameter)value).InnerParameter);
        }

        public override int IndexOf(string parameterName)
        {
            return _parameters.IndexOf(parameterName);
        }

        public override void Insert(int index, object value)
        {
            _parameters.Insert(index, ((MariaDbParameter)value).InnerParameter);
        }

        public override void Remove(object value)
        {
            _parameters.Remove(((MariaDbParameter)value).InnerParameter);
        }

        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public override void RemoveAt(string parameterName)
        {
            _parameters.RemoveAt(parameterName);
        }

        protected override DbParameter GetParameter(int index)
        {
            return new MariaDbParameter(_parameters[index]);
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return new MariaDbParameter(_parameters[parameterName]);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            _parameters[index] = ((MariaDbParameter)value).InnerParameter;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            _parameters[parameterName] = ((MariaDbParameter)value).InnerParameter;
        }
    }
}
