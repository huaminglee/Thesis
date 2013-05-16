using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;
using Thesis.Common.ViewModels;
using Thesis.Entities;

namespace Crm
{
    public interface IAddressRepository : IRepository<AddressViewModel>
    {
        IEnumerable GetAddressListItem(ComboBoxViewModel viewModel, out int totalCount);
    }
}
