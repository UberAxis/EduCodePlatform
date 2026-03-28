using AutoMapper;
using EduCodePlatform.Application;
using EduCodePlatform.Application.Mappings;
using EduCodePlatform.Infrastructure;
using EduCodePlatform.Infrastructure.Persistence;
using EduCodePlatform.WebApi;
using EduCodePlatform.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace EduCodePlatform.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AutoMapper
            builder.Services.AddAutoMapper(
                cfg => { },
                typeof(UserProfile)
            );

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://127.0.0.1:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // Controllers
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            // Application layer
            builder.Services.AddApplication();

            // Infrastucture layer
            builder.Services.AddInfrastructure(builder.Configuration);

            // JWT Authorization
            builder.Services.AddJwtAuth(builder.Configuration);

            // Swagger
            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors("Default");

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = services.GetRequiredService<AppDbContext>();

                try
                {
                    db.Database.Migrate();
                    await DbInitializer.SeedData(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            app.Run();
        }
    }
}
