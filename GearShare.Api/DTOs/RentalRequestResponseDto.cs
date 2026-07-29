namespace GearShare.Api.DTOs;

public class RentalRequestResponseDto
{
    public Guid Id { get; set; }
    public Guid GearItemId { get; set; }
    public string RenterName { get; set; } = string.Empty;
    public string RenterEmail { get; set; } = string.Empty;
    public string RenterPhone { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime RequestedAt { get; set; }
}