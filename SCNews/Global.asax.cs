using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SCNews.Helpers;

namespace SCNews
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes. MapRoute(
                "ContentManager", // Route name
                "ContentManager/{action}/{id}", // URL with parameters
                new { controller = "Content", action = "Index", id = UrlParameter. Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Tagged", // Route name
                "Tags/{tagName}", // URL with parameters
                new { controller = "News", action = "Tags", tagName = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "TaggedAndPaginated", // Route name
                "Tags/{tagName}/Page/{page}", // URL with parameters
                new { controller = "News", action = "Tags", tagName = UrlParameter.Optional, page = UrlParameter.Optional } // Parameter defaults
            );


            #region Paginated
            routes.MapRoute(
                "PaginatedNews",
                "News/Page/{page}",
                new { controller = "News", action = "Index" }
            );

            routes.MapRoute(
                "PaginatedUsers",
                "User/Page/{page}",
                new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
                "PaginatedGallery",
                "Gallery/Page/{page}",
                new { controller = "Gallery", action = "Index" }
            );

            routes.MapRoute(
                "PaginatedVote",
                "Vote/Page/{page}",
                new { controller = "Vote", action = "Index" }
            );

            routes.MapRoute(
                "PaginatedReplays",
                "Replays/Page/{page}",
                new { controller = "Replays", action = "Index" }
            );
            #endregion

            routes.MapRoute(
                "RemoveChatMessage",
                "Chat/RemoveMessage/{messageId}",
                new { controller = "Chat", action = "RemoveMessage", messageId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "TagsCloud",
                "TagsCloud",
                new { controller = "News", action = "AllTags" }
            );

            #region UserProfile
            routes.MapRoute(
                "EditProfile",
                "Profile/Edit",
                new { controller = "Profile", action = "Edit" }
            );
            
            routes.MapRoute(
                "Profile",
                "Profile/{userName}",
                new { controller = "Profile", action = "Details", userName = UrlParameter.Optional }
            );
            #endregion

            
            #region RoleManagement
            routes.MapRoute(
                "ManageRoles"    ,
                "User/ManageRoles/{userName}",
                new { controller = "User", action = "ManageRoles", userName = UrlParameter.Optional }
            );

            routes.MapRoute(
                "RemoveRole"    ,
                "User/RemoveRole/{roleId}/{userId}",
                new { controller = "User", action = "RemoveRole", roleId = UrlParameter.Optional, userId = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AddRole"    ,
                "User/AddRole/{roleId}/{userId}",
                new { controller = "User", action = "AddRole", roleId = UrlParameter.Optional, userId = UrlParameter.Optional }
            );
            #endregion

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "News", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            Application["UsersCount"] = 0;

            RegisterRoutes( RouteTable.Routes );
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting( RouteTable.Routes );
        }

        protected void Session_Start()
        {
            Application.Lock();
            Application["UsersCount"] = (int)Application["UsersCount"] + 1;
            Application.UnLock();
        }

        protected void Session_End()
        {
            if ((int)Application["UsersCount"] > 0)
            {
                Application.Lock();
                Application["UsersCount"] = (int) Application["UsersCount"] - 1;
                Application.UnLock();
            }
        }
    }
}