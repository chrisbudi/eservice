using E.ServiceCore.Identity.Data.Enities;

namespace E.ServiceCore.Identity.Api.Service.Interfaces
{
    public interface IAuthService
    {
        ApplicationUser Login(string username, string password);
        ApplicationUser LoginSSO(string username);
    }
}
