namespace ToDoMVC.Models
{
    public class Todo
    {
        public Todo(int id, string name, DateTime? deadline, bool isDone, int? categoryId, string? categoryName)
        {
            Id = id;
            Name = name;
            Deadline = deadline;
            IsDone = isDone;
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsDone { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
