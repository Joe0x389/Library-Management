using LibraryManagement.API.Common;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();

        return Ok(ApiResponse<IEnumerable<object>>
            .SuccessResponse(categories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);

        if (category == null)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Category not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(category));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        var category = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            ApiResponse<object>.SuccessResponse(
                category,
                "Category created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CategoryDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Category not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Category updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Category not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                null!,
                "Category deleted successfully."));
    }

    [HttpGet("{id}/books")]
    public async Task<IActionResult> GetBooks(int id)
    {
        var category = await _service.GetWithBooksAsync(id);

        if (category == null)
            return NotFound(
                ApiResponse<object>.FailResponse(
                    "Category not found."));

        return Ok(
            ApiResponse<object>.SuccessResponse(
                category.BookCategories.Select(x => x.Book)));
    }
}