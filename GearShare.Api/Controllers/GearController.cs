using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using GearShare.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearShare.Api.Controllers;

[ApiController]
[Route("api/gear")]
public class GearController(IGearRepository gearRepository) : ControllerBase
{
    // Returns a paged list of gear listings with optional category and status filters.
    // Single projection query — only DTO columns fetched. AsNoTracking applied in repo.
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDto<GearItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? category = null,
        [FromQuery] string? status   = null,
        [FromQuery] int     page     = 1,
        [FromQuery] int     pageSize = 10)
        => Ok(await gearRepository.GetAllAsync(category, status, page, pageSize));

    // Full-text search across Title and Description using a PostgreSQL GIN index.
    // Uses the generated tsvector column — avoids a LIKE %term% sequential scan.
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<GearItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(Enumerable.Empty<GearItemResponseDto>());

        return Ok(await gearRepository.SearchAsync(q));
    }

    // Returns a single gear listing by ID.
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GearItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await gearRepository.GetByIdAsync(id)
            ?? throw new GearNotFoundException(id);

        return Ok(MapGearToDto(item));
    }

    // Returns all rental requests for a gear listing. Requires authentication.
    [Authorize]
    [HttpGet("{id:guid}/requests")]
    [ProducesResponseType(typeof(IEnumerable<RentalRequestResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequests(Guid id)
    {
        _ = await gearRepository.GetByIdAsync(id)
            ?? throw new GearNotFoundException(id);

        return Ok(await gearRepository.GetRequestsAsync(id));
    }

    private static GearItemResponseDto MapGearToDto(GearShare.Api.Models.GearItem g) => new()
    {
        Id                 = g.Id,
        OwnerId            = g.OwnerId,
        Title              = g.Title,
        Description        = g.Description,
        Category           = g.Category.ToString(),
        DailyRateCents     = g.DailyRateCents,
        Status             = g.Status.ToString(),
        CreatedAt          = g.CreatedAt,
        ActiveRequestCount = 0
    };
}