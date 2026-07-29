namespace GearShare.Api.Exceptions;

public class GearNotFoundException(Guid gearItemId)
    : Exception($"No gear item exists with ID '{gearItemId}'.");