using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Chat.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ChatDB>(options => options.UseInMemoryDatabase("messages"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OpenChat API",
        Description = "REST API for OpenChat",
        Version = "v1"
    });
});
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenChat API V1");
});

app.Run();
