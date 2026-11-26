
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using WorkSync;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "WorkSync.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/index.html";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Temporary user storage
var users = new ConcurrentDictionary<string, DemoUser>(
    StringComparer.OrdinalIgnoreCase);

var passwordHasher = new PasswordHasher<DemoUser>();

// Default administrator
var admin = new DemoUser
{
    FullName = "System Administrator",
    Username = "admin",
    Role = "Admin"
};

admin.PasswordHash = passwordHasher.HashPassword(
    admin, "Admin@12345");

users.TryAdd(admin.Username, admin);

// Static files and authentication
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// EMPLOYEE REGISTRATION
app.MapPost("/api/register", async (
    RegisterRequest request,
    HttpContext context) =>
{
    if (string.IsNullOrWhiteSpace(request.FullName) ||
        string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new
        {
            message = "Please complete all fields."
        });
    }

    var username = request.Username.Trim();

    if (request.FullName.Length > 100 ||
        username.Length < 3 ||
        username.Length > 30 ||
        request.Password.Length < 8)
    {
        return Results.BadRequest(new
        {
            message = "Username must be 3-30 characters and password at least 8 characters."
        });
    }

    if (username.Any(char.IsWhiteSpace))
    {
        return Results.BadRequest(new
        {
            message = "Username cannot contain spaces."
        });
    }

    var user = new DemoUser
    {
        FullName = request.FullName.Trim(),
        Username = username,
        Role = "Employee"
    };

    user.PasswordHash = passwordHasher.HashPassword(
        user, request.Password);

    if (!users.TryAdd(user.Username, user))
    {
        return Results.Conflict(new
        {
            message = "Username already exists."
        });
    }

    await SignInUser(context, user);

    return Results.Ok(new
    {
        message = "Account created successfully!",
        redirect = "/employee-dashboard.html"
    });
});

// LOGIN
app.MapPost("/api/login", async (
    LoginRequest request,
    HttpContext context) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) ||
        string.IsNullOrWhiteSpace(request.Password) ||
        (request.Role != "Employee" &&
         request.Role != "Admin"))
    {
        return Results.BadRequest(new
        {
            message = "Please enter valid login information."
        });
    }

    if (!users.TryGetValue(request.Username.Trim(), out var user))
    {
        return Results.Unauthorized();
    }

    if (user.Role != request.Role)
    {
        return Results.Json(
            new
            {
                message = "This account does not match the selected role."
            },
            statusCode: 403);
    }

    var result = passwordHasher.VerifyHashedPassword(
        user, user.PasswordHash, request.Password);

    if (result == PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }

    await SignInUser(context, user);

    var destination = user.Role == "Admin"
        ? "/admin-dashboard.html"
        : "/employee-dashboard.html";

    return Results.Ok(new
    {
        message = "Login successful!",
        redirect = destination
    });
});

// CHECK CURRENT ACCOUNT
app.MapGet("/api/me", (HttpContext context) =>
{
    return Results.Ok(new
    {
        name = context.User.Identity?.Name,
        role = context.User.FindFirst(ClaimTypes.Role)?.Value
    });
}).RequireAuthorization();

// LOGOUT
app.MapPost("/api/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Ok(new
    {
        message = "Logged out successfully."
    });
});

app.Run();

// SIGN IN HELPER
static async Task SignInUser(
    HttpContext context,
    DemoUser user)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("FullName", user.FullName)
    };

    var identity = new ClaimsIdentity(
        claims,
        CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal);
}
