using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MailManager.Data;
using CryptSharp;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace MailManager.Security
{
    public class UserManager : IUserManager
    {        
        private DataContext _dataContext;

        private IConfiguration _configuration;

        public UserManager(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public Account Validate(string username, string password)
        {
            var account = _dataContext.Accounts.Where(a => a.Username == username).FirstOrDefault();

            if (account != null)
            {
                string passwordHash = account.Password.Substring(account.Password.IndexOf("$6$"));
                string salt = passwordHash.Substring(0, 19);
                string calculatedHash = Crypter.Sha512.Crypt(password, salt);

                if (passwordHash == calculatedHash) return account;
            }

            return null;
        }

        public async void SignIn(HttpContext httpContext, Account account, bool isPersistent = false)
        {
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(account), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = isPersistent }
            );
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool WeakPassword(string password)
        {
            int passwordStrength = int.Parse(_configuration["Security:PasswordStrength"]);
            int score = 0;

            if (string.IsNullOrEmpty(password))
                return true;

            if (password.Length >= 4)
                score++;

            if (password.Length >= 8)
                score++;

            if (password.Length >= 12)
                score++;

            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))
                score++;

            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript))
                score++;

            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript))
                score++;

            return score <= passwordStrength;
        }

        private IEnumerable<Claim> GetUserClaims(Account account)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, account.Username));

            if (_configuration["Security:AdminUser"] == account.Username)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            };
            
            return claims;
        }
    }
}
