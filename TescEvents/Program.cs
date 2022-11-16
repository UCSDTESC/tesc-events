using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TescEvents.DTOs;
using TescEvents.Entities;
using TescEvents.Profiles;
using TescEvents.Services;
using TescEvents.Utilities;
using TescEvents.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IValidator<UserCreateRequestDTO>, UserCreateRequestValidator>();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddControllers();
builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(AppSettings.ConnectionString));
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

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();