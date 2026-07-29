namespace GearShare.Api.Exceptions;

public class UnauthorizedActionException(string message)
    : Exception(message);