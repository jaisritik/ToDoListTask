using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using ToDoListTask.Models;
using ToDoListTask.Services.Interface;

namespace ToDoListTask.Services
{

    public class TodoService : ITodoService
    {
        private readonly List<ToDoItem> _todoItems;

        public TodoService()
        {
            _todoItems = new List<ToDoItem>();
            InitializeTodoItems();
        }
        private void InitializeTodoItems()
        {
            _todoItems.AddRange(new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Title = "Buy groceries", Description = "Milk, Eggs, Bread", IsCompleted = false },
                new ToDoItem { Id = 2, Title = "Clean the house", Description = "Living room, Kitchen, Bathroom", IsCompleted = false },
                new ToDoItem { Id = 3, Title = "Read a book", Description = "Finish reading 'C# in Depth'", IsCompleted = true }
            });
        }

        public ToDoItem Create(ToDoItem newItem)
        {
            // Check if the provided ID already exists
            if (newItem.Id != 0 && _todoItems.Any(i => i.Id == newItem.Id))
            {
                throw new Exception($"TODO item with ID {newItem.Id} already exists.");
            }
            var newId = _todoItems.Max(i => i.Id) + 1;
            // Assign a new ID if it's not already set
            if (newItem.Id == 0) // Assuming 0 means the client did not provide an ID
            {
                newItem.Id = _todoItems.Count == 0 ? 1 : newId;
            }

            _todoItems.Add(newItem);
            return newItem;
        }


        public bool Delete(int id)
        {
            var res = _todoItems.Where(t=> t.Id == id).FirstOrDefault();
            if (res == null)
            {
                throw new KeyNotFoundException("Invalid Id");
            }
            _todoItems.Remove(res);
            return true;
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _todoItems.ToList();
        }

        public ToDoItem GetById(int id)
        {
            var res = _todoItems.Where(t => t.Id == id).FirstOrDefault();
            if (res == null)
            {
                throw new KeyNotFoundException("Invalid Id");
            }
            return res;
        }

        public ToDoItem Update(int id, ToDoItem updatedItem)
        {
            var existingItem = _todoItems.Where(t => t.Id == id).FirstOrDefault();
            if (existingItem == null)
            {
                throw new KeyNotFoundException("Invalid Id");
            }
            existingItem.Id = id;
            existingItem.Title = updatedItem.Title;
            existingItem.Description = updatedItem.Description;
            existingItem.IsCompleted = updatedItem.IsCompleted;
            return updatedItem;
        }
    }
}
