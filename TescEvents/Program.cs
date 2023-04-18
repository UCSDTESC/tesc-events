using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TescEvents.Entities;
using TescEvents.Repositories;
using TescEvents.Utilities;
using TescEvents.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

builder.Services.AddControllers();
builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(AppSettings.ConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IEventService, EventService>();
    
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();