﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using Thesis.Entities;

namespace Crm
{
    public class AddressRepository : BaseRepository<ThesisObjectContext>, IAddressRepository
    {
        #region Compiled Queries

        static readonly Func<ThesisObjectContext, int, Addresses> cqGetById = CompiledQuery.Compile<ThesisObjectContext, int, Addresses>(
            (ctx, addressID) => ctx.Addresses.Where(ti => ti.AddressID == addressID).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, string, AddressViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, int, string, AddressViewModel>(
            (ctx, addressID, languageCode) => ctx.Addresses.Where(ti => ti.AddressID == addressID).Select(p => new AddressViewModel
            {
                AddressID = p.AddressID,
                IsActive = p.IsActive,
                AddressType = p.AddressType,
                Street = p.Street,
                HouseNumber = p.HouseNumber,
                Addition = p.Addition,
                PostalCode = p.PostalCode,
                CityID = p.CityID,
                CountryID = p.CountryID,
                Phone = p.Phone,
                Fax = p.Fax,
                Description = p.Description,
                KeyPersonName = p.KeyPersonName,
                KeyPersonPhone = p.KeyPersonPhone,
                ObjectType = p.ObjectType,
                LastInvoiceDate = p.LastInvoiceDate,
                Electriciteit = p.Electriciteit,
                DetailDescription = p.DetailDescription,
                Location = p.Location,
                Information = p.Information,
                ExtraLetterText = p.ExtraLetterText,

                AddressTypeName = p.Lookup.LookupResources.Where(l => l.Languages.Code == languageCode).Select(l => l.Text).FirstOrDefault(),//p.AddressTypes.Name,
                CityName = p.Cities.Name,
                CountryName = p.Countries.Name,
                ObjectTypeName = p.ObjectTypes.Name,
                ElectriciteitName = p.Electriciteities.Name,
                PlannedManHours = p.Activity.Where(ti => ti.ActivityTypes.ActivityTypeID.Equals(16)).Sum(ti => (int?)ti.PlannedHours),
                //TotalSurfaceM2 = p.Roofs.Sum(ti => (decimal?)ti.SurfaceAreaM2),
                ContractValue = p.Activity.Where(ti => ti.ActivityTypes.ActivityTypeID.Equals(16)).Sum(ti => (decimal?)ti.Value)
                //InvoicedWorkAddress = ctx.OutgoingInvoiceLines.Where(ti => ti.OutgoingInvoices.Activities.Addresses.AddressID.Equals(addressID) && ti.OutgoingInvoices.InvoiceStatusID.Equals(2)).Sum(ti => (decimal?)ti.LinePrice),
                //ToBeInvoiced = ctx.OutgoingInvoiceLines.Where(ti => ti.OutgoingInvoices.Activities.Addresses.AddressID.Equals(addressID) && ti.OutgoingInvoices.InvoiceStatusID.Equals(1)).Sum(ti => (decimal?)ti.LinePrice)
            }).FirstOrDefault());

        #endregion

        #region IRepository<AddressViewModel> Members

        public AddressViewModel LoadViewModel(int id)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cqLoadViewModel.Invoke(context, id, culture.Name);
        }

        public bool Save(int id, AddressViewModel model, List<RuleViolation> errors)
        {
            return SaveViewModel(id, cqGetById, model, errors);
        }

        public bool Delete(List<int> models)
        {
            return DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            string languageCode = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            var query = context.Addresses
                        .Select(a => new {
                            AddressID = a.AddressID,
                            AddressType = a.Lookup.LookupResources.Where(l => l.Languages.Code == languageCode).Select(l => l.Text).FirstOrDefault(),
                            Street = a.Street,
                            HouseNumber = a.HouseNumber,
                            Addition = a.Addition,
                            Postalcode = a.PostalCode,
                            City = a.Cities.Name,
                            Relation = context.RelationInAddresses.Where(r => r.Addresses.AddressID.Equals(a.AddressID)).FirstOrDefault().Relations.Name,
                            Phone = a.Phone
                        });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion

        #region IAddressRepository Members

        public IEnumerable GetAddressListItem(ComboBoxViewModel viewModel, out int totalCount)
        {
            var query = context.Addresses
                    .Select(a => new 
                    {
                        AddressID = a.AddressID,
                        Street = a.Street ?? string.Empty,
                        HouseNumber = a.HouseNumber ?? string.Empty,
                        City = a.Cities.Name ?? string.Empty,
                        Country = a.Countries.Name ?? string.Empty                        
                    });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion
    }
}
