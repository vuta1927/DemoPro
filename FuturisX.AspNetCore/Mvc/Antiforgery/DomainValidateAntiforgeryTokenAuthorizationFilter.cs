﻿using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FuturisX.AspNetCore.Mvc.Antiforgery
{
    public class DomainValidateAntiforgeryTokenAuthorizationFilter : ValidateAntiforgeryTokenAuthorizationFilter
    {
        private readonly AntiforgeryOptions _options;

        public DomainValidateAntiforgeryTokenAuthorizationFilter(
            IAntiforgery antiforgery,
            ILoggerFactory loggerFactory,
            IOptions<AntiforgeryOptions> options)
            : base(antiforgery, loggerFactory)
        {
            _options = options.Value;
        }

        protected override bool ShouldValidate(AuthorizationFilterContext context)
        {
            if (!base.ShouldValidate(context))
            {
                return false;
            }

            //No need to validate if antiforgery cookie is not sent.
            //That means the request is sent from a non-browser client.
            //See https://github.com/aspnet/Antiforgery/issues/115
            if (!context.HttpContext.Request.Cookies.ContainsKey(_options.Cookie.Name))
            {
                return false;
            }

            // Anything else requires a token.
            return true;
        }
    }
}