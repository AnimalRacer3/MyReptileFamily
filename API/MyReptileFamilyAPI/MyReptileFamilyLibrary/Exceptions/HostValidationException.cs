namespace MyReptileFamilyLibrary.Exceptions;

internal class HostValidationException(string Message)
    : ApplicationException($"Exception occurred when validating the created Host: {Message}");