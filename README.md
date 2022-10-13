# Connection Inspector
Library for inspect database connections and prevent requests to its until establish conneciton (for Asp.Net apps)

## Features list v1.0
- [x] Embedding inspector to request pipeline
- [x] Support PostgreSQL

## How to use library
Here the example of how you can use this library in your Asp.Net project (from Exmaple project)
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
