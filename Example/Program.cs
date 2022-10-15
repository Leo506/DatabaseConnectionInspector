
using System.Data;
using System.Net;
using DbConnectionInspector.Abstractions;
using DbConnectionInspector.Connections;
using DbConnectionInspector.Extensions;
using Example.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("postgres");
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(connString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Here the common usage of library
// but you can can use one of another ways
/*app.UseDbConnectionInspector(new ConnectionOptions()
{
    Connections = new []
    {
        new PostgresConnectionChecker()
        {
            ConnectionString = connString
        }
    }
});*/

// You may not provide any connection instances
// In this case checking would be always succeed
/* app.UseDbConnectionInspector(new ConnectionOptions()); */

// You may specify what action would be invoked if connection failed
/*app.UseDbConnectionInspector(new ConnectionOptions()
{
    Connections = new []
    {
        new PostgresConnection()
        {
            ConnectionString = connString
        }
    }
}, context => context.Response.StatusCode = (int)HttpStatusCode.BadRequest);*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();