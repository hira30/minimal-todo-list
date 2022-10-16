using Microsoft.EntityFrameworkCore;
using todo_react_app.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<TodoContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext") ??
//     throw new InvalidOperationException("Connection string 'TodoContext' not found.")));
builder.Services.AddDbContext<TodoContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("TodoContext")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapFallbackToFile("index.html");;

app.Run();
