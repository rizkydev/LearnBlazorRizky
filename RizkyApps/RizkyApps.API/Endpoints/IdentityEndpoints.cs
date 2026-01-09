using Microsoft.AspNetCore.Http.HttpResults;
using RizkyApps.API.Services;
using RizkyApps.Shared.DTOs;
using System.Security.Claims;

namespace RizkyApps.API.Endpoints
{
    public static class IdentityEndpoints
    {
        public static void RegisterIdentityEndpoints(this WebApplication app)
        {
            var IdentityGroup = app.MapGroup("/api/identity")
                .WithTags("Identity");

            IdentityGroup.MapPost("/login", async (LoginRequest request, IdentityService identityService, IUserService userService) =>
            {
                var userDat = await userService.GetUserAsync(request);
                if (userDat is not null)
                {
                    var token = identityService.GenerateToken(request.Username, ["admin"]);
                    return Results.Ok(new { access_token = token });
                }
                return Results.Unauthorized();
            });

            // Protected endpoint
            IdentityGroup.MapGet("/secure", (ClaimsPrincipal user) =>
            {
                return Results.Ok(new
                {
                    message = "Secure data",
                    username = user.Identity?.Name,
                    isAuthenticated = user.Identity?.IsAuthenticated
                });
            }).RequireAuthorization();

            IdentityGroup.MapGet("/claims", (ClaimsPrincipal user) =>
            {
                var claims = user.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value,
                    ValueType = c.ValueType
                }).ToList();

                return Results.Ok(claims);
            }).RequireAuthorization();

            IdentityGroup.MapGet("/claims2", (ClaimsPrincipal user) =>
            {
                Console.WriteLine($"✅ Access granted to: {user.Identity?.Name}");

                return Results.Ok(new
                {
                    Message = "This is protected data!",
                    User = user.Identity?.Name,
                    IsAuthenticated = user.Identity?.IsAuthenticated,
                    Claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList()
                });
            }).RequireAuthorization();


            IdentityGroup.MapGet("/debug-config", (JwtConfiguration config) =>
            {
                return Results.Ok(new
                {
                    SecretLength = config.Secret.Length,
                    SecretFirst10 = config.Secret.Length > 10
                        ? config.Secret.Substring(0, 10) + "..."
                        : config.Secret,
                    Issuer = config.Issuer,
                    Audience = config.Audience,
                    ExpireDays = config.ExpireDays
                });
            }).AllowAnonymous();
        }
    }
}