using Backend.Models;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUser(Guid Id,CancellationToken token);
        Task<UserModel> CreateUser(RegisterModel userModel, CancellationToken token);
        Task UpdateUserData(UserModel model, CancellationToken token);
        Task<UserModel> GetCurrentLoggedInUserData(CancellationToken token);
        Task ForgetPassword(string email);

    }
}
