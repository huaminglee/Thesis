﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Web;

namespace Thesis.Lib.Attributes
{
    public class SetCultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cultureCode = HttpContext.Current.Session["Culture"];
            CultureInfo culture = null;

            if (cultureCode != null)
                culture = new CultureInfo(cultureCode.ToString(), false);
            else if (HttpContext.Current.Request.Cookies["Language"] != null)
            {
                string code = HttpContext.Current.Request.Cookies["Language"].Value;
                culture = new CultureInfo(code, false);
                HttpContext.Current.Session["Culture"] = code;
            }

            if (culture != null)
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
