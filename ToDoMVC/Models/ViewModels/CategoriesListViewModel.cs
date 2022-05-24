using System.Collections.Generic;
using ToDoMVC.Models;

namespace ToDoMVC.Models.ViewModels
{
    public class CategoriesListViewModel
    {
        public CategoriesListViewModel(IEnumerable<Category> categories, PagingInfo pagingInfo)
        {
            Categories = categories;
            PagingInfo = pagingInfo;
        }
        public IEnumerable<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
