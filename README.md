# Connection Inspector
Library for inspect database connections and prevent requests to its until establish conneciton (for Asp.Net apps)


## Library Purpose
Library porpose is to try to help achieve max app reliability by handling database connection state. That is if your database server was down app would continue it's work

## Features list v1.0
- [x] Embedding inspector to request pipeline
- [x] Support PostgreSQL

## Features list v2.0
- [x] Specify action that would be invoke if connection failed 
- [ ] Support for MySql
- [x] Logger for inspector

## How to use library
Here the example of how you can use this library in your Asp.Net project (from Exmaple project)

### Common use
```c#
app.UseDbConnectionInspector(new ConnectionOptions()
{
    Connections = new []
    {
        new PostgresConnection()
        {
            ConnectionString = connString
        }
    }
});
```
### Without connections
In this case checking would be always succeed
```c#
app.UseDbConnectionInspector(new ConnectionOptions());
```

### Specify action
You may specify what action would be invoked if connection failed
```c#
app.UseDbConnectionInspector(new ConnectionOptions()
{
    Connections = new []
    {
        new PostgresConnection()
        {
            ConnectionString = connString
        }
    }
}, context => context.Response.StatusCode = (int)HttpStatusCode.BadRequest);
```
