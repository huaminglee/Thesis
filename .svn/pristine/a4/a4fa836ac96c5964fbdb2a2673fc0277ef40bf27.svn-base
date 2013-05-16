using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Thesis.Common.Models;

namespace Thesis.Lib.Extensions
{
    public static class ModelStateHelpers
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
        {
            int i = 0;
            ModelState mState = null;
            foreach (RuleViolation issue in errors)
            {                
                if (!string.IsNullOrEmpty(issue.PropertyName))
                {
                    mState = modelState[issue.PropertyName];
                    if(mState != null)
                        mState.Errors.Insert(0, new ModelError(issue.ErrorMessage));
                    else
                        modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
                }
                else
                {
                    modelState.AddModelError("__notificationError__" + ++i, issue.ErrorMessage);
                }
            }
        }
    }
}
