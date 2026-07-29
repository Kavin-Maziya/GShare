using GearShare.Api.DTOs;
using GearShare.Api.Models;

namespace GearShare.Api.Services;

public interface IRentalService
{
    Task<RentalRequestResponseDto> CreateAsync(Guid gearItemId, CreateRentalRequestDto dto);
    Task<RentalRequestResponseDto> UpdateStatusAsync(Guid requestId, RentalStatus newStatus, Guid callerId, string callerRole);
    Task<GearItemResponseDto>      SetMaintenanceAsync(Guid gearItemId, Guid callerId, string callerRole);
    Task<GearItemResponseDto>      RetireAsync(Guid gearItemId, string callerRole);
}