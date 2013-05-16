using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.Common.Extensions;
using Thesis.Common.ViewModels;
using Thesis.Lib.Attributes;
using Thesis.Lib.Controllers;
using Crm;

namespace Thesis.Areas.Crm.Controllers
{
    public class RelationInAddressController : BaseScenarios<RelationInAddressesViewModel, IRelationInAddressRepository>
    {
        public RelationInAddressController(IRelationInAddressRepository repository)
            : base(repository, "RelationID", "RelationInAddressesID", true)
        {

        }

        [View]
        public override ActionResult GetByFilter(GridPanelViewModel model)
        {
            var result = ValueProvider.GetValue("addressId");
            if(result != null)
                model.AddFilterBase("AddressID", result.AttemptedValue);
            return base.GetByFilter(model);
        }

    }
}
