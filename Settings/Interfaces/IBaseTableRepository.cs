﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Thesis.Common.ViewModels;

namespace Settings
{
    public interface IBaseTableRepository
    {
        IEnumerable GetCities(int countryID, ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetCountries(ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetElectriciteites(ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetRelations(ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetCompanies(ComboBoxViewModel viewModel, out int totalCount);
        IEnumerable GetShifts(ComboBoxViewModel viewModel, out int totalCount);
    }
}
