using Microsoft.AspNetCore.Identity;
using Infrastructure_Layer.Data;
using Infrastructure_Layer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain_Layer.Models;
using Application_Layer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Infrastructure_Layer.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["authToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    })
    .AddGoogle(options =>
    {
        var config = builder.Configuration;
        options.ClientId = config["Authentication:Google:ClientId"]!;
        options.ClientSecret = config["Authentication:Google:ClientSecret"]!;
        options.CallbackPath = "/signin-google";

        options.Events = new OAuthEvents
        {
            OnTicketReceived = async context =>
            {
                var handler = context.HttpContext.RequestServices.GetRequiredService<OAuthLoginHandler>();
                await handler.HandleTicketAsync(context, "Google");
            }
        };
    })
    .AddGitHub(options =>
    {
        var config = builder.Configuration;
        options.ClientId = config["Authentication:GitHub:ClientId"]!;
        options.ClientSecret = config["Authentication:GitHub:ClientSecret"]!;
        options.CallbackPath = "/signin-github";
        options.Scope.Add("user:email");

        options.Events = new OAuthEvents
        {
            OnTicketReceived = async context =>
            {
                var handler = context.HttpContext.RequestServices.GetRequiredService<OAuthLoginHandler>();
                await handler.HandleTicketAsync(context, "GitHub");
            }
        };
    });

builder.Services.AddAuthorization();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(allowedOrigins ?? [])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(); // Add this line

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Add this line
    app.UseSwaggerUI(); // Add this line
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
