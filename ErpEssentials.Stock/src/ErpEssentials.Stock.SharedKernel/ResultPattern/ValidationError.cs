// A sealed, concrete error for representing one or more validation failures, typically from a pipeline.
namespace ErpEssentials.Stock.SharedKernel.ResultPattern;

public sealed class ValidationError(IReadOnlyDictionary<string, string[]> errors)
    : Error("Error.Validation", "One or more validation errors occurred.", ErrorType.Validation)
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}