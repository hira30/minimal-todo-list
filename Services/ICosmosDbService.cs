using minimal_todo_app.Models;

namespace minimal_todo_app.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<TodoItem>> GetItemsAsync(string query);
        Task<TodoItem?> GetItemAsync(string id);
        Task AddItemAsync(TodoItem item);
        Task UpdateItemAsync(string id, TodoItem item);
        Task DeleteItemAsync(string id);
    }
}
