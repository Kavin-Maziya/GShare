namespace GearShare.Api.Models;

public class RentalRequest
{
    public Guid Id { get; set; }
    public Guid GearItemId { get; set; }
    public GearItem GearItem { get; set; } = null!;
    public string RenterName { get; set; } = string.Empty;
    public string RenterEmail { get; set; } = string.Empty;
    public string RenterPhone { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public RentalStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime RequestedAt { get; set; }
}