﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace Thesis.Lib.ModelBinders
{
    public class DeleteItemModelBinderForGuid : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string modelName = bindingContext.ModelName;
            NameValueCollection col = controllerContext.HttpContext.Request.Form;

            return (List<Guid>)Ext.Net.JSON.Deserialize(col[modelName], typeof(List<Guid>));
        }
    }
}
