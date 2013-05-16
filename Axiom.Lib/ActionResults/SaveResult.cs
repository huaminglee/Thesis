using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ext.Net.MVC;
using Thesis.Common.Helpers;

namespace Thesis.Lib.ActionResults
{
    public class SaveResult : ActionResult
    {
        bool isNew, isUpload;
        string title, newID;

        public SaveResult(bool isNew, ValueProviderResult title, object newID, bool isUpload)
            : this(isNew, (title != null ? title.AttemptedValue : string.Empty), Ax.ConvertStringWithDefault(newID), isUpload)
        {
        }

        public SaveResult(bool isNew, object title, object newID, bool isUpload)
            : this(isNew, Ax.ConvertStringWithDefault(title), Ax.ConvertStringWithDefault(newID), isUpload)
        {
        }

        public SaveResult(bool isNew, string title, int newID, bool isUpload)
            : this(isNew, title, newID.ToString(), isUpload)
        { 
        }

        public SaveResult(bool isNew, string title, string newID, bool isUpload)
        {
            this.isNew = isNew;
            this.title = title;
            this.newID = newID;
            this.isUpload = isUpload;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            AjaxFormResult response = new AjaxFormResult();
            response.Success = true;
            response.IsUpload = isUpload;
            response.ExtraParams["newID"] = newID ?? string.Empty;

            response.ExtraParams["title"] = title ?? string.Empty;
            response.ExtraParams["msg"] = "Save Success";
            response.ExecuteResult(context);
        }
    }
}
