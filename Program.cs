using Microsoft.Data.Sqlite;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
var app = builder.Build();
app.UseSwagger().UseSwaggerUI();
using var Connection = new SqliteConnection("Data Source=test.db");
t1? SelectData(int id) => Connection.QueryFirstOrDefault<t1?>("Select * from t1 where id = @id", new { id });

int InsertData(string name) => Connection.Execute("insert into t1 (name) values (@name) ", new { name });
int UpdateData(t1 t1) => Connection.Execute("update t1 set name = @name where id = @id ", t1);
int DeleteData(int id) => Connection.Execute("delete from t1 where id = @id", new { id }); 

IEnumerable<t1> SelectAllData() => Connection.Query<t1>("SELECT * from t1");
app.MapGet("/", () =>SelectAllData() is not null ? Results.Ok(SelectAllData()):Results.NotFound());
app.MapGet("/{id:int}", (int id) =>SelectData(id) is not null ? Results.Ok(SelectData(id)) : Results.NotFound());
app.MapPost("/", (string name) => InsertData(name) > 0 ? Results.Created() : Results.Problem());
app.MapPut("/", (t1 t1) => UpdateData(t1) > 0 ? Results.Accepted() : Results.Problem());
app.MapDelete("/",(int id) =>DeleteData(id)>0?Results.NoContent():Results.NotFound());

app.Run();
record t1
{
    public int id { get; init; }
    public string name { get; init; }
}