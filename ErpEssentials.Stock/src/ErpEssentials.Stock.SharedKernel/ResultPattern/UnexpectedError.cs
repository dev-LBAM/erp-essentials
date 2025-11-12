// Represents an unexpected error, typically used for unhandled exceptions or system-level failures.
namespace ErpEssentials.Stock.SharedKernel.ResultPattern;

public class UnexpectedError(string code, string message) : Error(code, message, ErrorType.Unexpected);