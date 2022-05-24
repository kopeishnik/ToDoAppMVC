namespace ToDoMVC.Models
{
    public class SettingsDataModel
    {
        public SettingsDataModel(int todosPageSize, int categoriesPageSize)
        {
            TodosPageSize = todosPageSize;
            CategoriesPageSize = categoriesPageSize;
        }
        public int TodosPageSize { get; set; }
        public int CategoriesPageSize { get; set; }
    }
}
