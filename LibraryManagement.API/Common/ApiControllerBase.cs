using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// +�+�+�+�+�+� +�+�+� +�+�+� Service +�+� HTTP status +�+�+�+�+� (404 / 409 / 400)
    protected IActionResult Fail<T>(ServiceResult<T> r) => r.ErrorType switch
    {
        ErrorType.NotFound => NotFound(new { message = r.Error }),
        ErrorType.Conflict => Conflict(new { message = r.Error }),
        _ => BadRequest(new { message = r.Error })
    };
}
