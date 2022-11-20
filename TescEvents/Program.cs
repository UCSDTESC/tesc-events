using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TescEvents.DTOs;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Profiles;
using TescEvents.Services;
using TescEvents.Utilities;
using TescEvents.Validators;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = env == Environments.Development;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
try {
    var root = Directory.GetCurrentDirectory();
    var dotenv = Path.Combine(root, ".env");
    DotEnv.Load(dotenv);
} catch {
    // ignored
}

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Configuration.AddEnvironmentVariables().Build();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IValidator<UserCreateRequestDTO>, UserCreateRequestValidator>();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(AppSettings.ConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(
                                                    Encoding.UTF8.GetBytes(Environment
                                                                               .GetEnvironmentVariable("JWT_KEY")
                                                                           ?? throw new
                                                                               InvalidOperationException("JWT_KEY is invalid"))
                                                   ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
    };
});


var app = builder.Build();
//if (isDevelopment) SeedDb();
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope()) {
    var context = serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>();
    context.Database.Migrate();
}
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
    
    context.Batches.AddRange(new Batch() {
        Capacity = 100,
        Length = new TimeSpan(0, 2, 0, 0),
        Timeslot = new DateTime(2022, 11, 15).ToUniversalTime(),
        WaitlistCapacity = 30
    });

    context.SaveChanges();
}