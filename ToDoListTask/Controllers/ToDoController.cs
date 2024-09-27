using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoListTask.Models;
using ToDoListTask.Services.Interface;

namespace ToDoListTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;         
        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo
        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = _todoService.GetAll();
            return Ok(todos);
        }

        // GET: api/todo/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todo = _todoService.GetById(id);
            if (todo == null)
                return NotFound(new { Message = "Todo not found" });

            return Ok(todo);
        }

        // POST: api/todo
        [HttpPost]
        public IActionResult Create([FromBody]ToDoItem newItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdItem = _todoService.Create(newItem);
            return Ok(createdItem);
        }

        // PUT: api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, ToDoItem updatedItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var todo = _todoService.Update(id, updatedItem);
            if (todo == null)
                return NotFound(new { Message = "Todo not found" });

            return Ok(todo);
        }

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _todoService.Delete(id);
            return NotFound();
        }
    }
}
