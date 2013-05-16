using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ext.Net.MVC;

namespace Thesis.Lib.ActionResults
{
    public class DeleteResult: ActionResult
    {
        bool success;
        int count;

        public DeleteResult(bool success, int count)
        {
            this.success = success;
            this.count = count;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            AjaxFormResult response = new AjaxFormResult();
            if (!success)
            {
                response.Success = false;
                response.ExtraParams["isSingle"] = count > 1 ? "0" : "1";
                response.ExtraParams["msg"] = count > 1 ? "Some records did not deleted!" : "This record did not deleted!";
            }
            response.ExecuteResult(context);
        }
    }
}
