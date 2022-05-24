using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoMVC.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using ToDoMVC.Models.ViewModels;

namespace ToDoMVC.Controllers
{
    public class TodoController : Controller
    {
        public static int PageSize = 30;
        //private FakeTasksRepository repository = new FakeTasksRepository();
        private DataBase DataBase = new();
        public TodoController()
        {
            //repository = new FakeTasksRepository();
        }
        // GET: Todo
        public ViewResult Index(int id = 1)
        {
            if (id < 1) id = 1;
            //select everything
            string query = "SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id ORDER BY [IsDone] ASC, CASE WHEN [Deadline] IS NULL THEN 1 ELSE 0 END, [Deadline] ASC OFFSET @offsetnumber ROWS FETCH NEXT @pagesize ROWS ONLY";
            var todos = DataBase.GetConnection().Query<Models.Todo>(query, new { offsetnumber = PageSize * (id - 1), pagesize = PageSize });
            var tasksCount = DataBase.GetConnection().QueryFirst<int>($"SELECT COUNT(*) FROM [Tasks]");
            PagingInfo pagingInfo = new (tasksCount, PageSize, id);
            TodosListViewModel todosListViewModel = new(todos, pagingInfo);
            return View(todosListViewModel);
            //return View(repository.Tasks.OrderBy(p => p.Id).Skip((id - 1) * PageSize).Take(PageSize));
        }

        // GET: Todo/Details/5
        public ActionResult Details(int id)
        {
            // select by id 

            return View(DataBase.GetConnection().QuerySingle<Models.Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { id }));

            //SqlCommand command = new("SELECT [Name], [Deadline], [IsDone], [CategoryId] FROM [Tasks] WHERE [Id] = @id", DataBase.GetConnection());
            //command.Parameters.AddWithValue("@id", id);
            //
            //SqlDataReader dr = command.ExecuteReader();
            //_ = command.ExecuteScalar();
            //
            //if (dr.Read())
            //{
            //    string? namenullable = dr.GetValue(0).ToString(); dr.GetString(0);
            //    string name = dr.GetString(0); //dr.GetValue(0).ToString();
            //    DateTime? deadline = dr.GetDateTime(1); //Convert.ToDateTime(dr.GetValue(1));
            //    int? categoryid = dr.GetInt32(3); //Convert.ToInt32(dr.GetValue(3));
            //    if (categoryid == 0) categoryid = null;
            //    bool isdone = dr.GetBoolean(2);
            //    dr.Close();
            //    Models.Todo task = new(id, name, deadline, isdone, categoryid);
            //    return View(task);
            //}
            //else
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(repository.Tasks.First(c => c.Id == id));
        }

        // GET: Todo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                //insert to DB

                //return View(DataBase.GetConnection().QuerySingle<Models.Todo>("SELECT [Id], [Name], [Deadline], [IsDone], [CategoryId] FROM [Tasks] WHERE [Id] = @id", new { id }));


                //_todoRepository.Add(new SmartIT.Employee.MockDB.Todo() { Name = collection["Name"});

                DataBase.OpenConnection();
                //name
                SqlCommand command = new("INSERT INTO [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) VALUES(@tname, @deadline, @isdone, @categoryid)", DataBase.GetConnection());
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
                //if (!int.TryParse(collection["CategoryId"], out int category))
                //    command.Parameters.AddWithValue("@categoryid", DBNull.Value);
                //else if (Convert.ToInt32(collection["CategoryId"]) == 0)
                //    command.Parameters.AddWithValue("@categoryid", DBNull.Value);
                //else command.Parameters.AddWithValue("@categoryid", Convert.ToInt32(collection["CategoryId"]));
                //new category
                if (collection["CategoryName"] == "")
                    command.Parameters.AddWithValue("@categoryid", DBNull.Value);
                else command.Parameters.AddWithValue("@categoryid", Convert.ToInt64(collection["CategoryName"]));
                //gaycheck for input
                if (command.ExecuteNonQuery() == 1)
                {
                    DataBase.CloseConnection();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DataBase.CloseConnection();
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Todo/Edit/5
        public ActionResult Edit(int id)
        {
            // select by id 

            return View(DataBase.GetConnection().QuerySingle<Models.Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { id }));

            //SqlCommand command = new("SELECT FROM [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) WHERE [Id] = @id", DataBase.GetConnection());
            //command.Parameters.AddWithValue("@id", id);
            //
            //SqlDataReader dr = command.ExecuteReader();
            //
            //if (dr.Read())
            //{
            //    string? namenullable = dr.GetValue(0).ToString();
            //    string name;
            //    if (namenullable != null)
            //    {
            //        name = namenullable;
            //        DateTime? deadline = Convert.ToDateTime(dr.GetValue(1));
            //        bool isdone = Convert.ToBoolean(dr.GetValue(2));
            //        int? categoryid = Convert.ToInt32(dr.GetValue(2));
            //        if (categoryid == 0) categoryid = null;
            //        dr.Close();
            //        Models.Todo task = new(id, name, deadline, isdone, categoryid);
            //        return View(task);
            //    }
            //}
            //return RedirectToAction(nameof(Index));
            //return View(repository.Tasks.First(c => c.Id == id));
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // update 

                DataBase.OpenConnection();
                //SqlCommand command = new("INSERT INTO [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) VALUES(@tname, @deadline, @isdone, @categoryid)", DataBase.GetConnection());

                SqlCommand command = new("UPDATE [Tasks] SET [Name] = @tname, [Deadline] = @deadline, [IsDone] = @isdone, [CategoryId] = @categoryid WHERE [Id] = @id", DataBase.GetConnection());
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
                
                if (command.ExecuteNonQuery() == 1)
                {
                    DataBase.CloseConnection();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DataBase.CloseConnection();
                    return View(DataBase.GetConnection().QuerySingle<Todo>("SELECT [Id], [Name], [Deadline], [IsDone], [CategoryId] FROM [Tasks] WHERE [Id] = @id", new { id }));
                }

                //var current = repository.Tasks.First(c => c.Id == id);
                //current.Name = collection["Name"];
                //current.Deadline = Convert.ToDateTime(collection["Deadline"]);
                //current.IsDone = Convert.ToBoolean(collection["IsDone"]);
                //current.CategoryId = Convert.ToInt32(collection["CategoryId"]);
                //repository.Tasks.Add(new Models.Todo())
            }
            catch
            {
                return View(DataBase.GetConnection().QuerySingle<Todo>("SELECT [Id], [Name], [Deadline], [IsDone], [CategoryId] FROM [Tasks] WHERE [Id] = @id", new { id }));
            }
        }

        // GET: Todo/Delete/5
        public ActionResult Delete(int id)
        {
            return View(DataBase.GetConnection().QuerySingle<Models.Todo>("SELECT [Tasks].[Id], [Tasks].[Name], [Deadline], [IsDone], [CategoryId], [Categories].[Name] AS CategoryName FROM [Tasks] LEFT JOIN [Categories] ON [Tasks].CategoryId = [Categories].Id WHERE [Tasks].[Id] = @id", new { id }));

        }

        // POST: Todo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //var taskToDelete = repository.Tasks.Find(x => x.Id == id);
                //if (taskToDelete != null)
                //{
                //    repository.Tasks.Remove(taskToDelete);
                //}

                DataBase.OpenConnection();

                SqlCommand command = new("DELETE [Tasks] WHERE [Id] = @id", DataBase.GetConnection());
                command.Parameters.AddWithValue("@id", id.ToString());

                if (command.ExecuteNonQuery() == 1)
                {
                    DataBase.CloseConnection();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DataBase.CloseConnection();
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Home/Settings
        public ActionResult Settings()
        {
            return View(new SettingsDataModel(TodoController.PageSize, CategoriesController.PageSize));
        }

        // POST: Home/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(int id, IFormCollection collection)
        {
            try
            {
                int todopagesize = Convert.ToInt32(collection["TodosPageSize"]);
                int categorypagesize = Convert.ToInt32(collection["CategoriesPageSize"]);
                TodoController.PageSize = todopagesize;
                CategoriesController.PageSize = categorypagesize;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new SettingsDataModel(TodoController.PageSize, CategoriesController.PageSize));
            }
        }
    }
}
