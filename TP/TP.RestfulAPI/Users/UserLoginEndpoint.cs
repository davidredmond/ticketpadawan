using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity.Data;
using TP.Domain;
using TP.Domain.Enum;
using TP.Domain.Models.Result;
using TP.Domain.Queries.User;

namespace TP.RestfulAPI.Users
{
    public class UserLoginEndpoint : Endpoint<LoginRequest>
    {
        private readonly IDispatcher _dispatcher;

        public UserLoginEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Post("/user/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var authorisationResult = await _dispatcher.SendQueryAsync<GetUserAuthorisationQuery, WorkResult<IEnumerable<UserRoleEnum>>>(new GetUserAuthorisationQuery(req.Email, req.Password));

            if (authorisationResult.IsSuccess && authorisationResult.Value != null)
            {
                var jwtToken = JwtBearer.CreateToken(a =>
                {
                    a.ExpireAt = DateTime.UtcNow.AddHours(1);
                    a.User.Claims.Add(("Username", req.Email));

                    foreach (var role in authorisationResult.Value)
                    {
                        a.User.Roles.Add(role.ToFriendlyString());
                    }
                });
                await SendAsync(new
                {
                    Username = req.Email,
                    Token = jwtToken,
                });
                return;
            }

            await SendResultAsync(TypedResults.Unauthorized());
        }
    }
}
