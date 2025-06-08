using TP.Domain.Enum;
using TP.Domain.Models.Result;

namespace TP.Domain.Queries.User
{
    public record GetUserAuthorisationQuery(string Username, string Password) : IQuery<WorkResult<IEnumerable<UserRoleEnum>>> { }

    public class GetUserAuthorisationQueryHandler : IQueryHandler<GetUserAuthorisationQuery, WorkResult<IEnumerable<UserRoleEnum>>>
    {
        public async Task<WorkResult<IEnumerable<UserRoleEnum>>> HandleAsync(GetUserAuthorisationQuery command)
        {
            if (command.Username == "admin" && command.Password == "admin123")
            {
                return new WorkResult<IEnumerable<UserRoleEnum>>([UserRoleEnum.ADMINISTRATOR, UserRoleEnum.USER])
                {
                    IsSuccess = true,
                    Message = "Admin user authenticated successfully"
                };
            }

            if (command.Username == "user" && command.Password == "user123")
            {
                return new WorkResult<IEnumerable<UserRoleEnum>>([UserRoleEnum.USER])
                {
                    IsSuccess = true,
                    Message = "Regular user authenticated successfully"
                };
            }

            return new WorkResult<IEnumerable<UserRoleEnum>>([])
            {
                IsSuccess = false,
                Message = "Incorrect authorisation details"
            };
        }
    }
}
