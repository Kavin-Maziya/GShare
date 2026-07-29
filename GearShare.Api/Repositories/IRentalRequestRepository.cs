using GearShare.Api.Models;

namespace GearShare.Api.Repositories;

public interface IRentalRequestRepository
{
    Task<RentalRequest?> GetByIdAsync(Guid id);
    Task AddAsync(RentalRequest request);
    Task SaveChangesAsync();
    Task<bool> HasOverlapAsync(Guid gearItemId, DateOnly start, DateOnly end, Guid? excludeId = null);
}