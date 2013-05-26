using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using Thesis.Lib.ActionResults;
using Thesis.Lib.Controllers;
using Thesis.Lib.Extensions;
using Ext.Net.MVC;
using Settings;
using Thesis.Authorization.ViewModels;
using Thesis.Common.Helpers;
using Thesis.Authorization.Services;
using Thesis.Authorization.Enumerations;
using Thesis.Lib.Attributes;

namespace Thesis.Areas.Settings.Controllers
{
    [ModuleAuthorize]
    public class RoleManagementController : BaseScenarios<RoleManagementViewModel, IRoleManagementRepository, Guid>
    {
        public RoleManagementController(IRoleManagementRepository repository)
            : base(repository, "RoleName", "RoleId", Guid.Empty)
        { }

        [View]
        [HttpGet]
        public override ActionResult Detail(Guid? id)
        {
            return base.Detail(id);
        }

        [View]
        [HttpPost]
        public override ActionResult Save(Guid? ID)
        {
            if (!ID.HasValue) ID = Guid.Empty;

            var viewModel = new RoleManagementViewModel();

            TryUpdateModel(viewModel, ControllerContext, ModelState);

            if (ModelState.IsValid)
            {
                var modules = ModuleAuthorizeService.GetModules();

                if (modules != null && modules.Count > 0)
                {
                    if (viewModel.ModulesInRolesViewModel == null)
                        viewModel.ModulesInRolesViewModel = new List<ModulesInRolesViewModel>();

                    var values = ControllerContext.HttpContext.Request.Form;

                    string value;
                    string[] mod;
                    int moduleId = 0;
                    for (int i = 0; i < values.Count; i++)
                    {
                        value = values[i];
                        if (value.IndexOf("_") == -1)
                            continue;

                        mod = value.Split('_');
                        moduleId = modules.Where(p => p.Name.ToLower() == mod[0].ToLower()).Select(p => p.ModuleId).FirstOrDefault();
                        if (moduleId > 0)
                        {
                            viewModel.ModulesInRolesViewModel.Add(
                                new ModulesInRolesViewModel()
                                {
                                    ModuleId = moduleId,
                                    ProcessType = (ProcessTypes)Enum.Parse(typeof(ProcessTypes), mod[1])
                                });
                        }
                    }
                }
                
                var validationResults = new List<RuleViolation>();
                bool isNew = ID.Equals(Guid.Empty);
                bool isValid = repository.Save(isNew, ID.Value, viewModel, validationResults);
                if (isValid && validationResults.Count == 0)
                    return new SaveResult(isNew, viewModel.RoleName, viewModel.RoleId, false);
                else
                    ModelState.AddModelErrors(validationResults);
            }

            return new ErrorResult(ModelState, false);
        }

        public AjaxStoreResult GetRoles(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetRoles(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        [View]
        [HttpPost]
        public virtual ActionResult GetRolesByUserId(GridPanelViewModel model, Guid userId)
        {
            int totalCount;
            var data = repository.GetRolesByUserId(userId, model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }
    }
}
