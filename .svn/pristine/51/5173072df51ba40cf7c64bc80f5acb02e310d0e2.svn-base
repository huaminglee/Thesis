using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Entities;
using System.Collections;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using System.Data.Objects;
using Thesis.Common.Helpers;

namespace Settings
{
    public class LookupRepository : BaseRepository<ThesisObjectContext>, ILookupRepository
    {

        #region Compiled Queries

        static readonly Func<ThesisObjectContext, int, Lookup> cqGetById = CompiledQuery.Compile<ThesisObjectContext, int, Lookup>(
            (ctx, lookupId) => ctx.Lookup.Where(ti => ti.LookupId == lookupId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, Lookup> cqGetLookupWithResources = CompiledQuery.Compile<ThesisObjectContext, int, Lookup>(
           (ctx, lookupId) => ctx.Lookup.Include("LookupResources").Where(ti => ti.LookupId == lookupId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, IQueryable<Languages>> cqGetAllLanguages = CompiledQuery.Compile<ThesisObjectContext, IQueryable<Languages>>(
           (ctx) => ctx.Languages);

        static readonly Func<ThesisObjectContext, int, LookupViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, int, LookupViewModel>(
                (ctx, lookupId) => ctx.Lookup.Where(ti => ti.LookupId == lookupId).Select(p => new LookupViewModel
                {
                    LookupId = p.LookupId,
                    LookupTypeId = p.LookupTypeId                    
                }).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, IQueryable<LookupResourcesViewModel>> cqGetLookupResourcesViewModel = CompiledQuery.Compile<ThesisObjectContext, int, IQueryable<LookupResourcesViewModel>>(
            (ctx, lookupId) => ctx.LookupResources.Where(ti => ti.LookupId == lookupId).Select(p => new LookupResourcesViewModel 
            { 
                  LanguageCode = p.Languages.Code,
                  Text = p.Text
            }));

        #endregion

        #region ILookupRepository Members

        public IEnumerable GetLookups(int lookupTypeId, ComboBoxViewModel viewModel, out int totalCount)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            var query = context.LookupResources.Where(p => p.Lookup.LookupTypeId == lookupTypeId && p.Languages.Code == culture.Name)
                .Select(p => new SelectItemViewModel
                {
                    Value = p.LookupId,
                    Text = p.Text
                });

            return query.ToList(viewModel, out totalCount);
        }

        public IEnumerable GetByFilter(int lookupTypeId, IDataViewModel viewModel, out int totalCount)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            var query = context.LookupResources.Where(p => p.Lookup.LookupTypeId == lookupTypeId && p.Languages.Code == culture.Name)
                .Select(p => new
                {
                    LookupId = p.LookupId,
                    Text = p.Text
                });

            return query.ToList(viewModel, out totalCount);
        }

        #endregion

        #region IRepository<LookupViewModel> Members

        public LookupViewModel LoadViewModel(int id)
        {
            var viewModel = cqLoadViewModel.Invoke(context, id);
            if (viewModel != null)
                viewModel.LookupResources = cqGetLookupResourcesViewModel(context, id).ToList();
            return viewModel;
        }

        public bool Save(int id, LookupViewModel viewModel, List<RuleViolation> validationResults)
        {
            bool isNew = id == 0;
            Lookup lookup = isNew ? new Lookup() { LookupTypeId = viewModel.LookupTypeId } : cqGetLookupWithResources(context, id);
            var languages = cqGetAllLanguages.Invoke(context).ToList();
            if (isNew)
            {
                foreach (var res in viewModel.LookupResources)
                {
                    lookup.LookupResources.Add(new LookupResources()
                    {
                        LanguageId = languages.Where(p => p.Code == res.LanguageCode).Select(p => p.LanguageId).FirstOrDefault(),
                        Text = res.Text
                    });
                }
            }
            else
            {
                foreach (var lang in languages)
                {
                    var resource = lookup.LookupResources.Where(p => p.Languages == lang).FirstOrDefault();
                    var text = viewModel.LookupResources.Where(p => p.LanguageCode == lang.Code).Select(p => p.Text).FirstOrDefault();
                    if (resource == null)
                    {
                        lookup.LookupResources.Add(new LookupResources()
                        {
                            LanguageId = lang.LanguageId,
                            Text = text
                        });
                    }
                    else
                        resource.Text = text;
                }
            }

            try
            {
                if (isNew)
                    context.AddToLookup(lookup);
                context.SaveChanges();
                viewModel.LookupId = lookup.LookupId;
                return true;
            }
            catch (Exception ex)
            {
                validationResults.Add(new RuleViolation(Ax.GetExceptionDetail(ex), "EntityError"));
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return false;
        }

        public bool Delete(List<int> models)
        {
            return DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
