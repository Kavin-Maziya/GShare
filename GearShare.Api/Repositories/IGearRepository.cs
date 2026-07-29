using GearShare.Api.DTOs;
using GearShare.Api.Models;

namespace GearShare.Api.Repositories;

public interface IGearRepository
{
    Task<PagedResponseDto<GearItemResponseDto>> GetAllAsync(
        string? category, string? status, int page, int pageSize);

    Task<IEnumerable<GearItemResponseDto>> SearchAsync(string q);

    Task<GearItem?> GetByIdAsync(Guid id);

    Task<IEnumerable<RentalRequestResponseDto>> GetRequestsAsync(Guid gearItemId);
}