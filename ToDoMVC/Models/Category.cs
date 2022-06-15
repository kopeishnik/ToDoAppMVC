using System.ComponentModel.DataAnnotations;

namespace ToDoMVC.Models
{
    [Serializable]
    public class Category
    {
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
        [Key]
        [Display(Name="Id")]
        public int Id { get; set; }
        [Display(Name="Name")]
        public string Name { get; set; }
    }
}
