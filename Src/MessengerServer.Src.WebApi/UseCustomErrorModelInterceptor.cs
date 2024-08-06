using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MessengerServer.Src.Contracts.ErrorResponses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MessengerServer.Src.WebApi;

public class UseCustomErrorModelInterceptor : IValidatorInterceptor
{
    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
    {
        return commonContext;
    }

    public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
        ValidationResult result)
    {
        var failures = result.Errors
            .Select(error => new ValidationFailure(error.PropertyName, SerializeError(error)));

        return new ValidationResult(failures);
    }

    private static string SerializeError(ValidationFailure failure)
    {
        var error = new ErrorResponse
        {
            ErrorCode = failure.ErrorCode, 
            ErrorMessage = failure.ErrorMessage
        };

        return JsonSerializer.Serialize(error);
    }
}

