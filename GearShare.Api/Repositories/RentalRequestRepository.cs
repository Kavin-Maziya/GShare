using GearShare.Api.Data;
using GearShare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GearShare.Api.Repositories;

// Scoped: matches DbContext lifetime — SaveChangesAsync must act on the same
// context instance that tracked the Add, so singleton would be wrong here.
public class RentalRequestRepository(GearShareDbContext context) : IRentalRequestRepository
{
    private readonly GearShareDbContext _context = context;

    public async Task<RentalRequest?> GetByIdAsync(Guid id)
        => await _context.RentalRequests
            .Include(r => r.GearItem)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(RentalRequest request)
        => await _context.RentalRequests.AddAsync(request);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    // Pre-flight overlap check — catches the common case before hitting the DB
    // constraint. The EXCLUDE constraint in Part E is the hard guarantee;
    // this check avoids surfacing a raw DB exception to the client for the
    // normal (non-concurrent) path.
    public async Task<bool> HasOverlapAsync(
        Guid gearItemId, DateOnly start, DateOnly end, Guid? excludeId = null)
        => await _context.RentalRequests
            .AnyAsync(r =>
                r.GearItemId == gearItemId &&
                r.Status     == RentalStatus.Approved &&
                r.Id         != (excludeId ?? Guid.Empty) &&
                r.StartDate  <= end &&
                r.EndDate    >= start);
}