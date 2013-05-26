using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using System.Web.Routing;
using System.Security.Principal;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Globalization;
using Thesis.Lib.Attributes;
using Thesis.Lib.Controllers;
using System.Web.Security;
using Thesis.Authorization.Services;
using Thesis.Authorization.Models;

namespace Thesis.Controllers
{
    public class AccountController : DefaultController
    {
        public AccountController(IFormsAuthenticationService formsService, IMembershipService membershipService)
        {
            FormsService = formsService;
            MembershipService = membershipService;
        }

        /// <summary>
        /// Get set the form authentication service.
        /// </summary>
        public IFormsAuthenticationService FormsService { get; private set; }

        /// <summary>
        /// Get / Set the membership service
        /// </summary>
        public IMembershipService MembershipService { get; private set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
            else
            {
                base.Initialize(requestContext);
            }
        }

        private string GetLanguageName(ref string code)
        {
            if (code == "en-US") return "English";
            else if (code == "es-ES") return "España";
            else if (code == "nl-NL") return "Nederlands";
            else if (code == "de-DE") return "Deutsche";

            code = "en-US";
            return "English";
        }

        public ActionResult Login()
        {
            Ext.Net.Theme theme = Ext.Net.Theme.Default;

            if (Session["Ext.Net.Theme"] == null)
            {
                theme = Request.Cookies["Theme"] == null ?
                                            Ext.Net.Theme.Default :
                                            (Ext.Net.Theme)Thesis.Common.Helpers.Ax.ConvertIntValueWithDefault(Request.Cookies["Theme"].Value);

                Session["Ext.Net.Theme"] = theme;
            }
            else
            {
                theme = (Ext.Net.Theme)Session["Ext.Net.Theme"];
            }

            string code = string.Empty;
            if (Request.Cookies["Language"] == null)
            {
                code = "en-US";
                Request.Cookies.Add(new HttpCookie("Language", code));
            }
            else
                code = Request.Cookies["Language"].Value;

            ViewData["LanguageName"] = GetLanguageName(ref code);
            ViewData["LanguageValue"] = code;

            int thm = (int)theme;            

            ViewData["ThemeName"] = thm == 0 ? "Default" : (thm == 1 ? "Gray" : "Slate");
            ViewData["ThemeValue"] = thm;
            //ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult Login(FormCollection values)
        {
            if (ModelState.IsValid)
            {
                LoginModel model = new LoginModel()
                {
                    UserName = values["Username"],
                    Password = values["Password"],
                    Language = values["Language_Value"],
                    ReturnUrl = values["ReturnUrl"]
                };

                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, false);

                    HttpContext.Session["Culture"] = model.Language;

                    if (!String.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            
            return new AjaxResult { ErrorMessage = Resources.Login.Login_Error };
        }

        [HttpPost]
        public ActionResult ChangeTheme(int theme)
        {
            Session["Ext.Net.Theme"] = (Ext.Net.Theme)theme;
            return new AjaxResult();
        }

        [HttpPost]
        public ActionResult ChangeLanguage(string language)
        {
            CultureInfo culture = new CultureInfo(language, false);

            AjaxResult result = new AjaxResult();
            result.ExtraParamsResponse["Authentication"] = Resources.Login.ResourceManager.GetString("Authentication", culture);
            result.ExtraParamsResponse["Button_Login"] = Resources.Login.ResourceManager.GetString("Button_Login", culture);
            result.ExtraParamsResponse["Login_Error"] = Resources.Login.ResourceManager.GetString("Login_Error", culture);
            result.ExtraParamsResponse["Password"] = Resources.Login.ResourceManager.GetString("Password", culture);
            result.ExtraParamsResponse["Title_Login"] = Resources.Login.ResourceManager.GetString("Title_Login", culture);
            result.ExtraParamsResponse["Title_Login_Error"] = Resources.Login.ResourceManager.GetString("Title_Login_Error", culture);
            result.ExtraParamsResponse["Username"] = Resources.Login.ResourceManager.GetString("Username", culture);
            result.ExtraParamsResponse["Verifying"] = Resources.Login.ResourceManager.GetString("Verifying", culture);
            result.ExtraParamsResponse["Language"] = Resources.Login.ResourceManager.GetString("Language", culture);
            result.ExtraParamsResponse["Theme"] = Resources.Login.ResourceManager.GetString("Theme", culture);

            return result;
        }

        public ActionResult Logout()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
