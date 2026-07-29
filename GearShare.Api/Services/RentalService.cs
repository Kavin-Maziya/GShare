using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Exceptions;
using GearShare.Api.Models;
using GearShare.Api.Repositories;

namespace GearShare.Api.Services;

// Scoped: owns a unit of work per request — coordinates repo calls that must
// share the same DbContext transaction boundary.
public class RentalService(
    IGearRepository gearRepository,
    IRentalRequestRepository rentalRequestRepository,
    GearShareDbContext context) : IRentalService
{
    public async Task<RentalRequestResponseDto> CreateAsync(
        Guid gearItemId, CreateRentalRequestDto dto)
    {
        var gear = await gearRepository.GetByIdAsync(gearItemId)
            ?? throw new GearNotFoundException(gearItemId);

        // Availability check — UnderMaintenance or Retired gear cannot be rented
        if (gear.Status != GearStatus.Available)
            throw new GearNotAvailableException(gearItemId, gear.Status);

        // Pre-flight overlap check — the EXCLUDE constraint is the hard guarantee
        // at the DB level, but we check here first to return a clean 422 to the
        // client instead of surfacing a raw Postgres exception.
        var hasOverlap = await rentalRequestRepository.HasOverlapAsync(
            gearItemId, dto.StartDate, dto.EndDate);

        if (hasOverlap)
            throw new GearNotAvailableException(gearItemId, gear.Status);

        var request = new RentalRequest
        {
            Id          = Guid.NewGuid(),
            GearItemId  = gearItemId,
            RenterName  = dto.RenterName,
            RenterEmail = dto.RenterEmail,
            RenterPhone = dto.RenterPhone,
            StartDate   = dto.StartDate,
            EndDate     = dto.EndDate,
            Status      = RentalStatus.Pending,
            Notes       = dto.Notes,
            RequestedAt = DateTime.UtcNow
        };

        await rentalRequestRepository.AddAsync(request);
        await rentalRequestRepository.SaveChangesAsync();

        return MapToDto(request);
    }

    public async Task<RentalRequestResponseDto> UpdateStatusAsync(
        Guid requestId, RentalStatus newStatus, Guid callerId, string callerRole)
    {
        var request = await rentalRequestRepository.GetByIdAsync(requestId)
            ?? throw new GearNotFoundException(requestId);

        var gear = request.GearItem;
        var isAdmin = callerRole == "Admin";
        var isOwner = gear.OwnerId == callerId;

        if (!isOwner && !isAdmin)
            throw new UnauthorizedActionException(
                "Only the gear owner or an Admin may change a rental request status.");

        request.Status = newStatus;
        await rentalRequestRepository.SaveChangesAsync();

        return MapToDto(request);
    }

    public async Task<GearItemResponseDto> SetMaintenanceAsync(
        Guid gearItemId, Guid callerId, string callerRole)
    {
        var gear = await gearRepository.GetByIdAsync(gearItemId)
            ?? throw new GearNotFoundException(gearItemId);

        var isAdmin = callerRole == "Admin";
        var isOwner = gear.OwnerId == callerId;

        if (!isOwner && !isAdmin)
            throw new UnauthorizedActionException(
                "Only the gear owner or an Admin may set gear to UnderMaintenance.");

        gear.Status = GearStatus.UnderMaintenance;
        await context.SaveChangesAsync();

        return MapGearToDto(gear);
    }

    public async Task<GearItemResponseDto> RetireAsync(Guid gearItemId, string callerRole)
    {
        var gear = await gearRepository.GetByIdAsync(gearItemId)
            ?? throw new GearNotFoundException(gearItemId);

        if (callerRole != "Admin")
            throw new UnauthorizedActionException(
                "Only an Admin may retire a gear listing.");

        gear.Status = GearStatus.Retired;
        await context.SaveChangesAsync();

        return MapGearToDto(gear);
    }

    private static RentalRequestResponseDto MapToDto(RentalRequest r) => new()
    {
        Id          = r.Id,
        GearItemId  = r.GearItemId,
        RenterName  = r.RenterName,
        RenterEmail = r.RenterEmail,
        RenterPhone = r.RenterPhone,
        StartDate   = r.StartDate,
        EndDate     = r.EndDate,
        Status      = r.Status.ToString(),
        Notes       = r.Notes,
        RequestedAt = r.RequestedAt
    };

    private static GearItemResponseDto MapGearToDto(GearItem g) => new()
    {
        Id             = g.Id,
        OwnerId        = g.OwnerId,
        Title          = g.Title,
        Description    = g.Description,
        Category       = g.Category.ToString(),
        DailyRateCents = g.DailyRateCents,
        Status         = g.Status.ToString(),
        CreatedAt      = g.CreatedAt
    };
}