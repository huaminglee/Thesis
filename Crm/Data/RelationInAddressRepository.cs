﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using Thesis.Entities;

namespace Crm
{
    public class RelationInAddressRepository : BaseRepository<ThesisObjectContext>, IRelationInAddressRepository
    {
        #region Compiled Queries

        static readonly Func<ThesisObjectContext, int, RelationInAddresses> cqGetById = CompiledQuery.Compile<ThesisObjectContext, int, RelationInAddresses>(
            (ctx, relationInAddressesID) => ctx.RelationInAddresses.Where(ti => ti.RelationInAddressesID == relationInAddressesID).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, RelationInAddressesViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, int, RelationInAddressesViewModel>(
            (ctx, relationInAddressesID) => ctx.RelationInAddresses.Where(ti => ti.RelationInAddressesID == relationInAddressesID).Select(p => new RelationInAddressesViewModel
            {
                RelationInAddressesID = p.RelationInAddressesID,
                RelationID = p.RelationID,
                RelationName = p.Relations.Name
            }).FirstOrDefault());

        #endregion

        #region IRepository<RelationInAddressesViewModel> Members

        public RelationInAddressesViewModel LoadViewModel(int id)
        {
            return cqLoadViewModel(context, id);
        }

        public bool Save(int id, RelationInAddressesViewModel viewModel, List<RuleViolation> validationResults)
        {
            return SaveViewModel(id, cqGetById, viewModel, validationResults);
        }

        public bool Delete(List<int> models)
        {
            return DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            var query = context.RelationInAddresses
                        .Select(p => new
                        {
                            RelationInAddressesID = p.RelationInAddressesID,
                            RelationName = p.Relations.Name,
                            AddressID = p.AddressID
                        });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion
    }
}
