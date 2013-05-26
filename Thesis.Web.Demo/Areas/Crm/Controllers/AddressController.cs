using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thesis.Common.ViewModels;
using Thesis.Lib.Controllers;
using Crm;
using Ext.Net.MVC;
using Thesis.Lib.Attributes;

namespace Thesis.Areas.Crm.Controllers
{
    [ModuleAuthorize]
    public class AddressController : BaseScenarios<AddressViewModel, IAddressRepository>
    {
        public AddressController(IAddressRepository repository) : base(repository, "Street", "AddressID")
        {
        }

        public AjaxStoreResult GetAddressListItem(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetAddressListItem(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }
    }
}
