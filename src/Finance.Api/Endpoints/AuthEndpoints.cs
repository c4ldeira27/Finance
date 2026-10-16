using Finance.Api.Contracts.Auth;
using Finance.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace Finance.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/v1/auth").WithTags("Auth");


            group.MapPost("register", Register);
            group.MapPost("login", Login);
            group.MapPost("logout", Logout).RequireAuthorization();
            group.MapGet("me", Me).RequireAuthorization();


        }

        private static Results<Ok<MeResponse>, UnauthorizedHttpResult> Me(ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = user.FindFirstValue(ClaimTypes.Email);

            if (id is null || email is null)
                return TypedResults.Unauthorized();

            return TypedResults.Ok(new MeResponse(id, email));
        }

        private static async Task<NoContent> Logout(IIdentityService service)
        {
            await service.LogoutAsync();

            return TypedResults.NoContent();
        }

        private static async Task<Results<NoContent, ProblemHttpResult>> Login(LoginRequest request, IIdentityService service)
        {
            var result = await service.LoginAsync(request.Email, request.Password);

            return result switch
            {
                LoginResult.Success => TypedResults.NoContent(),
                LoginResult.LockedOut => TypedResults.Problem(
                    title: "Account temporarily locked due to many failed attempts.",
                    statusCode: StatusCodes.Status429TooManyRequests),
                _ => TypedResults.Problem(
                    title: "Invalid email or password.",
                    statusCode: StatusCodes.Status401Unauthorized
                    )
            };

        }

        private static async Task<Results<Created<RegisterResponse>, ValidationProblem>> Register(RegisterRequest request, IIdentityService service)
        {
            var result = await service.RegisterAsync(request.Email, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .GroupBy(error => error.Code)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.Description).ToArray());

                return TypedResults.ValidationProblem(errors);
            }

            return TypedResults.Created((string?)null, new RegisterResponse(result.UserId!));
        }
    }
}
