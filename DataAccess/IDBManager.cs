using System.Data;

namespace TestTask.DataAccess
{
    public interface IDBManager
    {
        public IDbConnection GetDatabaseConnection();
        public void CloseConnection(IDbConnection connection);
        public IDbDataParameter CreateParameter(string name, object value, DbType dbType);
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType);
        public IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction);
        public DataTable GetDataTable(string commandText, CommandType commandType, IDbDataParameter[]? parameters = null);
        public DataSet GetDataSet(string commandText, CommandType commandType, IDbDataParameter[]? parameters = null);
        public IDataReader GetDataReader(string commandText, CommandType commandType, IDbDataParameter[] parameters, out IDbConnection connection);
        public void Delete(string commandText, CommandType commandType, IDbDataParameter[]? parameters = null);
        public void Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters);
        public int Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out int lastId);
        public long Insert(string commandText, CommandType commandType, IDbDataParameter[] parameters, out long lastId);
        public void InsertWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters);
        public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters);
        public void Update(string commandText, CommandType commandType, IDbDataParameter[] parameters);
        public void UpdateWithTransaction(string commandText, CommandType commandType, IDbDataParameter[] parameters);
        public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, IDbDataParameter[] parameters);
        public object GetScalarValue(string commandText, CommandType commandType, IDbDataParameter[] parameters = null);
    }
}
