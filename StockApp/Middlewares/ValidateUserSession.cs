using Microsoft.AspNetCore.Http;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using StockApp.Core.Application.ViewModels.User;

namespace WebApp.StockApp.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userViewModel == null)
            {
                return false;
            }
            return true;
        }

    }
}
