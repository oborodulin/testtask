using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.DataAccess
{
    public class DAL
    {
        public static int executeQuery(string query)
        {
            // Initialization.
            int rowCount = 0;
            string strConn = "Data Source=SQL Server Name(e.g. localhost);Database=SQL Database Name;User Id=SQL User Name;Password=SQL Password;";
            SqlConnection sqlConnection = new SqlConnection(strConn);
            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                // Settings.
                sqlCommand.CommandText = query;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandTimeout = 12 * 3600; //// Setting timeeout for longer queries to 12 hours.

                // Open.
                sqlConnection.Open();

                // Result.
                rowCount = sqlCommand.ExecuteNonQuery();

                // Close.
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                // Close.
                sqlConnection.Close();

                throw ex;
            }
            return rowCount;
        }
    }
}