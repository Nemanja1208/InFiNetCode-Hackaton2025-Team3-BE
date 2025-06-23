using Application_Layer;
using Application_Layer.Common.Mappings;
using Application_Layer.IdeaSessions.Queries.GetIdeaSessionById;
using Domain_Layer.Models;
using Infrastructure_Layer;
using Infrastructure_Layer.Auth;
using Infrastructure_Layer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetIdeaSessionByIdHandler).Assembly);
});

// Identity
builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Auth
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

// CORS
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

// Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
