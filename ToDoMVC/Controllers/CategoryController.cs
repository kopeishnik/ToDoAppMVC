using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;
using ToDoMVC.Models.ViewModels;
using ToDoMVC.Models;

namespace ToDoMVC.Controllers
{
    public class CategoriesController : Controller
    {
        public static int PageSize = 30;
        private DataBase DataBase = new();
        // GET: CategoriesController
        public ActionResult Index(int id = 1)
        {
            if (id < 1) id = 1;
            string query = "SELECT [Id], [Name] FROM [Categories] ORDER BY [Id] ASC OFFSET @offsetnumber ROWS FETCH NEXT @pagesize ROWS ONLY";
            var categories = DataBase.GetConnection().Query<Category>(query, new { offsetnumber = PageSize * (id - 1), pagesize = PageSize });
            var tasksCount = DataBase.GetConnection().QueryFirst<int>($"SELECT COUNT(*) FROM [Categories]");
            PagingInfo pagingInfo = new(tasksCount, PageSize, id);
            CategoriesListViewModel categoriesListViewModel = new(categories, pagingInfo);
            return View(categoriesListViewModel);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            string query = "SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id";
            return View(DataBase.GetConnection().QuerySingle<Models.Category>(query, new { id }));
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                DataBase.OpenConnection();
                //name
                SqlCommand command = new("INSERT INTO [Categories]([Name]) VALUES(@tname)", DataBase.GetConnection());
                command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
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

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            string query = "SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id";
            return View(DataBase.GetConnection().QuerySingle<Models.Category>(query, new { id }));
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // update 

                DataBase.OpenConnection();
                //SqlCommand command = new("INSERT INTO [Tasks]([Name], [Deadline], [IsDone], [CategoryId]) VALUES(@tname, @deadline, @isdone, @categoryid)", DataBase.GetConnection());

                SqlCommand command = new("UPDATE [Categories] SET [Name] = @tname WHERE [Id] = @id", DataBase.GetConnection());
                //name
                command.Parameters.AddWithValue("@tname", Convert.ToString(collection["Name"]));
                //id
                command.Parameters.AddWithValue("@id", id);
                //check
                if (command.ExecuteNonQuery() == 1)
                {
                    DataBase.CloseConnection();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    DataBase.CloseConnection();
                    return View(DataBase.GetConnection().QuerySingle<Models.Category>("SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id", new { id }));
                }
            }
            catch
            {
                return View(DataBase.GetConnection().QuerySingle<Models.Category>("SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id", new { id }));
            }
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            string query = "SELECT [Id], [Name] FROM [Categories] WHERE [Id] = @id";
            return View(DataBase.GetConnection().QuerySingle<Models.Category>(query, new { id }));
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                DataBase.OpenConnection();

                SqlCommand command = new("DELETE [Categories] WHERE [Id] = @id", DataBase.GetConnection());
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
                return View(nameof(Index));
            }
        }
    }
}
