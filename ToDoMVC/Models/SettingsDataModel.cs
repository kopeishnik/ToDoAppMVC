namespace ToDoMVC.Models
{
    public class SettingsDataModel
    {
        public SettingsDataModel(int todosPageSize, int categoriesPageSize, string storageType)
        {
            TodosPageSize = todosPageSize;
            CategoriesPageSize = categoriesPageSize;
            StorageType = storageType;
        }
        public int TodosPageSize { get; set; }
        public int CategoriesPageSize { get; set; }
        public string StorageType { get; set; }
    }
}
