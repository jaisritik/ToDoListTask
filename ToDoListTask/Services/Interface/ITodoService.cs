using ToDoListTask.Models;

namespace ToDoListTask.Services.Interface
{
    public interface ITodoService
    {
        IEnumerable<ToDoItem> GetAll();
        ToDoItem GetById(int id);
        ToDoItem Create(ToDoItem newItem);
        ToDoItem Update(int id, ToDoItem updatedItem);
        bool Delete(int id);
    }
}
