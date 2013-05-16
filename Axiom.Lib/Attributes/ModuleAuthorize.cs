using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Authorization.Enumerations;
using System.Web.Routing;
using System.Web.Mvc;

namespace Thesis.Lib.Attributes
{
    public class ModuleAuthorizeAttribute : Attribute
    {
        public string module;

        public ModuleAuthorizeAttribute()
        {
            this.module = System.Web.HttpContext.Current.
                            Request.RequestContext.RouteData.GetRequiredString("controller");
        }

        public ModuleAuthorizeAttribute(string module)
        {
            this.module = module;
        }
    }
}
