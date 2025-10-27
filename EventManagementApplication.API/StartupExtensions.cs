using EventManagementApplication.API.Middleware;
using EventManagementApplication.API.Services;
using EventManagementSystem.Application;
using EventManagementSystem.Application.Contracts;
using EventManagementSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace EventManagementApplication.API;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        AddSwagger(builder.Services);

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers(configure =>
        {
            configure.ReturnHttpNotAcceptable = true;
        })
        .AddNewtonsoftJson(setupAction =>
        {
            setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management System API");
            });
        }

        app.ConfigureExceptionHandler();

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Event Management System API",
            });

            setupAction.AddSecurityDefinition("EventManagementSystemApiBearerAuth", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Input a valid token to access this API"
            });

            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "EventManagementSystemApiBearerAuth"
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (context != null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}
