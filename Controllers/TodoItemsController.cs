using Microsoft.AspNetCore.Mvc;
using minimal_todo_app.Models;
using minimal_todo_app.Services;

namespace minimal_todo_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public TodoItemsController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            var result = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return result.ToList();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string id)
        {
            var todoItem = await _cosmosDbService.GetItemAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        [HttpPut]
        public async Task<IActionResult> Put(string id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _cosmosDbService.UpdateItemAsync(id, item);
            return NoContent();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post(TodoItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            await _cosmosDbService.AddItemAsync(item);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todoItem = await _cosmosDbService.GetItemAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _cosmosDbService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
