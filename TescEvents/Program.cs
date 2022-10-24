using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Utilities;
using TescEvents.Utilities.Profiles;
using TescEvents.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

// Add Automapper configuration
builder.Services.AddAutoMapper(typeof(EventProfile), typeof(UserProfile));

builder.Services.AddControllers();
builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(AppSettings.ConnectionString));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Add validators
builder.Services.AddScoped<IValidator<Event>, EventValidator>();
builder.Services.AddScoped<IValidator<Student>, StudentValidator>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") 
                                   ?? throw new InvalidOperationException("JWT_KEY is invalid"))
            ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) SeedDb();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

void SeedDb() {
    using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    context.Events.AddRange(new Event {
        Id = Guid.NewGuid(),
        Title = "Event 1",
        Start = new DateTime(2021, 5, 23, 9, 0, 0).ToUniversalTime(),
        End = new DateTime(2021, 5, 23, 10, 0, 0).ToUniversalTime()
    }, new Event {
        Id = Guid.NewGuid(),
        Title = "Event 2",
        Start = new DateTime(2022, 7, 23, 9, 0, 0).ToUniversalTime(),
        End = new DateTime(2022, 7, 23, 11, 30, 0).ToUniversalTime()
    }, new Event {
        Id = Guid.NewGuid(),
        Title = "Event 3",
        Start = new DateTime(2022, 9, 29, 11, 0, 0).ToUniversalTime(),
        End = new DateTime(2022, 9, 23, 14, 0, 0).ToUniversalTime()
    });
    
    context.Students.AddRange(new Student {
        Id = Guid.NewGuid(),
        Username = "sek007@ucsd.edu",
        FirstName = "Shane",
        LastName = "Kim",
        PasswordHash = "",
        Salt = "reallygoodsalt",
        UserType = UserTypes.REGULAR
    });
    context.SaveChanges();
}