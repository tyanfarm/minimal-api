using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleMinimalAPI.Config;
using SimpleMinimalAPI.DTOs;
using SimpleMinimalAPI.Helper;
using SimpleMinimalAPI.Messaging.Producer;
using SimpleMinimalAPI.Models;
using SimpleMinimalAPI.Services;

namespace SimpleMinimalAPI.Modules
{
    public static class AuthModule
    {
        public static WebApplication MapAuthApi(this WebApplication app)
        {
            var authApi = app.MapGroup("/api/auth");

            authApi.MapPost("login", async (DataContext context, UserDTO userDto) =>
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userDto.UserName);

                if (user == null || !Utilities.VerifyPassword(userDto.Password, user.PasswordHash)) {
                    return Results.NotFound("Invalid username or password !");
                }

                var token = JwtService.GenerateToken(user);

                return Results.Ok(token);
            });

            authApi.MapPost("register", 
                async (DataContext context, IMapper mapper, EmailProducer producer, UserDTO userDto) =>
            {
                var user = mapper.Map<User>(userDto);

                context.Users.Add(user);

                await context.SaveChangesAsync();

                // Create message
                var message = new MessageRequest
                {
                    Title = "Welcome to our platform",
                    Message = $"Hello {user.UserName}, you have successfully registered to our platform !"
                };

                await producer.Publish(new {EmailMessage = message, EmailDestination = user.Email});

                return Results.Ok("User registered successfully !");
            });

            return app;
        }
    }
}
