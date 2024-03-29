﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Thesis.Entities;
using Thesis.Common.ViewModels;

namespace Settings
{
    public class BaseTableRepository: IBaseTableRepository
    {
        #region IBaseTableRepository Members

        public IEnumerable GetCities(int countryID, ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Cities.Where(a => a.CountryID == countryID)
                    .Select(a => new SelectItemViewModel {
                        Text = a.Name,
                        Value = a.CityID
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetCountries(ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Countries.Where(a => a.Name.Length > 0)
                    .Select(a => new SelectItemViewModel {
                        Text = a.Name,
                        Value = a.CountryID
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetElectriciteites(ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Electriciteities.Select(a => new SelectItemViewModel {
                        Text = a.Name,
                        Value = a.ElectriciteitID
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetRelations(ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Relations.Select(a => new SelectItemViewModel {
                        Text = a.Name,
                        Value = a.RelationID
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetCompanies(ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Companies.Select(a => new SelectItemViewModel {
                        Text = a.Name,
                        Value = a.CompanyID
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetShifts(ComboBoxViewModel viewModel, out int totalCount)
        {
            using (ThesisObjectContext context = new ThesisObjectContext())
            {
                var query = context.Relations.Select(a => new SelectItemViewModel
                {
                    Text = a.Name,
                    Value = a.RelationID
                });

                return query.ToList(viewModel, out totalCount);
            }
        }

        #endregion
    }
}
