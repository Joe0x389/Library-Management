using LibraryManagement.API.Common;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _service;

    public PublishersController(IPublisherService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var publishers = await _service.GetAllAsync();

        return Ok(ApiResponse<IEnumerable<object>>
            .SuccessResponse(publishers));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var publisher = await _service.GetByIdAsync(id);

        if (publisher == null)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Publisher not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(publisher));
    }

    [HttpPost]
    public async Task<IActionResult> Create(PublisherDto dto)
    {
        var publisher = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = publisher.Id },
            ApiResponse<object>.SuccessResponse(
                publisher,
                "Publisher created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        PublisherDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Publisher not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Publisher updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Publisher not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Publisher deleted successfully."));
    }
}