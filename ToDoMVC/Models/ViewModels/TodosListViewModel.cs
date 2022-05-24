using System.Collections.Generic;
using ToDoMVC.Models;

namespace ToDoMVC.Models.ViewModels
{
    public class TodosListViewModel
    {
        public TodosListViewModel(IEnumerable<Models.Todo> todos, PagingInfo pagingInfo)
        {
            Todos = todos;
            PagingInfo = pagingInfo;
        }
        public IEnumerable<Models.Todo> Todos { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
