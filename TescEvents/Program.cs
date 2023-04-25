using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TescEvents.DTOs;
using TescEvents.Entities;
using TescEvents.Profiles;
using TescEvents.Repositories;
using TescEvents.Services;
using TescEvents.Utilities;
using TescEvents.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DB Context
var dbOptions = builder.Configuration.GetSection(DbOptions.Db).Get<DbOptions>();
var connectionString = DbOptions.ConnectionString(dbOptions.Host, dbOptions.Port, dbOptions.Database, dbOptions.User, 
                                                  dbOptions.Password);
builder.Services.AddDbContext<RepositoryContext>(options => 
                                                     options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IValidator<StudentCreateRequestDTO>, UserCreateRequestValidator>();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Jwt));
builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(DbOptions.Db));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
                                                    Encoding.UTF8.GetBytes(jwtOptions.Key)
                                                   ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});
    
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();