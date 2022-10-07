using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Repositories;
using TescEvents.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

builder.Services.AddControllers();
builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(AppSettings.ConnectionString));

builder.Services.AddScoped<IEventRepository, EventRepository>();
    
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();