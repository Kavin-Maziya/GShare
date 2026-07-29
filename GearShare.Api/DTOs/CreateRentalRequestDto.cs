using System.ComponentModel.DataAnnotations;

namespace GearShare.Api.DTOs;

public class CreateRentalRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "RenterName is required.")]
    public string RenterName { get; set; } = string.Empty;

    [Required(ErrorMessage = "RenterEmail is required.")]
    [EmailAddress(ErrorMessage = "RenterEmail must be a valid email address.")]
    public string RenterEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "RenterPhone is required.")]
    [MinLength(7, ErrorMessage = "RenterPhone must be at least 7 characters.")]
    public string RenterPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "StartDate is required.")]
    public DateOnly StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required.")]
    public DateOnly EndDate { get; set; }

    public string? Notes { get; set; }

    // Cross-field validation
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate < DateOnly.FromDateTime(DateTime.UtcNow))
            yield return new ValidationResult(
                "StartDate must not be in the past.",
                [nameof(StartDate)]);

        if (EndDate <= StartDate)
            yield return new ValidationResult(
                "EndDate must be strictly after StartDate.",
                [nameof(EndDate)]);

        var rangeDays = EndDate.DayNumber - StartDate.DayNumber;
        if (rangeDays > 14)
            yield return new ValidationResult(
                "Rental period must not exceed 14 days.",
                [nameof(EndDate)]);
    }
}