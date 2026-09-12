using LibraryManagement.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

// TODO: module owner - inject your service layer here, replace this placeholder action.
[ApiController]
[Route("api/fines")]
public class FinesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<string>.SuccessResponse("FinesController placeholder - implement me"));
    }
}
