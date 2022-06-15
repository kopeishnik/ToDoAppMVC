using ToDoMVC.Models;
using ToDoMVC.Models.ViewModels;

namespace ToDoMVC.Repositories
{
    public class FakeTodoRepository : IRepository
    {
        public FakeTodoRepository()
        {
            Tasks = new List<Todo>
            {
                new (1, "do dishes", new DateTime(2022, 5, 15, 18, 0, 0), true, null, null),
                new (2, "do labs", new DateTime(2022, 5, 15, 18, 0, 0), false, null, null),
                new (3, "work out", new DateTime(2022, 5, 15, 18, 0, 0), false, null, null),
                new (4, "go shopping", null, false, null, null)
            };
        }
        public List<Todo> Tasks { get; set; }

        public int DeleteCategory(int id, IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public int DeleteTodo(int id, IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public PagingInfo GetCategoriesPagingInfo(int categoriesCount, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public PagingInfo GetCategoriesPagingInfo(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetFreeNumber()
        {
            int i = 0;
            while (Tasks.Find(x => x.Id == i) != null)
            {
                i++;
            }
            return i;
        }

        public List<Todo> GetPageOfCategories(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<Todo> GetPageOfTodos(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Todo GetTodoById(int id)
        {
            throw new NotImplementedException();
        }

        public PagingInfo GetTodosPagingInfo(int todosCount, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public PagingInfo GetTodosPagingInfo(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public int CreateCategory(IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public int CreateTodo(IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public int UpdateCategory(int id, IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public int UpdateTodo(int id, IFormCollection collection)
        {
            throw new NotImplementedException();
        }

        public Todo? GetLastTodo()
        {
            throw new NotImplementedException();
        }

        public bool TodoExists(int id)
        {
            throw new NotImplementedException();
        }

        List<Category> IRepository.GetPageOfCategories(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Category? GetLastCategory()
        {
            throw new NotImplementedException();
        }

        public bool CategoryExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
