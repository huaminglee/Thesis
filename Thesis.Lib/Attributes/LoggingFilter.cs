﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Thesis.Common.Helpers;

namespace Thesis.Lib.Attributes
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        DateTime dt;
        string logPath;

        public LoggingFilterAttribute()
        {
            logPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Log.txt");

            if (!System.IO.File.Exists(logPath))
                System.IO.File.Create(logPath);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dt = DateTime.Now;
            Log("OnActionExecuting : " + DateTime.Now.ToLongTimeString(), filterContext.RouteData);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted : " + DateTime.Now.ToLongTimeString(), filterContext.RouteData);

            if (filterContext.Exception != null)
            {
                
                //string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                //string action = filterContext.ActionDescriptor.ActionName;
                //string message = filterContext.Exception.Message;
                //string stackTrace = filterContext.Exception.StackTrace;

                Log("Error", filterContext.RouteData);

                //filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");
            }

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting : " + DateTime.Now.ToLongTimeString(), filterContext.RouteData);
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted : " + DateTime.Now.ToLongTimeString(), filterContext.RouteData);
            base.OnResultExecuted(filterContext);
        }

        private void Log(string text, System.Web.Routing.RouteData routeData)
        {
            if (!Ax.EnableLoggingFilter)
                return;

            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];

            using (System.IO.StreamWriter sw = System.IO.File.AppendText(logPath))
            {
                if (text.StartsWith("OnActionExecuting"))
                    sw.WriteLine(string.Format("Page = /{0}/{1}", controllerName, actionName));

                sw.WriteLine(text);

                if (text.StartsWith("OnResultExecuted"))
                {
                    sw.WriteLine("------------------");
                    sw.WriteLine("Total Milliseconds = " + (DateTime.Now.TimeOfDay - dt.TimeOfDay).TotalMilliseconds.ToString());
                    sw.WriteLine("------------------");
                }
            }
        }
    }
}
