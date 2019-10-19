using System.Web;
using System.Web.Mvc;

using ePhoto.NET.Models;

namespace ePhoto.NET.Helpers {
    public static class Extensions {
        public static UserModel FindUser(this HttpContext context, bool guestAllowed = true) {
            return new HttpContextWrapper(context).FindUser(guestAllowed);
        }

        public static UserModel FindUser(this HttpContextBase context, bool guestAllowed = true) {
            if (context.Request.IsAuthenticated)
                return DependencyResolver.Current.GetService<IPhotoSharingContext>().FindUserByEmail(context.User.Identity.Name);

            if(guestAllowed)
                return new UserModel {DisplayName = "مهمان", Email = "guest@ephoto.net"};

            return null;
        }
    }
}