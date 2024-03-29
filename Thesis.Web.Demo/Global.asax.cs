﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Thesis.Common.ViewModels;
using Thesis.Lib.ModelBinders;
using Thesis.Infrastructure;
using Thesis.Lib.ControllerFactory;
using Thesis.Lib.GlobalFilters;

namespace Thesis
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
        {
            string url = HttpContext.Current.Request.RawUrl.ToLower();
            if (url.Contains("ext.axd"))
            {
                HttpContext.Current.SkipAuthorization = true;
            }
            else if (url.Contains("returnurl=/default.aspx") || url.Contains("returnurl=%2fdefault.aspx"))
            {
                Response.Redirect(url.Replace("returnurl=/default.aspx", "r=/").Replace("returnurl=%2fdefault.aspx", "r=/"));
            }
        } 

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{exclude}/{extnet}/ext.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter()); 
            filters.Add(new HandleErrorAttribute());
        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {
            //Response.AppendHeader("X-UA-Compatible", "IE=8");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            RegisterGlobalFilters(GlobalFilters.Filters);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(new AxiomModuleServices()));
            
            ModelBinders.Binders.Add(typeof(List<int>)            , new DeleteItemModelBinder());
            ModelBinders.Binders.Add(typeof(List<Guid>)           , new DeleteItemModelBinderForGuid());
            ModelBinders.Binders.Add(typeof(List<FilterViewModel>), new FilterViewModelModelBinder());
        }
    }
}