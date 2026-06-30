using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AsosiyProject.Api.Configurations; 

public static class JwtConfiguration
{
    public static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        // ... JWT sozlamalaringiz (SecretKey, Issuer, Audience) ...
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // !!! SIGNALR UCHUN SHU YERNI QO'SHASIZ !!!
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // 1. Tokenni Query Stringdan qidiramiz (?access_token=...)
                    var accessToken = context.Request.Query["access_token"];

                    // 2. So'rov qaysi URL ga ketyapti?
                    var path = context.HttpContext.Request.Path;

                    // 3. Agar token bor bo'lsa VA so'rov "/hubs" bilan boshlansa (Sizda /hubs/notifications)
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        // Tokenni headerga o'tkazib qo'yamiz, Authentication ishlashi uchun
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }
}