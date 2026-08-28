using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDirectory.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = exception.Message
        };


        if (exception.Message.Contains("Already there") || exception.Message.Contains("Max limit") || exception.Message.Contains("Sorry"))
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Request error (Business Rule)";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);


        if (exception is FluentValidation.ValidationException validationException)
        {
            problemDetails.Status = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest;
            problemDetails.Title = "Validation Error";
            problemDetails.Detail = string.Join(" | ", validationException.Errors.Select(e => e.ErrorMessage));
        }

        return true;


    }
}