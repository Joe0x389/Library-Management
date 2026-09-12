using LibraryManagement.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

// TODO: module owner - inject your service layer here, replace this placeholder action.
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<string>.SuccessResponse("ReportsController placeholder - implement me"));
    }
}
