﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ext.Net.MVC;

namespace Thesis.Lib.ActionResults
{
    public class ErrorResult : ActionResult
    {
        ModelStateDictionary ModelState;
        bool isUpload;

        public ErrorResult(ModelStateDictionary modelState, bool isUpload)
        {
            this.ModelState = modelState;
            this.isUpload = isUpload;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            AjaxFormResult response = new AjaxFormResult();            
            response.Success = false;
            response.IsUpload = isUpload;

            var errors = ModelState.Where(e => e.Value.Errors.Any()).Select(e => new { Key = e.Key, Message = e.Value.Errors[0].ErrorMessage }).ToList();

            string errorMessage = string.Empty;

            foreach (var error in errors)
            {
                if (error.Key.StartsWith("__notificationError__"))
                    errorMessage += string.Format("-{0}<br>", error.Message);
                else
                    response.Errors.Add(new FieldError(error.Key, error.Message));
            }

            response.ExtraParams["msg"] = string.IsNullOrEmpty(errorMessage) ? "Please check fields!" : string.Format("<div style=\"width: 180px; height: 64px; text-align: left; padding-top: 4px; padding-right: 3px; padding-bottom: 3px; padding-left: 8px;\">{0}</div>", errorMessage);

            response.ExecuteResult(context);
        }
    }
}
