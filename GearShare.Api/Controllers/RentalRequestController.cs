using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using GearShare.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GearShare.Api.Controllers;

[ApiController]
[Route("api/gear")]
public class RentalRequestsController(IMemoryCache cache) : ControllerBase
{

    /// Submit a rental request for a gear item
    /// Supply an **Idempotency-Key** header
    /// The same key resubmitted within 5 minutes returns the cached response
    /// without creating a duplicate row.
    [HttpPost("{gearItemId:guid}/requests")]
    [ProducesResponseType(typeof(RentalRequestResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateRentalRequest(
        Guid gearItemId,
        [FromBody] CreateRentalRequestDto dto)
    {
        await Task.CompletedTask;

        // Idempotency-Key guard
        // Scoped to header + gearItemId + email + dates so the same key used
        // against a different request is never a false duplicate hit.
        var idempotencyHeader = Request.Headers["Idempotency-Key"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(idempotencyHeader))
        {
            var cacheKey = $"{idempotencyHeader}:{gearItemId}:{dto.RenterEmail}:{dto.StartDate}:{dto.EndDate}";
            if (cache.TryGetValue(cacheKey, out RentalRequestResponseDto? cached))
                return CreatedAtAction(nameof(CreateRentalRequest), new { gearItemId }, cached);

            // Store after success — cacheKey is captured in this scope and reused below
            HttpContext.Items["IdempotencyCacheKey"] = cacheKey;
        }

        // Checks if the Gear exists
        var gear = InMemoryStore.GearItems.FirstOrDefault(g => g.Id == gearItemId);
        if (gear is null)
            throw new GearNotFoundException(gearItemId);

        // Returns 422 for unavailable gear
        if (gear.Status is GearStatus.UnderMaintenance or GearStatus.Retired)
            throw new GearNotAvailableException(gearItemId, gear.Status);

        // Creates a new rental request
        var rentalRequest = new RentalRequest
        {
            Id = Guid.NewGuid(),
            GearItemId = gearItemId,
            RenterName = dto.RenterName,
            RenterEmail = dto.RenterEmail,
            RenterPhone = dto.RenterPhone,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = RentalStatus.Pending,
            Notes = dto.Notes,
            RequestedAt = DateTime.UtcNow
        };

        InMemoryStore.RentalRequests.Add(rentalRequest);
        var response = MapToDto(rentalRequest);

        // Cache result for 5-minute idempotency window
        if (HttpContext.Items["IdempotencyCacheKey"] is string key)
            cache.Set(key, response, TimeSpan.FromMinutes(5));

        return CreatedAtAction(nameof(CreateRentalRequest), new { gearItemId }, response);
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
}