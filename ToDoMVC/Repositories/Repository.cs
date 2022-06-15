using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;

namespace ToDoMVC.Repositories
{
    public enum Storage { Sql, Xml }
    public class Repository : IRepository
    {
        private static Storage storageType = Storage.Sql;
        public static Storage StorageType { get => storageType; set => storageType = value; }
        private IRepository ThisRepository = new SqlRepository();
        private SqlRepository ThisSqlRepository = new();
        private XmlRepository ThisXmlRepository = new();
        private void ResolveRepo()
        {
            if (storageType == Storage.Sql) ThisRepository = ThisSqlRepository;
            else ThisRepository = ThisXmlRepository;
        }
        public static List<Category> GetAllCategories()
        {
            return storageType == Storage.Sql ? SqlRepository.GetAllCategories() : XmlRepository.GetAllCategories();
        }
        public static List<Todo> GetAllTodos(int? categoryid = null)
        {
            return storageType == Storage.Sql ? SqlRepository.GetAllTodos(categoryid) : XmlRepository.GetAllTodos(categoryid);
        }
        List<Todo> IRepository.GetPageOfTodos(int page, int pageSize)
        {
            ResolveRepo();
            return ThisRepository.GetPageOfTodos(page, pageSize);
        }

        PagingInfo IRepository.GetTodosPagingInfo(int page, int pageSize)
        {
            ResolveRepo();
            return ThisRepository.GetTodosPagingInfo(page, pageSize);
        }

        Todo? IRepository.GetTodoById(int id)
        {
            ResolveRepo();
            return ThisRepository.GetTodoById(id);
        }

        Todo? IRepository.GetLastTodo()
        {
            ResolveRepo();
            return ThisRepository.GetLastTodo();
        }

        int IRepository.CreateTodo(IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.CreateTodo(collection);
        }

        int IRepository.UpdateTodo(int id, IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.UpdateTodo(id, collection);
        }

        int IRepository.DeleteTodo(int id, IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.DeleteTodo(id, collection);
        }

        bool IRepository.TodoExists(int id)
        {
            ResolveRepo();
            return ThisRepository.TodoExists(id);
        }

        List<Category> IRepository.GetPageOfCategories(int page, int pageSize)
        {
            ResolveRepo();
            return ThisRepository.GetPageOfCategories(page, pageSize);
        }

        PagingInfo IRepository.GetCategoriesPagingInfo(int page, int pageSize)
        {
            ResolveRepo();
            return ThisRepository.GetCategoriesPagingInfo(page, pageSize);
        }

        Category? IRepository.GetCategoryById(int id)
        {
            ResolveRepo();
            return ThisRepository.GetCategoryById(id);
        }

        Category? IRepository.GetLastCategory()
        {
            ResolveRepo();
            return ThisRepository.GetLastCategory();
        }

        int IRepository.CreateCategory(IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.CreateCategory(collection);
        }

        int IRepository.UpdateCategory(int id, IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.UpdateCategory(id, collection);
        }

        int IRepository.DeleteCategory(int id, IFormCollection collection)
        {
            ResolveRepo();
            return ThisRepository.UpdateCategory(id, collection);
        }

        bool IRepository.CategoryExists(int id)
        {
            ResolveRepo();
            return ThisRepository.CategoryExists(id);
        }
    }
}
