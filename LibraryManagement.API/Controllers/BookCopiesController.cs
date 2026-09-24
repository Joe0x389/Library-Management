using LibraryManagement.API.Common;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Enums;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/copies")]
public class BookCopiesController : ControllerBase
{
    private readonly IBookCopyService _service;

    public BookCopiesController(IBookCopyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CopyStatus? status)
    {
        var copies = await _service.GetAllAsync(status);

        return Ok(ApiResponse<object>.SuccessResponse(copies));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var copy = await _service.GetByIdAsync(id);

        if (copy == null)
            return NotFound(
                ApiResponse<object>.FailResponse("Book copy not found."));

        return Ok(ApiResponse<object>.SuccessResponse(copy));
    }

    [HttpGet("book/{bookId}")]
    public async Task<IActionResult> GetByBookId(int bookId)
    {
        var copies = await _service.GetByBookIdAsync(bookId);

        return Ok(ApiResponse<object>.SuccessResponse(copies));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookCopyDto dto)
    {
        try
        {
            var copy = await _service.CreateAsync(dto);

            if (copy == null)
                return NotFound(
                    ApiResponse<object>.FailResponse("Book not found."));

            return CreatedAtAction(
                nameof(GetById),
                new { id = copy.Id },
                ApiResponse<object>.SuccessResponse(
                    copy,
                    "Book copy created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<object>.FailResponse(ex.Message));
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateCopyStatusDto dto)
    {
        var updated = await _service.UpdateStatusAsync(id, dto);

        if (!updated)
            return NotFound(
                ApiResponse<object>.FailResponse("Book copy not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Book copy status updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(
                    ApiResponse<object>.FailResponse("Book copy not found."));

            return Ok(
                ApiResponse<object>.SuccessResponse(
                    null!,
                    "Book copy deleted successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<object>.FailResponse(ex.Message));
        }
    }
}