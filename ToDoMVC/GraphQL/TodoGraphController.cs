using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;
using ToDoMVC.Repositories;
using ToDoMVC.Controllers;

namespace ToDoMVC.GraphQL
{
    public class TodoGraphController : GraphController
    {
        IRepository repository = new Repository();
        
        [QueryRoot("todo")]
        public Todo? RetrieveTodo(int id)
        {
            return repository.GetTodoById(id);
        }
        [QueryRoot("todos")]
        public List<Todo> RetrieveAllTodos(int? categoryid = null)
        {
            return Repository.GetAllTodos(categoryid);
        }
        [QueryRoot("todosPage")]
        public List<Todo> RetrieveTodosPage(int page = 1)
        {
            if (page < 1) page = 1;
            return repository.GetPageOfTodos(page, Controllers.TodoController.PageSize);
        }
        [QueryRoot("todosPageInfo")]
        public PagingInfo RetrieveTodosPageInfo(int page = 1)
        {
            return repository.GetTodosPagingInfo(page, TodoController.PageSize);
        }
        [MutationRoot("createTodo")]
        public Todo? CreateTodo(string name, DateTime? deadline, bool isDone, int? categoryId)
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dictionary = new()
            {
                { "Name", (Microsoft.Extensions.Primitives.StringValues)name.ToString() },
                { "Deadline", (Microsoft.Extensions.Primitives.StringValues)( deadline == null ? "" : deadline.ToString()) },
                { "IsDone", (Microsoft.Extensions.Primitives.StringValues)(isDone == true ? "1" : "0") },
                { "CategoryName", (Microsoft.Extensions.Primitives.StringValues)(categoryId == null ? "" : categoryId.ToString()) }
            };
            IFormCollection x = new FormCollection(dictionary);
            repository.CreateTodo(x);
            return repository.GetLastTodo();
        }
        [MutationRoot("editTodo")]
        public Todo? EditTodo(int id, string name, DateTime? deadline, bool isDone, int? categoryId)
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dictionary = new()
            {
                { "Name", (Microsoft.Extensions.Primitives.StringValues)name.ToString() },
                { "Deadline", (Microsoft.Extensions.Primitives.StringValues)( deadline == null ? "" : deadline.ToString()) },
                { "IsDone", (Microsoft.Extensions.Primitives.StringValues)(isDone == true ? "1" : "0") },
                { "CategoryName", (Microsoft.Extensions.Primitives.StringValues)(categoryId == null ? "" : categoryId.ToString()) }
            };
            IFormCollection formdata = new FormCollection(dictionary);
            repository.UpdateTodo(id, formdata);
            Todo? todo = repository.GetTodoById(id);
            return todo;
        }
        [MutationRoot("deleteTodo")]
        public Todo? DeleteTodo(int id)
        {
            Todo? todo = repository.GetTodoById(id);
            repository.DeleteTodo(id, new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));
            return todo;
        }
        [QueryRoot("category")]
        public Todo? RetrieveCategory(int id)
        {
            return repository.GetTodoById(id);
        }
        [QueryRoot("categories")]
        public List<Category> RetrieveAllCategories()
        {
            return Repository.GetAllCategories();
        }
        [QueryRoot("categoriesPage")]
        public List<Category> RetrieveCategoriesPage(int page)
        {
            if (page < 1) page = 1;
            return repository.GetPageOfCategories(page, Controllers.TodoController.PageSize);
        }
        [QueryRoot("categoriesPageInfo")]
        public PagingInfo RetrieveCategoriesPageInfo(int page = 1)
        {
            return repository.GetCategoriesPagingInfo(page, CategoriesController.PageSize);
        }
        [MutationRoot("createCategory")]
        public Category? CreateCategory(string name)
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dictionary = new()
            {
                { "Name", (Microsoft.Extensions.Primitives.StringValues)name.ToString() }
            };
            IFormCollection x = new FormCollection(dictionary);
            repository.CreateCategory(x);
            return repository.GetLastCategory();
        }
        [MutationRoot("editCategory")]
        public Category? EditCategory(int id, string name)
        {
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> dictionary = new()
            {
                { "Name", (Microsoft.Extensions.Primitives.StringValues)name.ToString() }
            };
            IFormCollection formdata = new FormCollection(dictionary);
            repository.UpdateCategory(id, formdata);
            Category? todo = repository.GetCategoryById(id);
            return todo;
        }
        [MutationRoot("deleteCategory")]
        public Category? DeleteCategory(int id)
        {
            Category? todo = repository.GetCategoryById(id);
            repository.DeleteTodo(id, new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));
            return todo;
        }
        [QueryRoot("storage")]
        public string SetStorage(string typeName)
        {
            if (typeName == "sql") Repository.StorageType = Storage.Sql;
            else if (typeName == "xml") Repository.StorageType = Storage.Xml;
            else typeName = "error";
            return Repository.StorageType == Storage.Sql ? "sql" : Repository.StorageType == Storage.Sql ? "xml" : "error";
        }
        [QueryRoot("getStorageType")]
        public string GetStorage()
        {
            return Repository.StorageType == Storage.Sql ? "Sql" : "Xml";
        }
        [QueryRoot("setSettings")]
        public SettingsDataModel SetSettings(int todoItemsPerPage, int categoryItemsPerPage, string storageType)
        {
            if (todoItemsPerPage < 5 || todoItemsPerPage > 100) todoItemsPerPage = TodoController.PageSize;
            else TodoController.PageSize = todoItemsPerPage;
            if (categoryItemsPerPage < 5 || categoryItemsPerPage > 100) categoryItemsPerPage = CategoriesController.PageSize;
            else CategoriesController.PageSize = categoryItemsPerPage;
            if (storageType == "sql") Repository.StorageType = Storage.Sql;
            else if (storageType == "xml") Repository.StorageType = Storage.Xml;
            else storageType = "error";
            SettingsDataModel settingsData = new SettingsDataModel(todoItemsPerPage, categoryItemsPerPage, storageType);
            return settingsData;
        }
    }
}
