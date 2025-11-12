// Defines the high-level categories for errors to enable programmatic decisions.
namespace ErpEssentials.Stock.SharedKernel.ResultPattern;
public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unexpected
}