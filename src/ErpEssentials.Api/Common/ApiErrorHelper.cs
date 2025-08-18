using ErpEssentials.SharedKernel.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace ErpEssentials.Api.Common;

public static class ApiErrorHelper
{
    public static ActionResult HandleFailure(this ControllerBase controller, Error error)
    {
        if (error is ValidationError validationError)
        {
            return controller.BadRequest(validationError);
        }

        return error.Type switch
        {
            ErrorType.NotFound => controller.NotFound(error),
            ErrorType.Conflict => controller.Conflict(error),
            ErrorType.Validation => controller.BadRequest(error),
            _ => controller.StatusCode(StatusCodes.Status500InternalServerError, error)
        };
    }
}