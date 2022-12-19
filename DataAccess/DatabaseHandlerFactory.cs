using System.Configuration;
using System.IO;

namespace TestTask.DataAccess
{
    public class DatabaseHandlerFactory
    {
        private ConnectionStringSettings connectionStringSettings;

        public DatabaseHandlerFactory(string connectionStringName)
        {
            connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
        }

        public IDatabaseHandler CreateDatabase()
        {
            IDatabaseHandler database = null;
            string connString = connectionStringSettings.ConnectionString;
            connString.Replace("|DataDirectory|", Directory.GetCurrentDirectory());

            switch (connectionStringSettings.ProviderName.ToLower())
            {
                case "system.data.sqlclient":
                    database = new SqlDataAccess(connString);
                    break;
                case "system.data.oracleclient":
                    //database = new OracleDataAccess(connectionStringSettings.ConnectionString);
                    break;
                case "system.data.oleDb":
                    //database = new OledbDataAccess(connectionStringSettings.ConnectionString);
                    break;
                case "system.data.odbc":
                    database = new OdbcDataAccess(connString);
                    break;
                default:
                    break;
            }

            return database;
        }

        public string GetProviderName()
        {
            return connectionStringSettings.ProviderName;
        }
    }
}
