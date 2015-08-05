using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;

namespace Sliding_Sesh.App_Start
{
    // TODO: Create this derived middleware
    class MyCookieAuthenticationMiddleware : CookieAuthenticationMiddleware
    {
        private readonly ILogger _logger;

        public MyCookieAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, CookieAuthenticationOptions options) : base(next, app, options) 
        {
            _logger = app.CreateLogger<CookieAuthenticationMiddleware>();
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="CookieAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<CookieAuthenticationOptions> CreateHandler()
        {
            return new MyCookieAuthenticationHandler(_logger);
        }

    }
}
