
using Application.Exceptions;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Middlewares
{
    public class ErrorHandelingMiddleware : IMiddleware
    {
        private async Task HandleExceptionrResponseAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response = new { statusCode, message };
            await context.Response.WriteAsJsonAsync(response);
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UnAuthorizedException unAuthorized)
            {
              await  HandleExceptionrResponseAsync(context, unAuthorized.StatusCode, unAuthorized.Message);
            }
            catch (NotFoundResourceException notFound)
            {
                await HandleExceptionrResponseAsync(context, notFound.StatusCode, notFound.Message);
            }
            catch(NotFoundException notFoundException)
            {
                await HandleExceptionrResponseAsync(context, notFoundException.StatusCode, notFoundException.Message);
            }
            catch (BadRequestException badRequest)
            {
                await HandleExceptionrResponseAsync(context, badRequest.StatusCode, badRequest.Message);
            }
            catch (ForbiddenException forbidden)
            {
                await HandleExceptionrResponseAsync(context, forbidden.StatusCode, forbidden.Message);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"Something went wrong ... , {ex.Message}");
            }
        }
    }
}
