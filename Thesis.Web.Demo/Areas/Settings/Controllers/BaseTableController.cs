﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Thesis.Common.ViewModels;
using Thesis.Lib.Controllers;
using Ext.Net.MVC;
using Settings;

namespace Thesis.Areas.Settings.Controllers
{
    public class BaseTableController : ThesisBaseController<IBaseTableRepository>
    {
        public BaseTableController(IBaseTableRepository repository): base(repository)
        {

        }

        public AjaxStoreResult GetCities(ComboBoxViewModel viewModel, int countryID = 0)
        {
            if (countryID <= 0)
                return null;

            int totalCount;
            var data = repository.GetCities(countryID, viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        public AjaxStoreResult GetCountries(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetCountries(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        public AjaxStoreResult GetElectriciteites(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetElectriciteites(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        public AjaxStoreResult GetRelations(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetRelations(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        public AjaxStoreResult GetCompanies(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetCompanies(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }

        public AjaxStoreResult GetShifts(ComboBoxViewModel viewModel)
        {
            int totalCount;
            var data = repository.GetShifts(viewModel, out totalCount);
            return new AjaxStoreResult(data, totalCount);
        }
    }
}
