using LibraryManagement.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

// TODO: module owner - inject your service layer here, replace this placeholder action.
[ApiController]
[Route("api/publishers")]
public class PublishersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<string>.SuccessResponse("PublishersController placeholder - implement me"));
    }
}
