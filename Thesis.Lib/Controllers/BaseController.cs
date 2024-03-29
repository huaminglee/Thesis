﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Thesis.Lib.Controllers
{
    [Authorize]
    public class BaseController : DefaultController
    {
        /// <summary>
        /// return int value from querystring (default -1)
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public int GetIntValueFromQuery(string key)
        {
            if (string.IsNullOrEmpty(key)) return -1;
            string value = Request.QueryString[key];
            int result = -1;
            if (!Thesis.Common.Helpers.Ax.TryParse(value, ref result)) return -1;
            return result;
        }

        /// <summary>
        /// return int value from value provider (default -1)
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public int GetIntValueFromValueProvider(string key)
        {
            if (string.IsNullOrEmpty(key)) return -1;
            ValueProviderResult valueResult = ValueProvider.GetValue(key);
            int result = -1;
            if (valueResult == null || !Thesis.Common.Helpers.Ax.TryParse(valueResult.AttemptedValue, ref result)) return -1;
            return result;
        }

        /// <summary>
        /// return int value from form (default -1)
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public int GetIntValueFromForm(string key)
        {
            if (string.IsNullOrEmpty(key)) return -1;
            string value = Request.Form[key];
            int result = -1;
            if (!Thesis.Common.Helpers.Ax.TryParse(value, ref result)) return -1;
            return result;
        }
    }
}
