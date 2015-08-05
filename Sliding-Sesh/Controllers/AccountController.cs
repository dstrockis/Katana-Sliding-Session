using System.Web;
using System.Web.Mvc;

//The following libraries were added to this sample.
using System.Security.Claims;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

//The following libraries were defined and added to this sample.
using Sliding_Sesh.Utils;

namespace Sliding_Sesh.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Sends an OpenIDConnect Sign-In Request.
        /// </summary>
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext()
                    .Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                        OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Signs the user out and clears the cache of access tokens.
        /// </summary>
        public void SignOut()
        {
            // Remove all cache entries for this user and send an OpenID Connect sign-out request.
            // TODO: Check for existense of ClaimsPrincipal if(notnull)
            Claim userObjectID =
                ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
            if (userObjectID != null && userObjectID.Value != null)
            {
                var authContext = new AuthenticationContext(Startup.Authority, new NaiveSessionCache(userObjectID.Value));
                authContext.TokenCache.Clear();
            }

            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}