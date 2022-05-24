using System;
namespace ToDoMVC.Models.ViewModels
{
    public class PagingInfo
    {
        public PagingInfo(int totalItems, int itemsPerPage, int currentPage)
        {
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}