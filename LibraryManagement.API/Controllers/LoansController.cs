using LibraryManagement.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

// TODO: module owner - inject your service layer here, replace this placeholder action.
[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<string>.SuccessResponse("LoansController placeholder - implement me"));
    }
}
