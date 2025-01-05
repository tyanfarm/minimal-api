using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SimpleMinimalAPI.Data;
using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Modules
{
    public static class UserModule
    {
        public static WebApplication MapUserApi(this WebApplication app)
        {
            var userApi = app.MapGroup("/api/user");

            userApi.MapGet("list", async (DataContext context) =>
            {
                return await context.Users.ToListAsync();
            });

            userApi.MapGet("{id}", async (DataContext context, int id) =>
            {
                return await context.Users.FindAsync(id) is User user ?
                        Results.Ok(user) :
                        Results.NotFound("User not found !");
            });

            userApi.MapPost("", async (DataContext context, User user) =>
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Results.Ok();
            });

            userApi.MapPut("{id}", async (DataContext context, User updatedUser, int id) =>
            {
                var user = await context.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound("User not found !");
                }

                user = updatedUser;
                await context.SaveChangesAsync();

                return Results.Ok();
                
            });

            userApi.MapDelete("{id}", async (DataContext context, int id) =>
            {
                var user = await context.Users.FindAsync(id);
                if (user is null)
                {
                    return Results.NotFound("User not found !");
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Results.Ok();
            });

            return app;
        }
    }
}
