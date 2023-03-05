using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Chat.Models;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Chat") ?? "Data Source=Chat.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<ChatDB>(connectionString);
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("*").WithMethods("*").WithHeaders("*");
    });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

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
