using System.Web.Mvc;
using System;

namespace Thesis.Areas.Settings
{
    public class SettingsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Settings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Settings_UserManagement",
                "Settings/{controller}/{action}/{id}",
                new { action = "List", id = UrlParameter.Optional },
                new { controller = "UserManagement" }
            );

            context.MapRoute(
                "Settings_RoleManagement",
                "Settings/{controller}/{action}/{id}",
                new { action = "List", id = UrlParameter.Optional },
                new { controller = "RoleManagement" }
            );

            context.MapRoute(
                "Settings_default",
                "Settings/{controller}/{action}/{id}",
                new { action = "List", id = 0 },
                new { id = @"\d+" }
            );
        }
    }
}
