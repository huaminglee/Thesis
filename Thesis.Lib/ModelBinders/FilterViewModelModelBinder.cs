using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;
using Thesis.Common.ViewModels;

namespace Thesis.Lib.ModelBinders
{
    public class FilterViewModelModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection col = controllerContext.HttpContext.Request.Form;
            string[] values = col.GetValues(bindingContext.ModelName);
            if (values != null && values.Length > 0)
                return (List<FilterViewModel>)Ext.Net.JSON.Deserialize(string.Format("[{0}]", values[0]), typeof(List<FilterViewModel>));
            return null;
        }
    }
}
