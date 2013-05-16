using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.Common.Extensions;
using Thesis.Common.ViewModels;
using Thesis.Lib.ActionResults;
using Thesis.Lib.Controllers;
using Crm;

namespace Thesis.Areas.Crm.Controllers
{
    public class RoofController : ThesisBaseController<IRoofRepository>
    {
        public RoofController(IRoofRepository repository): base(repository)
        {

        }

        [HttpPost]
        public ActionResult GetByFilter(GridPanelViewModel model, int addressID)
        {
            model.AddFilterBase("AddressID", addressID);

            int totalCount;
            var data = repository.GetByFilter(model, out totalCount);
            return new GridPanelResult(data, totalCount, model);
        }

    }
}
