using GearShare.Api.Data;
using GearShare.Api.DTOs;
using GearShare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GearShare.Api.Repositories;

// Scoped: one instance per HTTP request — matches DbContext lifetime so we never
// share a context across requests or hold it open longer than needed.
public class GearRepository(GearShareDbContext context) : IGearRepository
{
    private readonly GearShareDbContext _context = context;
    private const int MaxPageSize = 50;

    public async Task<PagedResponseDto<GearItemResponseDto>> GetAllAsync(
        string? category, string? status, int page, int pageSize)
    {
        pageSize = Math.Min(pageSize, MaxPageSize);
        page     = Math.Max(page, 1);

        var query = _context.GearItems.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(category) &&
            Enum.TryParse<Category>(category, ignoreCase: true, out var categoryEnum))
            query = query.Where(g => g.Category == categoryEnum);

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<GearStatus>(status, ignoreCase: true, out var statusEnum))
            query = query.Where(g => g.Status == statusEnum);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(g => g.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GearItemResponseDto
            {
                Id             = g.Id,
                OwnerId        = g.OwnerId,
                Title          = g.Title,
                Description    = g.Description,
                Category       = g.Category.ToString(),
                DailyRateCents = g.DailyRateCents,
                Status         = g.Status.ToString(),
                CreatedAt      = g.CreatedAt,
                ActiveRequestCount = g.RentalRequests
                    .Count(r => r.Status == RentalStatus.Pending ||
                                r.Status == RentalStatus.Approved)
            })
            .ToListAsync();

        return new PagedResponseDto<GearItemResponseDto>
        {
            Items      = items,
            TotalCount = totalCount,
            Page       = page,
            PageSize   = pageSize
        };
    }

    public async Task<IEnumerable<GearItemResponseDto>> SearchAsync(string q)
    {
        return await _context.GearItems
            .AsNoTracking()
            .Where(g => g.SearchVector.Matches(EF.Functions.ToTsQuery("english", q)))
            .Select(g => new GearItemResponseDto
            {
                Id             = g.Id,
                OwnerId        = g.OwnerId,
                Title          = g.Title,
                Description    = g.Description,
                Category       = g.Category.ToString(),
                DailyRateCents = g.DailyRateCents,
                Status         = g.Status.ToString(),
                CreatedAt      = g.CreatedAt,
                ActiveRequestCount = g.RentalRequests
                    .Count(r => r.Status == RentalStatus.Pending ||
                                r.Status == RentalStatus.Approved)
            })
            .ToListAsync();
    }

    public async Task<GearItem?> GetByIdAsync(Guid id)
        => await _context.GearItems.FindAsync(id);

    public async Task<IEnumerable<RentalRequestResponseDto>> GetRequestsAsync(Guid gearItemId)
        => await _context.RentalRequests
            .AsNoTracking()
            .Where(r => r.GearItemId == gearItemId)
            .Select(r => new RentalRequestResponseDto
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
            })
            .ToListAsync();
}