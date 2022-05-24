using System.Data.SqlClient;

namespace ToDoMVC
{
    public class DataBase
    {
        SqlConnection sqlConnection = new(@"Data Source=localhost;Initial Catalog=AmogusDB;Integrated Security=True");

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
