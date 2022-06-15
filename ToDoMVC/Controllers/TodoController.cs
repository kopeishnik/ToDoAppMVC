using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoMVC.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using ToDoMVC.Models.ViewModels;
using ToDoMVC.Repositories;
//using GraphQL.AspNet.Attributes;
//using GraphQL.AspNet.Controllers;

namespace ToDoMVC.Controllers
{
    public class TodoController : Controller
    {
        public static int PageSize = 30;
        //private DataBase _dataBase = new();
        private readonly IRepository repository;
        public TodoController()
        {
            repository = new Repository();
        }
        // GET: Todo
        public ViewResult Index(int id = 1) //, string? categoryName
        {
            if (id < 1) id = 1;
            List<Todo> todos = repository.GetPageOfTodos(id, PageSize);
            PagingInfo pagingInfo = repository.GetTodosPagingInfo(id, PageSize);
            return View(new TodosListViewModel(todos, pagingInfo));
        }

        // GET: Todo/Details/5
        public ActionResult Details(int id)
        {
            // select by id 
            return View(repository.GetTodoById(id));
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
                if (repository.CreateTodo(collection) == 1)
                    return RedirectToAction(nameof(Index));
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Todo/Edit/5
        public ActionResult Edit(int id)
        {
            return View(repository.GetTodoById(id));
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                if (repository.UpdateTodo(id, collection) == 1)
                    return RedirectToAction(nameof(Index));
                else
                    return View(repository.GetTodoById(id));
            }
            catch
            {
                return View(repository.GetTodoById(id));
            }
        }

        // GET: Todo/Delete/5
        public ActionResult Delete(int id)
        {
            return View(repository.GetTodoById(id));

        }

        // POST: Todo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (repository.DeleteTodo(id, collection) == 1)
                    return RedirectToAction(nameof(Index));
                else 
                    return View(repository.GetCategoryById(id));
            }
            catch
            {
                return View(repository.GetCategoryById(id));
            }
        }
        // GET: Home/Settings
        public ActionResult Settings()
        {
            return View(new SettingsDataModel(PageSize, CategoriesController.PageSize, Repository.StorageType == Storage.Sql ? "Sql" : "Xml"));
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
                PageSize = todopagesize;
                CategoriesController.PageSize = categorypagesize;
                string str = collection["StorageType"].ToString();
                if (str == "Sql") Repository.StorageType = Storage.Sql;
                else Repository.StorageType = Storage.Xml;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new SettingsDataModel(PageSize, CategoriesController.PageSize, Repository.StorageType == Storage.Sql ? "Sql" : "Xml"));
            }
        }
    }
}
