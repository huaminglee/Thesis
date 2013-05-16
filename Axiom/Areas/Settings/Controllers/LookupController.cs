using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.Lib.Controllers;
using Settings;
using Thesis.Lib.Attributes;
using Thesis.Lib.ActionResults;
using Thesis.Common.ViewModels;
using Thesis.Common.Models;
using Thesis.Common.Helpers;
using Ext.Net.MVC;
using Thesis.Lib.Extensions;

namespace Thesis.Areas.Settings.Controllers
{
    public class LookupController : BaseScenarios<LookupViewModel, ILookupRepository>
    {
        public LookupController(ILookupRepository repository)
            : base(repository, "Text", "LookupId")
        { }

        public AjaxStoreResult GetLookups(int lookupTypeId, ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetLookups(lookupTypeId, viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        [View]
        [HttpGet]
        [CompressFilter]
        public override ActionResult Detail(int id)
        {
            var model = id == 0 ?
                        new LookupViewModel() { LookupResources = new List<LookupResourcesViewModel>() } :
                        repository.LoadViewModel(id);

            return View(model ?? new LookupViewModel() { LookupResources = new List<LookupResourcesViewModel>() });
        }

        [View]
        [HttpPost]
        public override ActionResult GetByFilter(GridPanelViewModel model)
        {
            int lookupTypeId = base.GetIntValueFromForm("lookupTypeId");
            if (lookupTypeId > 0)
            {
                int totalCount;
                var data = repository.GetByFilter(lookupTypeId, model, out totalCount);
                return new GridPanelResult(data, totalCount, model);
            }

            return new AjaxFormResult() { Success = false, IsUpload = false };
        }

        [Save]
        [HttpPost]
        public override ActionResult Save(int id)
        {
            var viewModel = new LookupViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            if (ModelState.IsValid)
            {
                var validationResults = new List<RuleViolation>();

                var formCollection = Request.Form;                   

                viewModel.LookupResources = new List<LookupResourcesViewModel>();
                viewModel.LookupResources.Add(new LookupResourcesViewModel() { LanguageCode = "en-US", Text = formCollection["Text_en_us"] });
                viewModel.LookupResources.Add(new LookupResourcesViewModel() { LanguageCode = "nl-NL", Text = formCollection["Text_nl_nl"] });

                bool isValid = repository.Save(id, viewModel, validationResults);
                if (isValid && validationResults.Count == 0)
                {                    
                    var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                    AjaxFormResult response = new AjaxFormResult();
                    response.Success = true;
                    response.IsUpload = false;

                    if (id.Equals(0))
                    {
                        response.ExtraParams["newID"] = viewModel.LookupId.ToString();
                    }

                    response.ExtraParams["title"] = formCollection[string.Format("Text_{0}", culture.Name.ToLower().Replace("-", "_"))] ?? string.Empty;
                    response.ExtraParams["msg"] = "Save Success";
                    return response;
                }
                else
                    ModelState.AddModelErrors(validationResults);
            }

            return new ErrorResult(ModelState, false);
        }
    }
}
