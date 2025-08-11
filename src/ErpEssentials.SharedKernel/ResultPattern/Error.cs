// Represents the abstract base class for all errors, defining a common contract and the 'None' value.
namespace ErpEssentials.SharedKernel.ResultPattern;

public abstract class Error(string code, string message, ErrorType type)
{
    public string Code { get; } = code;
    public string Message { get; } = message;
    public ErrorType Type { get; } = type;

    public static readonly Error None = new EmptyError();

    private sealed class EmptyError() : Error(string.Empty, string.Empty, ErrorType.Failure) { }
}