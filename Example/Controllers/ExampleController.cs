using DbConnectionInspector.Core;
using Example.Database;
using Example.Models;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

public class ExampleController : Controller
{
    private readonly AppDbContext _dbContext;

    public ExampleController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [RequireDbInspection]
    [HttpGet("/get")]
    public IActionResult Get()
    {
        return Ok(_dbContext.Models.ToList());
    }

    [RequireDbInspection]
    [HttpPost("/post")]
    public IActionResult Post([FromBody] ExampleModel model)
    {
        _dbContext.Models.Add(model);
        _dbContext.SaveChanges();
        return Ok();
    }
}