using EduCodePlatform.Application;
using EduCodePlatform.Application.Mappings.Users;
using EduCodePlatform.Infrastructure;
using EduCodePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace EduCodePlatform.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // AutoMapper
            builder.Services.AddAutoMapper(
                cfg => { },
                typeof(UserProfile)
            );

            builder.Services.AddHttpContextAccessor();

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


            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    //                        frontend
                    policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseCors("Default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

            app.Run();
        }
    }
}
