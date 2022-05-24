namespace ToDoMVC.Models
{
    public interface ITasksRepository
    {
        List<Todo> Tasks { get; set; }
    }
}
