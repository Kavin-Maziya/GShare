using GearShare.Api.Models;

namespace GearShare.Api.Exceptions;

public class GearNotAvailableException(Guid gearItemId, GearStatus status)
    : Exception($"Gear item '{gearItemId}' cannot accept rental requests because its status is '{status}'.");