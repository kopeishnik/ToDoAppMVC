using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;

namespace ToDoMVC.Repositories
{
    public interface IRepository
    {
        public List<Todo> GetPageOfTodos(int page, int pageSize);
        public PagingInfo GetTodosPagingInfo(int page, int pageSize);
        public Todo? GetTodoById(int id);
        public Todo? GetLastTodo();
        public int CreateTodo(IFormCollection collection);
        public int UpdateTodo(int id, IFormCollection collection);
        public int DeleteTodo(int id, IFormCollection collection);
        public bool TodoExists(int id);
        public List<Category> GetPageOfCategories(int page, int pageSize);
        public PagingInfo GetCategoriesPagingInfo(int page, int pageSize);
        public Category? GetCategoryById(int id);
        public Category? GetLastCategory();
        public int CreateCategory(IFormCollection collection);
        public int UpdateCategory(int id, IFormCollection collection);
        public int DeleteCategory(int id, IFormCollection collection);
        public bool CategoryExists(int id);
        //List<Todo> Tasks { get; set; }
    }
}
