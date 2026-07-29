using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using GearShare.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearShare.Api.Controllers;

[ApiController]
[Route("api/gear")]
public class GearController : ControllerBase
{

    // Returns all gear listings.
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GearItemResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        await Task.CompletedTask;
        var items = InMemoryStore.GearItems.Select(MapToDto);
        return Ok(items);
    }

    // Returns a single gear listing by ID.

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GearItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        await Task.CompletedTask;
        var item = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == id);

        if (item is null)
            throw new GearNotFoundException(id);

        return Ok(MapToDto(item));
    }


    // Returns all rental requests for a gear listing.
    [Authorize]
    [HttpGet("{id:guid}/requests")]
    [ProducesResponseType(typeof(IEnumerable<RentalRequestResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequests(Guid id)
    {
        await Task.CompletedTask;
        var item = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == id);

        if (item is null)
            throw new GearNotFoundException(id);

        var requests = InMemoryStore.RentalRequests
            .Where(r => r.GearItemId == id)
            .Select(MapRequestToDto);

        return Ok(requests);
    }
    // ── Private mappers ──────────────────────────────────────────────────────

    private static GearItemResponseDto MapToDto(GearItem g) => new()
    {
        Id = g.Id,
        OwnerId = g.OwnerId,
        Title = g.Title,
        Description = g.Description,
        Category = g.Category.ToString(),
        DailyRateCents = g.DailyRateCents,
        Status = g.Status.ToString(),
        CreatedAt = g.CreatedAt
    };

    private static RentalRequestResponseDto MapRequestToDto(RentalRequest r) => new()
    {
        Id = r.Id,
        GearItemId = r.GearItemId,
        RenterName = r.RenterName,
        RenterEmail = r.RenterEmail,
        RenterPhone = r.RenterPhone,
        StartDate = r.StartDate,
        EndDate = r.EndDate,
        Status = r.Status.ToString(),
        Notes = r.Notes,
        RequestedAt = r.RequestedAt
    };
}