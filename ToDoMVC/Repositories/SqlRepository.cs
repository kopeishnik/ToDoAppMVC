using Dapper;
using System.Data.SqlClient;
using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;

namespace ToDoMVC.Repositories
{
    public class SqlRepository : IRepository
    {
        DataBase _dataBase = new();
        public static List<Todo> GetAllTodos(int? categoryid = null)
        {
            SqlConnection sqlConnection = new(DataBase.SqlConnectionString);
            string query = "SELECT [Id], [Name] FROM [Categories] ORDER BY [Id] ASC";
            IEnumerable<Todo> categories = sqlConnection.Query<Todo>(query);
            if (categoryid != null)
            {
                query = "SELECT [Id], [Name] FROM [Categories] WHERE [] =  ORDER BY [Id] ASC";

            }
            return categories.ToList();
        }
        public static List<Category> GetAllCategories()
        {
            SqlConnection sqlConnection = new(DataBase.SqlConnectionString);
            string query = "SELECT [Id], [Name] FROM [Categories] ORDER BY [Id] ASC";
            var categories = sqlConnection.Query<Category>(query);
            return categories.ToList();
        }

        List<Todo> IRepository.GetPageOfTodos(int page, int pageSize)
        {
            string query = "SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id ORDER BY [IsDone] ASC, CASE WHEN [Deadline] IS NULL THEN 1 ELSE 0 END, [Deadline] ASC OFFSET @offsetnumber ROWS FETCH NEXT @pagesize ROWS ONLY";
            return _dataBase.GetConnection().Query<Todo>(query, new { offsetnumber = pageSize * (page - 1), pagesize = pageSize }).ToList();

        }

        PagingInfo IRepository.GetTodosPagingInfo(int page, int pageSize)
        {
            if (page < 1) page = 1;
            var tasksCount = _dataBase.GetConnection().QueryFirst<int>($"SELECT COUNT(*) FROM [Tasks]");
            return new PagingInfo(page, pageSize, tasksCount);
        }

        Todo? IRepository.GetTodoById(int id)
        {
            return _dataBase.GetConnection().QueryFirstOrDefault<Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { id });
        }

        Todo? IRepository.GetLastTodo()
        {
            int x = _dataBase.GetConnection().QueryFirstOrDefault<int>("SELECT IDENT_CURRENT('Tasks')");
            return _dataBase.GetConnection().QueryFirstOrDefault<Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { x });
        }

        int IRepository.CreateTodo(IFormCollection collection)
        {
            //insert to DB
            _dataBase.OpenConnection();
            //name
            SqlCommand command = new("INSERT INTO [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) VALUES(@tname, @deadline, @isdone, @categoryid)", _dataBase.GetConnection());
            command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
            //deadline
            if (DateTime.TryParse(collection["Deadline"], out DateTime deadline))
            {
                command.Parameters.AddWithValue("@deadline", deadline.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                command.Parameters.AddWithValue("@deadline", DBNull.Value);
            }
            //is done
            bool MyBoolValue = Convert.ToBoolean(collection["IsDone"].ToString().Split(',')[0]);
            command.Parameters.AddWithValue("@isdone", MyBoolValue == true ? 1 : 0);
            //category
            if (collection["CategoryName"] == "")
                command.Parameters.AddWithValue("@categoryid", DBNull.Value);
            else command.Parameters.AddWithValue("@categoryid", Convert.ToInt64(collection["CategoryName"]));
            //gaycheck for input
            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;
        }

        int IRepository.UpdateTodo(int id, IFormCollection collection)
        {
            // update 
            _dataBase.OpenConnection();
            SqlCommand command = new("UPDATE [Tasks] SET [Name] = @tname, [Deadline] = @deadline, [IsDone] = @isdone, [CategoryId] = @categoryid WHERE [Id] = @id", _dataBase.GetConnection());
            //name
            command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
            //deadline
            if (DateTime.TryParse(collection["Deadline"], out DateTime deadline))
            {
                command.Parameters.AddWithValue("@deadline", deadline.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                command.Parameters.AddWithValue("@deadline", DBNull.Value);
            }
            //is done
            bool MyBoolValue = Convert.ToBoolean(collection["IsDone"].ToString().Split(',')[0]);
            command.Parameters.AddWithValue("@isdone", MyBoolValue == true ? 1 : 0);
            //category
            if (collection["CategoryName"] == "")
                command.Parameters.AddWithValue("@categoryid", DBNull.Value);
            else command.Parameters.AddWithValue("@categoryid", Convert.ToInt64(collection["CategoryName"]));
            //id
            command.Parameters.AddWithValue("@id", id);
            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;

        }

        int IRepository.DeleteTodo(int id, IFormCollection collection)
        {
            _dataBase.OpenConnection();

            SqlCommand command = new("DELETE [Tasks] WHERE [Id] = @id", _dataBase.GetConnection());
            command.Parameters.AddWithValue("@id", id.ToString());

            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;
        }

        bool IRepository.TodoExists(int id)
        {
            Todo? x = _dataBase.GetConnection().QueryFirstOrDefault<Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { id });
            if (x == null) return false;
            return true;
        }

        List<Category> IRepository.GetPageOfCategories(int page, int pageSize)
        {
            string query = "SELECT [Id], [Name] FROM [Categories] ORDER BY [Id] ASC OFFSET @offsetnumber ROWS FETCH NEXT @pagesize ROWS ONLY";
            return _dataBase.GetConnection().Query<Category>(query, new { offsetnumber = pageSize * (page - 1), pagesize = pageSize }).ToList();
        }

        PagingInfo IRepository.GetCategoriesPagingInfo(int page, int pageSize)
        {
            if (page < 1) page = 1;
            var categoriesCount = _dataBase.GetConnection().QueryFirst<int>($"SELECT COUNT(*) FROM [Categories]");
            return new PagingInfo(page, pageSize, categoriesCount);
        }

        Category? IRepository.GetCategoryById(int id)
        {
            return _dataBase.GetConnection().QueryFirstOrDefault<Category>("SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id", new { id });
        }

        Category? IRepository.GetLastCategory()
        {
            int x = _dataBase.GetConnection().QuerySingle<int>("SELECT IDENT_CURRENT('Categories')");
            return _dataBase.GetConnection().QueryFirstOrDefault<Category>("SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id", new { x });
        }

        int IRepository.CreateCategory(IFormCollection collection)
        {
            _dataBase.OpenConnection();
            //name
            SqlCommand command = new("INSERT INTO [Categories]([Name]) VALUES(@tname)", _dataBase.GetConnection());
            command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;
        }

        int IRepository.UpdateCategory(int id, IFormCollection collection)
        {
            // update 

            _dataBase.OpenConnection();
            //SqlCommand command = new("INSERT INTO [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) VALUES(@tname, @deadline, @isdone, @categoryid)", _dataBase.GetConnection());

            SqlCommand command = new("UPDATE [Categories] SET [Name] = @tname WHERE [Id] = @id", _dataBase.GetConnection());
            //name
            command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
            //id
            command.Parameters.AddWithValue("@id", id);

            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;
        }

        int IRepository.DeleteCategory(int id, IFormCollection collection)
        {
            _dataBase.OpenConnection();
            SqlCommand command = new("DELETE [Categories] WHERE [Id] = @id", _dataBase.GetConnection());
            command.Parameters.AddWithValue("@id", id.ToString());
            int x = command.ExecuteNonQuery();
            _dataBase.CloseConnection();
            return x;
        }

        bool IRepository.CategoryExists(int id)
        {
            Category? x = _dataBase.GetConnection().QueryFirstOrDefault<Category>("SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id", new { id });
            if (x == null) return false;
            return true;
        }
    }
}
