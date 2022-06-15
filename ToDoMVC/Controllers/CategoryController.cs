using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;
using ToDoMVC.Models.ViewModels;
using ToDoMVC.Models;
using ToDoMVC.Repositories;

namespace ToDoMVC.Controllers
{
    public class CategoriesController : Controller
    {
        public static int PageSize = 30;
        public IRepository repository = new Repository();
        // GET: CategoriesController
        public ActionResult Index(int id = 1)
        {
            if (id < 1) id = 1;
            List<Category> categories = repository.GetPageOfCategories(id, PageSize);
            PagingInfo pagingInfo = repository.GetCategoriesPagingInfo(id, PageSize);
            CategoriesListViewModel categoriesListViewModel = new(categories, pagingInfo);
            return View(categoriesListViewModel);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View(repository.GetCategoryById(id));
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
                if (repository.CreateCategory(collection) == 1)
                    return RedirectToAction(nameof(Index));
                else
                    return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(repository.GetCategoryById(id));
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                //check
                if (repository.UpdateCategory(id, collection) == 1)
                    return RedirectToAction(nameof(Index));
                else
                    return View(repository.GetCategoryById(id));
            }
            catch
            {
                return View(repository.GetCategoryById(id));
            }
        }

        // GET: CategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(repository.GetCategoryById(id));
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (repository.DeleteCategory(id, collection) == 1)
                    return RedirectToAction(nameof(Index));
                else
                    return View(repository.GetCategoryById(id));
            }
            catch
            {
                return View(repository.GetCategoryById(id));
            }
        }
    }
}
