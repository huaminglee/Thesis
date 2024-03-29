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

namespace Activity
{
    public class ActivityRepository : BaseRepository<ThesisObjectContext>, IActivityRepository
    {
        #region Compiled Queries

        static readonly Func<ThesisObjectContext, int, Thesis.Entities.Activity> cqGetById = CompiledQuery.Compile<ThesisObjectContext, int, Thesis.Entities.Activity>(
            (ctx, activityId) => ctx.Activity.Where(ti => ti.ActivityID == activityId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, ActivityViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, int, ActivityViewModel>(
                (ctx, activityId) => ctx.Activity.Where(ti => ti.ActivityID == activityId).Select(p => new ActivityViewModel
                {
                    ActivityID = p.ActivityID,
                    AddressID = p.Addresses.AddressID,
                    Name = p.Name,
                    Description = p.Description,
                    IsCompleted = p.IsCompleted,
                    IsInvoiced = p.IsInvoiced,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    OwnerID = p.OwnerID,
                    ExecuterID = p.ExecuterID,
                    DocumentID = p.DocumentID,
                    Value = p.Value,
                    TypeID = p.TypeID,
                    RelationID = p.RelationID,
                    RemainderDate = p.RemainderDate,  
                    PlannedHours = p.PlannedHours,
                    ShiftID = p.ShiftID,
                    InvoiceText = p.InvoiceText,
                    InvoiceAddressID = p.InvoiceAddressID,

                    TypeName = p.ActivityTypes.Name,
                    ExecuterName = p.Companies.Name,
                    OwnerName = p.Companies.Name,
                    Address = p.Addresses.Street,
                    ShiftNumber = p.Shifts.Name,
                    Relation = p.Relations.Name,
                    InvoiceAddress = p.Addresses1.Street
                }).FirstOrDefault());

        #endregion

        #region IRepository<ActivityViewModel> Members

        public ActivityViewModel LoadViewModel(int id)
        {
            return cqLoadViewModel.Invoke(context, id);
        }

        public bool Save(int id, ActivityViewModel viewModel, List<RuleViolation> validationResults)
        {
            return SaveViewModel(id, cqGetById, viewModel, validationResults);
        }

        public bool Delete(List<int> models)
        {
            return DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            var query = context.Activity
                        .Select(a => new
                        {
                            ActivityID = a.ActivityID,
                            TypeName = a.ActivityTypes.Name,
                            Name = a.Name,
                            Value = a.Value,
                            StartDate = a.StartDate,
                            EndDate = a.EndDate,
                            OwnerName = a.Companies1.Name,
                            ExecuterName = a.Companies.Name,
                            IsCompleted = a.IsCompleted,
                            IsInvoiced = a.IsInvoiced,
                            AddressID = a.Addresses.AddressID
                        });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion
    }
}
