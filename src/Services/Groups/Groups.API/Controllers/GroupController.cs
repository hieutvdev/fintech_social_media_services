using Microsoft.AspNetCore.Mvc;

namespace Groups.API.Controllers;


[ApiController]
[Route("api/v1/groups")]
public class GroupController : Controller
{
    [HttpGet("")]
    public IActionResult GetGroups()
    {
        return Ok("ok");
    }
}