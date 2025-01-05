using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SimpleMinimalAPI.Data;
using SimpleMinimalAPI.Modules;
using SimpleMinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Config Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new() { Title = "Test Minimal API", Version = "v1" });

    options.TagActionsBy(api =>
    {
        var tag = api.RelativePath?.Split('/')[1];

        return new[] { tag };
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = JwtService.TokenValidationParameters;
                });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    "v1"
));

app.UseAuthentication();

app.MapStudentApi();
app.MapUserApi();

app.Run();
