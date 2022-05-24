namespace ToDoMVC.Models
{
    public class FakeTasksRepository : ITasksRepository
    {
        public FakeTasksRepository()
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
        public int GetFreeNumber()
        {
            int i = 0;
            while (Tasks.Find(x => x.Id == i) != null)
            {
                i++;
            }
            return i;
        }
    }
}
