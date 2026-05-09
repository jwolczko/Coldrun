using Coldrun.Modules.TruckManagement.Application.Trucks.Exceptions;
using Coldrun.Modules.TruckManagement.Domain.Trucks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coldrun.Modules.TruckManagement.Api.Errors;

public sealed class TruckProblemDetailsMapper
{
    public ProblemDetails Map(Exception exception, HttpContext httpContext)
    {
        return exception switch
        {
            TruckNotFoundException truckNotFound =>
                new ProblemDetails
                {
                    Type = "https://api.example.com/problems/truck-not-found",
                    Title = "Truck not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = truckNotFound.Message,
                    Instance = httpContext.Request.Path
                },

            TruckCodeAlreadyExistsException truckCodeAlreadyExists =>
                new ProblemDetails
                {
                    Type = "https://api.example.com/problems/truck-code-already-exists",
                    Title = "Truck code already exists",
                    Status = StatusCodes.Status409Conflict,
                    Detail = truckCodeAlreadyExists.Message,
                    Instance = httpContext.Request.Path
                },

            InvalidTruckStatusTransitionException invalidTransition =>
                new ProblemDetails
                {
                    Type = "https://api.example.com/problems/invalid-truck-status-transition",
                    Title = "Invalid truck status transition",
                    Status = StatusCodes.Status409Conflict,
                    Detail = invalidTransition.Message,
                    Instance = httpContext.Request.Path
                },

            _ =>
                new ProblemDetails
                {
                    Type = "https://api.example.com/problems/internal-server-error",
                    Title = "Internal server error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An unexpected error occurred.",
                    Instance = httpContext.Request.Path
                }
        };
    }
}
