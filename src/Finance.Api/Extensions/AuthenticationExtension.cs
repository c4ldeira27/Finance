using Finance.Api.Identity;
using Finance.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Finance.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "finance.auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
