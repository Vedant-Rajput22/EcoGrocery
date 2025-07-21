using Application.Common;
using Application.Features.Orders.Commands;
using Application.Features.Products.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Stripe;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EcoDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<EcoDbContext>());

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Dev", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});
builder.Services
    .AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<EcoDbContext>();
var stripeCfg = builder.Configuration.GetSection("Stripe");
Stripe.StripeConfiguration.ApiKey = stripeCfg["SecretKey"];

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
var jwtSection = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            RoleClaimType = ClaimTypes.Role
        };


        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = c =>
            {
                Console.WriteLine($"JWT FAIL ?? {c.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = c =>
            {
                Console.WriteLine($"JWT OK   ?? userId={c.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<MappingProfile>());
builder.Services.AddValidatorsFromAssemblyContaining<PlaceOrderCommand>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommand>();
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(Application.Infrastructure.Behaviors.ValidationBehavior<,>));
builder.Services.AddControllers().AddJsonOptions(o =>
 o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EcoGrocery API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Paste **only** the raw JWT – UI adds the Bearer prefix"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EcoDbContext>();
    var um = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var rm = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await Infrastructure.Data.Seed.EcoDbContextSeed.SeedAsync(db, um, rm);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Dev");
app.Use(async (ctx, next) =>
{
    Console.WriteLine($"Authorization header = [{ctx.Request.Headers["Authorization"]}]");
    await next();
});
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (InvalidOperationException ex)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (DbUpdateConcurrencyException)
    {
        ctx.Response.StatusCode = 409;
        await ctx.Response.WriteAsJsonAsync(new { error = "Stock changed—refresh cart." });
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.Run();
