using System;
using System.Web.Mvc;

namespace ePhoto.NET.Helpers {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class DbAuthorizerAttribute : FilterAttribute, IAuthorizationFilter {
        public bool AdminRequired { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext) {
            var request = filterContext.HttpContext.Request;

            if (request.IsAuthenticated && (!AdminRequired || filterContext.HttpContext.FindUser().IsAdmin))
                return;

            var urlHelper = DependencyResolver.Current.GetService<UrlHelper>();

            filterContext.Result = new RedirectResult(urlHelper.Action(T4Routes.Account.Login(request.Url.AbsolutePath)));
        }
    }
}