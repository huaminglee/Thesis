﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Thesis.Authorization.Services;
using Thesis.Authorization.Enumerations;
using Thesis.Common.Helpers;
using Thesis.Lib.ActionResults;
using Thesis.Common.Enumerations;

namespace Thesis.Lib.Attributes
{
    public class ViewAttribute : ActionFilterAttribute
    {
        object emptyValue;
        public ViewAttribute() : this(0)
        {

        }

        public ViewAttribute(object emptyValue)
        {
            this.emptyValue = emptyValue;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] modules = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ModuleAuthorizeAttribute), false);
            if (modules != null && modules.Length > 0)
            {                
                object idValue = null;
                if (filterContext.ActionParameters.ContainsKey("id"))
                    idValue = filterContext.RouteData.Values["id"];

                ModuleAuthorizeAttribute mod;
                foreach (var module in modules)
                {
                    mod = module as ModuleAuthorizeAttribute;
                    if (idValue != null && emptyValue != null && idValue.ToString().Equals(emptyValue.ToString()))
                    {
                        var permissions = ModuleAuthorizeService.GetModulePermissionsByModule(mod.module);
                        if (permissions == null || permissions.IndexOf(ProcessTypes.View) == -1 || permissions.IndexOf(ProcessTypes.Add) == -1)
                        {
                            filterContext.Result = new HttpUnauthorizedResult();
                            break;
                        }
                    }
                    else
                    {
                        if (!ModuleAuthorizeService.HasPermission(mod.module, ProcessTypes.View))
                        {
                            filterContext.Result = new HttpUnauthorizedResult();
                            break;
                        }
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class SaveAttribute : ActionFilterAttribute
    {
        object emptyValue;
        public SaveAttribute() : this(0)
        {

        }

        public SaveAttribute(object emptyValue)
        {
            this.emptyValue = emptyValue;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] modules = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ModuleAuthorizeAttribute), false);
            if (modules != null && modules.Length > 0)
            {
                object idValue = null;
                if (filterContext.ActionParameters.ContainsKey("id"))
                    idValue = filterContext.ActionParameters["id"];

                ModuleAuthorizeAttribute mod;
                foreach (var module in modules)
                {
                    mod = module as ModuleAuthorizeAttribute;
                    if (idValue == null || emptyValue == null || !ModuleAuthorizeService.HasPermission(mod.module, (idValue.ToString().Equals(emptyValue.ToString()) ? ProcessTypes.Add : ProcessTypes.Update)))
                    {
                        filterContext.Result = new UnauthorizedResult();
                        break;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class DeleteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] modules = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ModuleAuthorizeAttribute), false);
            if (modules != null && modules.Length > 0)
            {
                ModuleAuthorizeAttribute mod;
                foreach (var module in modules)
                {
                    mod = module as ModuleAuthorizeAttribute;
                    if (!ModuleAuthorizeService.HasPermission(mod.module, ProcessTypes.Delete))
                    {
                        filterContext.Result = new UnauthorizedResult();
                        break;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
