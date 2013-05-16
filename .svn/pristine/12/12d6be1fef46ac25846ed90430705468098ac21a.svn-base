using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Thesis.Lib.Controllers;
using Settings;
using Thesis.Lib.Attributes;
using Thesis.Common.ViewModels;
using Thesis.Lib.ActionResults;

namespace Thesis.Areas.Settings.Controllers
{
    [ModuleAuthorize]
    public class UserManagementController : BaseScenarios<UserManagementViewModel, IUserManagementRepository, Guid>
    {
        public UserManagementController(IUserManagementRepository repository)
            : base(repository, "UserName", "UserId", Guid.Empty)
        { }

        [View]
        [HttpGet]
        public override ActionResult Detail(Guid? id)
        {
            return base.Detail(id);
        }

        [Save]
        [HttpPost]
        public override ActionResult Save(Guid? ID)
        {
            return base.Save(ID);
        }

        public ActionResult GetUsersByRoleId(GridPanelViewModel model, Guid roleId)
        {
            int totalCount;
            var data = repository.GetUsersByRoleId(roleId, model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }
    }
}
