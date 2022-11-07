using minimal_todo_app.Models;
using Microsoft.Azure.Cosmos;

namespace minimal_todo_app.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cosmosClient">Cosmosクライアント</param>
        /// <param name="databaseName">データベース名</param>
        /// <param name="containerName">コンテナ名</param>
        public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        /// <summary>
        /// クエリに一致するTodoアイテムを取得する
        /// </summary>
        /// <param name="queryString">SQLクエリ</param>
        /// <returns></returns>
        public async Task<IEnumerable<TodoItem>> GetItemsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<TodoItem>(new QueryDefinition(queryString));

            var results = new List<TodoItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <summary>
        /// Todoアイテムを1件取得する
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public async Task<TodoItem?> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<TodoItem> response = await _container.ReadItemAsync<TodoItem>(id, new PartitionKey(id));
                return response.Resource;
            }
            // 404 Not Foundの場合はnullを返す
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        /// <summary>
        /// Todoアイテムを登録する
        /// </summary>
        /// <param name="item">Todoアイテム</param>
        /// <returns></returns>
        public async Task AddItemAsync(TodoItem item)
        {
            await _container.CreateItemAsync<TodoItem>(item, new PartitionKey(item.Id));
        }

        /// <summary>
        /// Todoアイテムを更新する
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="item">Todoアイテム</param>
        /// <returns></returns>
        public async Task UpdateItemAsync(string id, TodoItem item)
        {
           await _container.UpsertItemAsync<TodoItem>(item, new PartitionKey(id));
        }

        /// <summary>
        /// Todoアイテムを削除する
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<TodoItem>(id, new PartitionKey(id));
        }
    }
}
