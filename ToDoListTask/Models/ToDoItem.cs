using System.ComponentModel.DataAnnotations;

namespace ToDoListTask.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}
