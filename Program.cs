using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using SimpleMinimalAPI.Config;
using SimpleMinimalAPI.Helper;
using SimpleMinimalAPI.Mapping;
using SimpleMinimalAPI.Messaging.Producer;
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

// Add AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});

// Add RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(config =>
{
    var factory = new ConnectionFactory
    {
        HostName = "localhost",
        UserName = "user",
        Password = "mypass",
        VirtualHost = "/",
    };

    return factory;
});

// Add AppSettings
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
AppSettings.Initialize(appSettings);

builder.Services.AddEndpointsApiExplorer();

// Add DbContext
builder.Services.AddDbContext<DataContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = JwtService.TokenValidationParameters;
                });

builder.Services.AddScoped<EmailProducer>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    "v1"
));

app.UseAuthentication();
app.UseAuthorization();

app.MapStudentApi();
app.MapUserApi();
app.MapAuthApi();

app.Run();
