# Connection Inspector
Library for inspect database connections and prevent requests to its until establish conneciton (for Asp.Net apps)


## Library Purpose
Library purpose is to try to help achieve max app reliability by handling database connection state. That is if your database server was down app would continue it's work

## How to use library
Here the example of how you can use this library in your Asp.Net project (from Example project)

### Common use
In your `Program.cs` or `Startup.cs` file:
```c#
app.UseDbConnectionInspector(new ConnectionOptions()
{
    new ConnectionChecker(new NpgsqlConnection(connString)),
    new ConnectionChecker(new MySqlConnection(connString))
});
```
You can provide `key` parameter
```c#
app.UseDbConnectionInspector(new ConnectionOptions()
{
    new ConnectionChecker(new NpgsqlConnection(connString), "chekerOne"),
    new ConnectionChecker(new MySqlConnection(connString), "checkerTwo")
});
```

In your controller:
```c#
[RequireDbInspection]
[HttpGet("/get")]
public IActionResult Get()
{
    return Ok(_dbContext.Models.ToList());
}
```

If you want check only one specific connection:
```c#
[RequireDbInspection("checkerKey")]
[HttpGet("/get")]
public IActionResult Get()
{
    return Ok(_dbContext.Models.ToList());
}
```

### Without connections
In this case checking will be always succeed whether inspection is required or not
```c#
app.UseDbConnectionInspector(new ConnectionOptions());
```

### Specify action
You may specify what action would be invoked if connection failed
```c#
app.UseDbConnectionInspector(new ConnectionOptions()
{
    new ConnectionChecker(new NpgsqlConnection(connString))
}, context => context.Response.StatusCode = (int)HttpStatusCode.BadRequest);
```
---
## Features list v1.0
- [x] Embedding inspector to request pipeline
- [x] Support PostgreSQL

## Features list v2.0
- [x] Specify action that would be invoke if connection failed 
- [x] Support for all connection providers who provide a class that implements `IDbConnection` interface
- [x] Logger for inspector
### Note
Architecture has been redesigned, so a new version is not compatible with old one

## Features list v3.0
- [x] Attribute based inspection  
Starting with this version connection inspection would be called only for controller methods marked by special attribute `RequireDbInspection`

## Features list v3.0.1
- [x] Added additional parameter for `IConnectionChecker` - `key` for identification each checker
- [x] Added ability provide `key` parameter in `RequireDbInspection` attribute constructor
- [x] Change inspection logic
