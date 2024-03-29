﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Globalization;

namespace Thesis.Lib.ValueProviders
{
    public class ThesisValueProvider : IValueProvider
    {
        ControllerContext controllerContext;
        IValueProvider valueProvider;
        NameValueCollection values;

        public ThesisValueProvider(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
            valueProvider = controllerContext.Controller.ValueProvider;
            values = controllerContext.HttpContext.Request.Form;
        }

        #region IValueProvider Members

        public bool ContainsPrefix(string prefix)
        {
            return valueProvider.ContainsPrefix(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            var value = values[string.Concat(key, "_Value")];
            ValueProviderResult result = valueProvider.GetValue(key);
            if(value == null)
            {
                if (result == null) return null;
                return new ValueProviderResult(result.RawValue, result.AttemptedValue, CultureInfo.CurrentCulture);
            }

            if (result == null || !string.IsNullOrEmpty(result.AttemptedValue))
                return new ValueProviderResult(value, value, CultureInfo.CurrentCulture);

            return null;
        }

        #endregion
    }
}
