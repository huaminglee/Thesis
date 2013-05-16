using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Thesis.Common.Interfaces;
using Thesis.Common.Models;

using Thesis.Lib.Attributes;
using Thesis.Lib.Extensions;
using Thesis.Lib.ValueProviders;

namespace Thesis.Lib.Controllers
{
    public class ThesisBaseController<T> : BaseController
    {
        protected T repository;
        public ThesisBaseController(T repository)
        {
            this.repository = repository;
        }

        public bool TryUpdateModel<TModel>(TModel model, ControllerContext controllerContext) where TModel : class
        {
            return base.TryUpdateModel(model, new ThesisValueProvider(controllerContext));
        }

        public bool TryUpdateModel<TModel>(TModel model, ControllerContext controllerContext, ModelStateDictionary modelState) where TModel : class
        {
            bool isUpdated = this.TryUpdateModel(model, controllerContext);

            this.ValidateBusinessModel(model, modelState);

            return isUpdated;
        }

        public void UpdateModel<TModel>(TModel model, ControllerContext controllerContext) where TModel : class
        { 
            base.UpdateModel(model, new ThesisValueProvider(controllerContext));
        }

        public void UpdateModel<TModel>(TModel model, ControllerContext controllerContext, ModelStateDictionary modelState) where TModel : class
        {
            this.UpdateModel(model, controllerContext);

            this.ValidateBusinessModel(model, modelState);
        }

        public void ValidateBusinessModel<TModel>(TModel model, ModelStateDictionary modelState) where TModel : class
        {
            IBusinessRules rules = model as IBusinessRules;

            if (rules != null)
            {
                var errors = new List<RuleViolation>();
                rules.Validate(errors);
                modelState.AddModelErrors(errors);
            }
        }        
    }
}
