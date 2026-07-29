using System.Security.Claims;
using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using GearShare.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GearShare.Api.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize] // All endpoints here require authentication — unauthenticated → 401
public class RequestsController : ControllerBase
{
    // Update the rental request status.
    // Only the gear owner or an Admin may approve, reject, or mark as returned.
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(RentalRequestResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchStatus(Guid id, [FromBody] UpdateRentalStatusDto dto)
    {
        await Task.CompletedTask;

        var request = InMemoryStore.RentalRequests.FirstOrDefault(r => r.Id == id);
        if (request is null) throw new GearNotFoundException(id);

        var gear = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == request.GearItemId);
        if (gear is null) throw new GearNotFoundException(request.GearItemId);

        var callerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue("sub")!);
        var callerRole = User.FindFirstValue(ClaimTypes.Role);
        var isAdmin = callerRole == "Admin";
        var isOwner = gear.OwnerId == callerId;

        // Non-owner non-admin authenticated user → 403, not 401.
        if (!isOwner && !isAdmin)
            throw new UnauthorizedActionException(
                "Only the gear owner or an Admin may change a rental request status.");

        request.Status = dto.Status;

        return Ok(MapToDto(request));
    }

    
    // Set gear status to UnderMaintenance. Owner or Admin only.
    [HttpPatch("{gearItemId:guid}/maintenance")]
    [ProducesResponseType(typeof(GearItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetUnderMaintenance(Guid gearItemId)
    {
        await Task.CompletedTask;

        var gear = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == gearItemId);
        if (gear is null) throw new GearNotFoundException(gearItemId);

        var callerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue("sub")!);
        var callerRole = User.FindFirstValue(ClaimTypes.Role);
        var isAdmin = callerRole == "Admin";
        var isOwner = gear.OwnerId == callerId;

        if (!isOwner && !isAdmin)
            throw new UnauthorizedActionException(
                "Only the gear owner or an Admin may set gear to UnderMaintenance.");

        gear.Status = GearStatus.UnderMaintenance;
        return Ok(MapGearToDto(gear));
    }

    
    /// Retire a gear listing. Admin only — owners cannot retire their own gear.
    [HttpPatch("{gearItemId:guid}/retire")]
    [ProducesResponseType(typeof(GearItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RetireGear(Guid gearItemId)
    {
        await Task.CompletedTask;

        var gear = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == gearItemId);
        if (gear is null) throw new GearNotFoundException(gearItemId);

        var callerRole = User.FindFirstValue(ClaimTypes.Role);

        // Retire is Admin-only. An owner attempting this on their own
        // gear still gets 403
        if (callerRole != "Admin")
            throw new UnauthorizedActionException(
                "Only an Admin may retire a gear listing.");

        gear.Status = GearStatus.Retired;
        return Ok(MapGearToDto(gear));
    }

    private static RentalRequestResponseDto MapToDto(RentalRequest r) => new()
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

    private static GearItemResponseDto MapGearToDto(GearItem g) => new()
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
}