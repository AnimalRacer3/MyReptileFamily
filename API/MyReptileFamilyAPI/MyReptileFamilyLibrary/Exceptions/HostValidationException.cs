namespace MyReptileFamilyLibrary.Exceptions;

internal class HostValidationException(string _p_Message)
    : ApplicationException($"Exception occurred when validating the created Host: {_p_Message}");