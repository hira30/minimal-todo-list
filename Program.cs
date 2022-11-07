using Microsoft.Azure.Cosmos;
using minimal_todo_app.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Cosmosクライアントの初期化処理
static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
{
    // appsettings.jsonからCosmosDBの設定値を取得
    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var account = configurationSection["Account"];
    var key = configurationSection["Key"];

    // CosmosClientとCosmosDbServiceのインスタンスを生成
    var client = new CosmosClient(account, key);
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);

    // データベースが存在しない場合は新たに作成する
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

    // コンテナが存在しない場合は新たに作成する
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

    return cosmosDbService;
}

// Cosmosクライアントのシングルトンインスタンスを生成
builder.Services.AddSingleton<ICosmosDbService>(
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapFallbackToFile("index.html");;

app.Run();
