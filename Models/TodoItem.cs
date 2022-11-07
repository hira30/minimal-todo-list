using Newtonsoft.Json;

namespace minimal_todo_app.Models
{
    public class TodoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }
    }
}
