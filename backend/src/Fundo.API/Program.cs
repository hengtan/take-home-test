using Fundo.API.Extensions;
using Fundo.Application.DependencyInjection;
using Fundo.Infrastructure.DependencyInjection;
using Fundo.Infrastructure.Persistence;
using Fundo.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

SerilogConfiguration.Configure();
builder.Host.UseSerilog();

var env = builder.Environment;
var config = builder.Configuration;

builder.Services.Configure<JwtSettings>(config.GetSection("Jwt"));

builder.Services.AddInfrastructure(config);
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension();
builder.Services.AddAuthenticationExtension(config);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandling();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerExtension(env);

ApplyMigrations(app);
app.Run();
return;

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
    dbContext.Database.Migrate();
}

public partial class Program { }