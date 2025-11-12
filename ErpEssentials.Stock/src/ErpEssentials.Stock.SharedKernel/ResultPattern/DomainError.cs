// A simple, concrete implementation of Error used for creating specific business rule error instances.
namespace ErpEssentials.Stock.SharedKernel.ResultPattern;

internal sealed class DomainError(string code, string message, ErrorType type)
    : Error(code, message, type)
{
}