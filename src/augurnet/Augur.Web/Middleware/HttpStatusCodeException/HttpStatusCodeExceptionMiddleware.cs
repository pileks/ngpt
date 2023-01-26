using System;
using System.Threading.Tasks;
using Augur.Security.Exceptions;
using Augur.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Augur.Web.Middleware.HttpStatusCodeException
{
    public class AugurHttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AugurHttpStatusCodeExceptionMiddleware> _logger;

        public AugurHttpStatusCodeExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
            this._logger = loggerFactory?.CreateLogger<AugurHttpStatusCodeExceptionMiddleware>() ??
                           throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                try
                {
                    await this._next(context);
                }
                catch (UnauthorizedException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogInformation(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync("You are unauthorized.");

                    return;
                }
                catch (ForbiddenException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    this._logger.LogWarning("The action is forbidden.");

                    context.Response.Clear();
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(ex.Message);

                    return;
                }
                catch (VerboseException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    var dto = ex.GetDto();
                    var dtoJson = JsonConvert.SerializeObject(new
                    {
                        IsVerboseException = true,
                        Type = ex.GetType().Name,
                        Data = dto
                    });

                    this._logger.LogWarning($"A verbose exception occurred on the server: {dtoJson}");

                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(dtoJson);

                    return;
                }
            }
            else
            {
                try
                {
                    await this._next(context);
                }
                catch (UnauthorizedException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogInformation(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync("You are unauthorized.");

                    return;
                }
                catch (ForbiddenException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    this._logger.LogWarning("The action is forbidden.");

                    context.Response.Clear();
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(ex.Message);

                    return;
                }
                catch (Web.HttpStatusCodeException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    this._logger.LogWarning($"Status code exception ({ex.StatusCode}): {ex.Message}");

                    context.Response.Clear();
                    context.Response.StatusCode = ex.StatusCode;
                    context.Response.ContentType = ex.ContentType;

                    await context.Response.WriteAsync(ex.Message);

                    return;
                }
                catch (InvalidOperationException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    this._logger.LogWarning(ex, "Invalid operation.");

                    context.Response.Clear();
                    context.Response.StatusCode = 500;

                    await context.Response.WriteAsync("A internal server error occurred.");

                    return;
                }
                catch (VerboseException ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    var dto = ex.GetDto();
                    var dtoJson = JsonConvert.SerializeObject(new
                    {
                        IsVerboseException = true,
                        Type = ex.GetType().Name,
                        Data = dto
                    });

                    this._logger.LogWarning($"A verbose exception occurred on the server: {dtoJson}");

                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(dtoJson);

                    return;
                }
                catch (Exception ex)
                {
                    if (context.Response.HasStarted)
                    {
                        this._logger.LogWarning(
                            "The response has already started, the http status code middleware will not be executed.");
                        throw;
                    }

                    this._logger.LogError(ex, "An internal server error occurred.");

                    context.Response.Clear();
                    context.Response.StatusCode = 500;

                    await context.Response.WriteAsync("An internal server error occurred.");

                    return;
                }
            }
        }
    }
}