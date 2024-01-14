using Domain.Constants;
using Domain.Exceptions;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
namespace Backend.CodeAuth.Middleware
{
  

        /// <summary>
        /// Middleware for the exception handling
        /// </summary>
        public static class ProblemDetailsExtensions
        {
            public static IServiceCollection AddAppProblemDetails(this IServiceCollection services, IWebHostEnvironment environment)
            {
                // This will add the required services to the DI container
                services.AddProblemDetails(options =>
                {
                    options.IncludeExceptionDetails = (context, ex) =>
                    {
                        if (ex is ValidationException)
                        {
                            return false;
                        }

                        ILogger<ProblemDetails> logger = context.RequestServices.GetRequiredService<ILogger<ProblemDetails>>();

                        logger.LogError(ex, ex.Message);

                        return environment.IsDevelopment();
                    };

                    options.Map<InvalidOperationException>(ex => CreateProblemDetails(StatusCodes.Status400BadRequest, ErrorMessages.UnexpectedErrorMessageTitle, ErrorMessages.UnexpectedErrorMessage));
                    options.Map<AppNotFoundException>(ex => CreateProblemDetails(StatusCodes.Status404NotFound, ErrorMessages.ResourceNotFound, ErrorMessages.ResourceNotFound));
                    options.Map<AppBadDataException>(ex => CreateProblemDetails(StatusCodes.Status400BadRequest, ex.Message, ex.Message));
                    options.Map<DbUpdateException>(ex => CreateProblemDetails(StatusCodes.Status400BadRequest, ErrorMessages.UnexpectedErrorMessageTitle, ErrorMessages.UnexpectedErrorMessage));
                    options.Map<NotImplementedException>(ex => CreateProblemDetails(StatusCodes.Status400BadRequest, ErrorMessages.UnexpectedErrorMessageTitle, ErrorMessages.UnexpectedErrorMessage));
                    options.Map<UnauthorizedAccessException>(ex => CreateProblemDetails(StatusCodes.Status403Forbidden, ErrorMessages.NoAccessErrorMessageTitle, ErrorMessages.NoAccessErrorMessage));
                    options.Map<ValidationException>(ex =>
                    {
                        return new ValidationProblemDetails(ex.Errors
                            .GroupBy(validationFailure => validationFailure.PropertyName)
                            .ToDictionary(group => group.Key, group => group.Select(validationFailure => validationFailure.ErrorMessage).ToArray()))
                        {
                            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                            Status = StatusCodes.Status400BadRequest
                        };
                    });
                    options.Map<Exception>(ex => CreateProblemDetails(StatusCodes.Status500InternalServerError, ErrorMessages.UnexpectedErrorMessageTitle, ErrorMessages.UnexpectedErrorMessage));
                });

                return services;
            }

            public static IApplicationBuilder UseAppProblemDetails(this IApplicationBuilder app)
            {
                // Add middleware to the request processing pipeline
                app.UseProblemDetails();

                return app;
            }

            /// <summary>
            /// Method to create a ProblemDetails to return it as error message which gets displayed in frontend
            /// </summary>
            /// <param name="statusCode">Status code of the error</param>
            /// <param name="title">Title of the error</param>
            /// <param name="message">Message which gets displayed to the user</param>
            /// <param name="instance">Uri which refrence the specifc error</param>
            /// <returns>New instance of ProblemDetails</returns>
            public static ProblemDetails CreateProblemDetails(int statusCode, string title, string? message = null, string? instance = null)
            {
                return new ProblemDetails
                {
                    Type = $"https://httpstatuses.com/{statusCode}",
                    Title = title,
                    Detail = message,
                    Status = statusCode,
                    Instance = instance
                };
            }
        }

}
