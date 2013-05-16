using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;
using System.Collections;
using Thesis.Common.ViewModels;
using Thesis.Common.Models;

namespace Settings
{
    public interface ILookupRepository : IRepository<LookupViewModel>
    {
        IEnumerable GetByFilter(int lookupTypeId, IDataViewModel viewModel, out int totalCount);
        IEnumerable GetLookups(int lookupTypeId, ComboBoxViewModel viewModel, out int totalCount);
    }
}
