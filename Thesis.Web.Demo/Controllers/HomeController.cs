﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using System.Xml.Linq;
using Thesis.Lib.Controllers;

namespace Thesis.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["AppName"] = "<b>Thesis</b> @Bilgi";
            ViewData["Username"] = this.HttpContext.User.Identity.Name;

            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public AjaxStoreResult GetHomeSchema()
        {
            XElement document = XElement.Load(Server.MapPath("~/Content/Dashboard/HomeSchema.xml"));
            var defaultIcon = document.Attribute("defaultIcon") != null ? document.Attribute("defaultIcon").Value : "";
            var query = from g in document.Elements("group")
                        select new
                        {
                            Title = g.Attribute("title") != null ? g.Attribute("title").Value : "",
                            Items = (from i in g.Elements("item")
                                         select new
                                         {
                                             Title = i.Element("title") != null ? i.Element("title").Value : "",
                                             Icon = i.Element("item-icon") != null ? i.Element("item-icon").Value : defaultIcon,
                                             Accordion = i.Element("accordion-item") != null ? i.Element("accordion-item").Value : "",
                                             MenuItem = i.Element("menu-item") != null ? i.Element("menu-item").Value : ""
                                         }
                                     )
                        };

            return new AjaxStoreResult(query);
        }

        [HttpPost]
        public ActionResult ChangeTheme(int theme)
        {
            Session["Ext.Net.Theme"] = (Ext.Net.Theme)theme;
            return new AjaxResult();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
