using System.ComponentModel.DataAnnotations;
using GearShare.Api.Models;

namespace GearShare.Api.DTOs;

public class UpdateRentalStatusDto
{
    [Required]
    public RentalStatus Status { get; set; }
}