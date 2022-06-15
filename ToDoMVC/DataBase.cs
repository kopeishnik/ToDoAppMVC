using System.Data.SqlClient;

namespace ToDoMVC
{
    public class DataBase
    {
        private static string sqlConnectionString = @"Data Source=localhost;Initial Catalog=AmogusDB;Integrated Security=True";
        public static string SqlConnectionString => sqlConnectionString;

        readonly SqlConnection sqlConnection = new(SqlConnectionString);

        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
