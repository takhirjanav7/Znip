using AsosiyProject.Api.Configurations;
using AsosiyProject.Api.Hubs;
using AsosiyProject.Application.Common.Exceptions;
using AsosiyProject.Application.Validators.Commands.Follows.CreateFollow;
using AsosiyProject.Infrastructure;
using Microsoft.OpenApi.Models; 
using System.Text.Json.Serialization;

namespace AsosiyProject.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddEndpointsApiExplorer();

        // 2. AddSwaggerGen QISMINI MANA SHUNDAY O'ZGARTIRING:
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "AsosiyProject API", Version = "v1" });

            // JWT Bearer token sozlamalari (Qulf tugmasi chiqishi uchun)
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Tokenni quyidagi formatda kiriting: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        builder.Services.AddSignalR();
        // builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>(); // Agar bu class mavjud bo'lsa kommentdan oling

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.ConfigureJwt(); // Bu method token validatsiyasini qiladi (Infrastructure ichida bo'lsa kerak)

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Authentication va Authorization ketma-ketligi MUHIM
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        // app.UseDirectoryBrowser(); // Xavfsizlik uchun productionda o'chiq turgani ma'qul

        app.MapHub<NotificationHub>("/hubs/notifications");
        app.MapHub<ChatHub>("/hubs/chat");

        app.ConfigureEndpoints();

        app.Run();
    }
}