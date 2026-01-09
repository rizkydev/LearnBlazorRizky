using Microsoft.AspNetCore.Http.HttpResults;
using RizkyApps.Shared.DTOs;
using RizkyApps.API.Services;

namespace RizkyApps.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void RegisterUserEndpoints(this WebApplication app)
        {
            var UserGroup = app.MapGroup("/api/user")
                .WithTags("User");

            // GET /api/user
            UserGroup.MapGet("/", async (IUserService UserService) =>
            {
                var Users = await UserService.GetAllUsersAsync();
                return Results.Ok(Users);
            })
            .WithSummary("Get all Users")
            .Produces<List<DtoUserResponse>>(StatusCodes.Status200OK)
            .WithDescription("Returns a All of User objects."); // ✅ Use this;

            // GET /api/user/{id}
            UserGroup.MapGet("/{id}", async (int id, IUserService UserService) =>
            {
                var User = await UserService.GetUserByIdAsync(id);
                return User is not null ? Results.Ok(User) : Results.NotFound();
            })
            .WithSummary("Get User by ID")
            .Produces<DtoUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Returns 1 User objects."); // ✅ Use this;

            // POST /api/user
            UserGroup.MapPost("/", async (DtoUserCreate UserDto, IUserService UserService) =>
            {
                var User = await UserService.CreateUserAsync(UserDto);
                return Results.Created($"/api/Users/{User.Id}", User);
            })
            .WithSummary("Create a new User")
            .Produces<DtoUserResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

            // PUT /api/user
            UserGroup.MapPut("/", async (DtoUserUpdate UserDto, IUserService UserService) =>
            {
                var User = await UserService.UpdateUserAsync(UserDto);
                return User is not null ? Results.Ok(User) : Results.NotFound();
            })
            .WithSummary("Update an existing User")
            .Produces<DtoUserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // DELETE /api/user/{id}
            UserGroup.MapDelete("/{id}", async (int id, IUserService UserService) =>
            {
                var deleted = await UserService.DeleteUserAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithSummary("Delete a User")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}