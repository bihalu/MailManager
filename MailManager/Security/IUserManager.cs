using Microsoft.AspNetCore.Http;
using MailManager.Data;

namespace MailManager.Security
{
    public interface IUserManager
    {
        Account Validate(string username, string password);

        void SignIn(HttpContext httpContext, Account user, bool isPersistent = false);

        void SignOut(HttpContext httpContext);

        bool WeakPassword(string password);
    }
}
