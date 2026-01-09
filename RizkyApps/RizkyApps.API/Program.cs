using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RizkyApps.API.Config;
using RizkyApps.API.Data;
using RizkyApps.API.Endpoints;
using RizkyApps.API.Services;
using RizkyApps.Shared.DTOs;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Load JWT configuration
//var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtConfiguration>()
//                ?? throw new InvalidOperationException("JWT configuration is missing");
var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtConfiguration>()
                ?? new JwtConfiguration
                {
                    Secret = builder.Configuration["JwtSettings:Secret"]!,
                    Issuer = builder.Configuration["JwtSettings:Issuer"]!,
                    Audience = builder.Configuration["JwtSettings:Audience"]!,
                    ExpireDays = int.TryParse(builder.Configuration["JwtSettings:ExpireDays"], out var days)
                        ? days : 7
                };

builder.Services.AddSingleton(jwtConfig);

// Configure authentication with .NET 10 Minimal API patterns
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig.Secret)),

            // .NET 10: Cleaner claim mapping approach
            // Instead of MapInboundClaims, set these properties for clean claim types
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };

        // .NET 10: Alternative approach for disabling inbound claim mapping
        // This prevents legacy XML claim type mappings (like http://schemas.xmlsoap.org/...)
        options.MapInboundClaims = false;

        // .NET 10: Better handling for API endpoints (no redirects for unauthorized)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => Task.CompletedTask
        };
    });

// .NET 10: Using AddAuthorizationBuilder for cleaner policy configuration
//builder.Services.AddAuthorizationBuilder()
//    .AddPolicy("AdminOnly", policy =>
//        policy.RequireRole("admin"))
//    .AddPolicy("UserAccess", policy =>
//        policy.RequireClaim("scope", "api_access"));

builder.Services.AddAuthorization();

// Register Identity Service (we'll create this next)
builder.Services.AddScoped<IdentityService>();

builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); // Register the fixed transformer
});


var app = builder.Build();



// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Purple)
            .WithTitle("🛒 Product API Explorer") // ✅ Correct: Use the string "modern"
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

// Register endpoint groups
app.RegisterExamEndpoints();
app.RegisterIdentityEndpoints();
app.RegisterUserEndpoints();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.Run();