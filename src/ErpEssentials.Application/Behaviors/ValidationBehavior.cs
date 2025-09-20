using ErpEssentials.SharedKernel.ResultPattern;
using FluentValidation;
using MediatR;
using System.Reflection;
using FluentValidation.Results;
using System.Linq;

namespace ErpEssentials.Application.Behaviors;

// Using a primary constructor (C# 12) for concise dependency injection.
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    // The 'where' constraint ensures this behavior only runs for MediatR requests...
    where TRequest : IRequest<TResponse>
    // ...and that the expected response is our custom 'Result' type, so we can return failures.
    where TResponse : Result
{
    // This field holds all validators found by the Dependency Injection container for the specific 'TRequest' type being processed.
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    // This is the main method called by MediatR. It intercepts the request.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // STEP 1: Quick Check (Guard Clause)
        // If no validators are registered for this request, there's nothing to do.
        if (!_validators.Any())
        {
            // We pass the request directly to the next step in the pipeline (the Handler).
            return await next(cancellationToken);
        }

        // STEP 2: Validation Execution
        // Create a context for FluentValidation, which contains the request to be validated.
        ValidationContext<TRequest> context = new(request);

        // We execute all validators asynchronously and wait for all to complete.
        ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // We use LINQ to execute all found validators:
        //  - .Select executes 'Validate' on each validator.
        //  - .SelectMany flattens the lists of errors from each result into a single list.
        //  - .Where ensures we don't have any null failures.
        //  - The collection expression `[..]` (C# 12) concisely creates the final list.
        List<ValidationFailure> validationFailures = [.. validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)];

        // STEP 3: Failure Handling ("Fail Fast")
        // If our list of failures contains any items, validation has failed.
        if (validationFailures.Count != 0)
        {
            // Execution stops here. The Handler will NEVER be called.

            // We group the errors by property name (e.g., "Sku", "Price") and create a dictionary.
            // This builds the structured "tree of errors" that is very useful for the front-end.
            Dictionary<string, string[]> errorsDictionary = validationFailures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => string.IsNullOrEmpty(g.Key)
                            ? "error"
                            : char.ToLowerInvariant(g.Key[0]) + g.Key[1..],
                    g => g.Select(vf => vf.ErrorMessage ?? "Unknown error").ToArray()
                );



            // We create our specialized error object, 'ValidationError',
            // which carries this dictionary of errors.
            ValidationError validationError = new(errorsDictionary);

            // This part uses Reflection. It's a necessary trick because we don't know the exact
            // generic type of the failure result at compile time (e.g., Result<Product>, Result<Order>).

            // Instead of searching on the base 'Result' class, we search directly on the specific
            // response type 'TResponse' that MediatR expects. This is cleaner and more direct.
            MethodInfo? failureMethod = typeof(TResponse)
                .GetMethod(
                    // We are looking for the static method named "Failure"...
                    nameof(Result<TRequest>.Failure),
                    // ...that is public and static...
                    BindingFlags.Public | BindingFlags.Static,
                    // ...and that accepts a single parameter of type 'Error'.
                    // This precisely targets the 'public static Result<T> Failure(Error error)' method.
                    [typeof(Error)]);

            if (failureMethod is not null)
            {
                // ...and invoke it to create the correct 'Result<Product>.Failure' or 'Result<Order>.Failure'.
                // This ensures we are returning the response type that the Handler expects.
                // The pipeline is now short-circuited.
                return (TResponse)failureMethod.Invoke(null, [validationError])!;
            }
        }

        // STEP 4: Validation Success
        // If there were no validation errors, we call 'next()' to pass control
        // to the next step in the pipeline, which is our business logic Handler.
        return await next(cancellationToken);
    }
}