using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MinecraftServerPanel
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthenticationConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<AuthenticationOptions>(configuration.GetSection("Auth"));

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect();

            services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
                .Configure(options => { options.AccessDeniedPath = "/Error"; });
            services.AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
                .Configure<IOptions<AuthenticationOptions>>((options, authOptions) =>
                {
                    options.Authority = authOptions.Value.Authority;
                    options.ClientId = authOptions.Value.ClientId;
                    options.ClientSecret = authOptions.Value.ClientSecret;
                    options.ResponseType = "code";
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.Events.OnTokenValidated = (ctx) =>
                    {
                        var userId = ctx.Principal.FindFirstValue(ClaimTypes.Name);
                        if (!authOptions.Value.ValidUsers.Contains(userId, StringComparer.OrdinalIgnoreCase))
                        {
                            ctx.Fail($"{userId} has no access");
                        }

                        return Task.CompletedTask;
                    };
                });
        }
    }
}