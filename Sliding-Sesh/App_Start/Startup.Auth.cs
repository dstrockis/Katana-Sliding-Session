using System;
using System.Collections.Generic;
using System.Web;

//The following libraries were added to this sample.
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

//The following libraries were defined and added to this sample.
using Sliding_Sesh.Utils;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using System.Web.Routing;
using Sliding_Sesh.App_Start;

namespace Sliding_Sesh
{

    public partial class Startup
    {
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The App Key is a credential used to authenticate the application to Azure AD.  Azure AD supports password and certificate credentials.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        // The Authority is the sign-in URL of the tenant.
        // The GraphResourceId the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.

        private static readonly string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static readonly string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        private static readonly string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static readonly string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static readonly string postLogoutRedirectUri =
            ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        public static readonly string Authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        private static string graphResourceId = "https://graph.windows.net";

        /// <summary>
        /// Configures OpenIDConnect Authentication & Adds Custom Application Authorization Logic on User Login.
        /// </summary>
        /// <param name="app">The application represented by a <see cref="IAppBuilder"/> object.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            
            // TODO
            CookieAuthenticationOptions cao = new CookieAuthenticationOptions 
            {
                ExpireTimeSpan = TimeSpan.FromSeconds(15),
                SlidingExpiration = true,
            };

            app.Use(typeof(MyCookieAuthenticationMiddleware), app, cao);

            //Configure OpenIDConnect, register callbacks for OpenIDConnect Notifications
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = Authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    RedirectUri = postLogoutRedirectUri,
                    
                    //TODO
                    UseTokenLifetime = false,
                    
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        // This is also where we hook into a user login event and add our own custom application authorization logic.
                        AuthorizationCodeReceived = context =>
                        {
                            //var credential = new ClientCredential(clientId, appKey);
                            //string userObjectId = context.AuthenticationTicket.Identity.FindFirst(
                            //    "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                            //// Configure a new <see cref="NaiveSessionCache" \> for storing the access token
                            //// and making it accessible throughout the entire application.
                            //var authContext = new AuthenticationContext(Authority, new NaiveSessionCache(userObjectId));

                            //// Acquire Access Token
                            //AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                            //    context.Code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential,
                            //    graphResourceId);

                            return Task.FromResult(0);
                        },
                        

                        RedirectToIdentityProvider = context =>
                        {
                            //var cookie = context.Request.Cookies[cao.CookieName];
                            //if (cookie != null)
                            //{
                            //    IDataProtector idp =
                            //        app.CreateDataProtector(
                            //            "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware", "Cookies",
                            //            "v1");
                            //    TicketDataFormat tdf = new TicketDataFormat(idp);
                            //    var ticket = tdf.Unprotect(cookie);
                            //    if (ticket.Properties.ExpiresUtc <= DateTime.UtcNow)
                            //    {
                            //        // Default: Bounceback

                            //        // Option 1: Append Prompt=Login to Query String to Show Credentials UI. Can be gamed.
                            //        //context.ProtocolMessage.Prompt = "login";

                            //        // Option 2: Redirect to Error Page. Haven't made logout request, so bounceback happens when user clicks links.
                            //        //context.OwinContext.Authentication.SignOut(
                            //        //    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
                            //        //HttpContext.Current.Response.RedirectToRoute(new RouteValueDictionary(
                            //        //    new
                            //        //    {
                            //        //        controller = "Error",
                            //        //        action = "ShowError",
                            //        //        errorMessage = "Your session has timed out."
                            //        //    })
                            //        //);

                            //        // Option 3: Redirect to another page, which calls OwinContext.SignOut, issuing a SignOut request,
                            //        // Logging the user out, and returning to an error page using AuthenticationProperties.
                            //        //context.HandleResponse();
                            //        //HttpContext.Current.Response.RedirectToRoute(new RouteValueDictionary(
                            //        //    new
                            //        //    {
                            //        //        controller = "Account",
                            //        //        action = "SessionTimeOut",
                            //        //    })
                            //        //);

                            //        // Option 4: Single Sign Out (Coming Soon)
                            //    }
                            //}
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}