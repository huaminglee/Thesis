﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;
using Thesis.Common.ViewModels;

namespace Thesis.Lib.ModelBinders
{
    public class ListItemModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string modelName = bindingContext.ModelName;
            NameValueCollection col = controllerContext.HttpContext.Request.Form;

            // Selected Item'ın index değeri de string.Concat(modelName, "_SelIndex");

            return new SelectItemViewModel<string>() {
                        Text = col[modelName],
                        Value = col[string.Concat(modelName, "_Value")]
                    };
        }
    }
}
