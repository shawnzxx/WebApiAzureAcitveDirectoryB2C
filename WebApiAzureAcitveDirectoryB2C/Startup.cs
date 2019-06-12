﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApiAzureAcitveDirectoryB2C
{
    public class Startup
    {
        // These values are pulled from web.config
        public static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string signUpSignInPolicy = ConfigurationManager.AppSettings["ida:SignUpSignInPolicyId"];
        public static string editProfilePolicy = ConfigurationManager.AppSettings["ida:UserProfilePolicyId"];

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Web API routes
            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signUpSignInPolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(editProfilePolicy));
        }

        private OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string policy)
        {
            var metadataEndpoint = string.Format(aadInstance, tenant, policy);

            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // This is where you specify that your API only accepts tokens from its own clients
                ValidAudience = clientId,
                AuthenticationType = policy,
                NameClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier"
            };

            return new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C metadata & signing keys from the OpenIDConnect metadata endpoint
                AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(metadataEndpoint))
            };
        }
    }
}