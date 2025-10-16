// Represents an unexpected error, typically used for unhandled exceptions or system-level failures.
namespace ErpEssentials.SharedKernel.ResultPattern;

public class UnexpectedError(string code, string message) : Error(code, message, ErrorType.Unexpected);