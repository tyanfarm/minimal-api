using SimpleMinimalAPI.Data;
using SimpleMinimalAPI.Modules;

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
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    "v1"
));

app.MapStudentApi();

app.Run();
