using LibraryManagement.API.Common;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _service;

    public AuthorsController(IAuthorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var authors = await _service.GetAllAsync();

        return Ok(ApiResponse<IEnumerable<object>>
            .SuccessResponse(authors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _service.GetByIdAsync(id);

        if (author == null)
            return NotFound(
                ApiResponse<object>.FailResponse("Author not found."));

        return Ok(ApiResponse<object>
            .SuccessResponse(author));
    }

    [HttpPost]
    public async Task<IActionResult> Create(AuthorDto dto)
    {
        var author = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = author.Id },
            ApiResponse<object>.SuccessResponse(
                author,
                "Author created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        AuthorDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(
                ApiResponse<object>.FailResponse("Author not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Author updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(
                ApiResponse<object>.FailResponse("Author not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Author deleted successfully."));
    }

    [HttpGet("{id}/books")]
    public async Task<IActionResult> GetBooks(int id)
    {
        var author = await _service.GetWithBooksAsync(id);

        if (author == null)
            return NotFound(
                ApiResponse<object>.FailResponse("Author not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                author.BookAuthors.Select(x => x.Book)));
    }
}