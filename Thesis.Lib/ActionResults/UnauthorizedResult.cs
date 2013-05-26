using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ext.Net.MVC;

namespace Thesis.Lib.ActionResults
{
    public class UnauthorizedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            AjaxFormResult response = new AjaxFormResult();
            response.Success = false;
            response.ExtraParams["hasPermission"] = "0";
            response.ExtraParams["msg"] = "You are not authorized to perform this action";
            response.ExecuteResult(context);
        }
    }
}
