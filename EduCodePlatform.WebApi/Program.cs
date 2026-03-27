using AutoMapper;
using EduCodePlatform.Application;
using EduCodePlatform.Infrastructure;
using EduCodePlatform.WebApi;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using EduCodePlatform.WebApi.Middlewares;
using EduCodePlatform.Application.Mappings;

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

            app.UseHttpsRedirection();

            app.UseCors("Default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();
        }
    }
}
