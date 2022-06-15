using System.ComponentModel.DataAnnotations;

namespace ToDoMVC.Models
{
    [Serializable]
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
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "DeadLine")]
        public DateTime? Deadline { get; set; }
        [Display(Name = "IsDone")]
        public bool IsDone { get; set; }
        [Display(Name = "CategoryId")]
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
