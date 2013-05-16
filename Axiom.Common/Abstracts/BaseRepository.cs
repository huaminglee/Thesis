using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Thesis.Common.Extensions;
using Thesis.Common.Helpers;
using Thesis.Common.Interfaces;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;

namespace Thesis.Common.Abstracts
{
    public abstract class BaseRepository<TObjectContext> : IDisposable where TObjectContext : ObjectContext, new()
    {
        public TObjectContext context;

        public BaseRepository()
        {
            this.context = new TObjectContext();
        }

        #region Methods

        public void UpdateModel<TEntity, TViewModel>(TEntity entity, TViewModel viewModel)
            where TEntity : EntityObject, new()
            where TViewModel : class, new()
        {
            UpdateModel(entity, viewModel, null);
        }

        public void UpdateModel<TEntity, TViewModel>(TEntity entity, TViewModel viewModel, List<RuleViolation> validationResults)
            where TEntity : EntityObject, new()
            where TViewModel : class, new()
        {
            bool hasValidationResult = validationResults != null;

            var entityType = entity.GetType();
            var attrs = entityType.GetCustomAttributes(typeof(MetadataTypeAttribute), false).Select(p => ((MetadataTypeAttribute)p).MetadataClassType).ToList();
            if (attrs != null)
                attrs.ForEach(p => TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(entityType, p), entityType));

            var viewModelType = viewModel.GetType();

            var properties = viewModelType.GetProperties();

            List<string> primaryKeys = null;
            if (entity.EntityKey == null || entity.EntityKey.EntityKeyValues == null)
            {
                primaryKeys = entityType.GetProperties()
                                .Where(p => p.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), false).Any(t => ((EdmScalarPropertyAttribute)t).EntityKeyProperty))
                                .Select(p => p.Name).ToList();
            }
            else
            {
                primaryKeys = entity.EntityKey.EntityKeyValues.Select(p => p.Key).ToList();
            }

            string propertyName;
            PropertyInfo propertyInfo;
            object propertyValue;
            var results = new List<ValidationResult>();
            bool hasPrimaryKeys = primaryKeys != null;
            foreach (var property in properties)
            {                
                propertyName = property.Name;
                if (!hasPrimaryKeys || primaryKeys.IndexOf(propertyName) == -1)
                {
                    propertyInfo = entityType.GetProperty(propertyName);
                    if (propertyInfo == null)
                        continue;
                    propertyValue = property.GetValue(viewModel, null);

                    if (Validator.TryValidateProperty(propertyValue, new ValidationContext(entity, null, null) { MemberName = propertyName }, results))
                    {
                        try 
                        {
                            propertyInfo.SetValue(entity, propertyValue, null);
                        }
                        catch (Exception ex)
                        {
                            validationResults.Add(new RuleViolation(Ax.GetExceptionDetail(ex), propertyName));
                            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        }
                    }
                }
            }

            if (hasValidationResult)
            {
                foreach (var result in results)
                    foreach (var memberName in result.MemberNames)
                        validationResults.Add(new RuleViolation(result.ErrorMessage, memberName));

                var entityBusinessRules = entity as IBusinessRules;
                if (entityBusinessRules != null)
                    entityBusinessRules.Validate(validationResults);
            }
        }

        public bool SaveViewModel<TEntity, TViewModel>(int id, Func<TObjectContext, int, TEntity> cqGetById, TViewModel viewModel, List<RuleViolation> validationResults) 
            where TEntity : EntityObject, new()
            where TViewModel : class, new()
        {
            return SaveViewModel(id == 0, id, cqGetById, viewModel, validationResults);
        }

        public bool SaveViewModel<TId, TEntity, TViewModel>(bool isNew, TId id, Func<TObjectContext, TId, TEntity> cqGetById, TViewModel viewModel, List<RuleViolation> validationResults) 
            where TEntity : EntityObject, new()
            where TViewModel : class, new()
        {
            TEntity entity = isNew ? new TEntity() : cqGetById.Invoke(context, id);
            if (entity == null)
                return false;

            UpdateModel(entity, viewModel, validationResults);

            bool isSuccess = validationResults.Count == 0 ? SaveEntity(entity, validationResults) : false;

            if (isSuccess)
            {
                if (entity.EntityKey != null)
                {
                    foreach (var item in entity.EntityKey.EntityKeyValues)
                        Ax.SetValue(viewModel, item.Key, item.Value);
                }
            }

            return isSuccess;
        }

        public bool SaveEntity<TEntity>(TEntity model) where TEntity : EntityObject
        {
            return SaveEntity(model, null);
        }

        public bool SaveEntity<TEntity>(TEntity model, List<RuleViolation> validationResults) where TEntity : EntityObject
        {
            if (model.EntityState == System.Data.EntityState.Detached)
            {
                string entitySetName = context.GetEntitySetFullName(model);
                context.AddObject(entitySetName, model);
            }

            try { context.SaveChanges(); return true; }
            catch (Exception ex) {
                if (validationResults != null)
                    validationResults.Add(new RuleViolation(Ax.GetExceptionDetail(ex), "EntityError"));
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return false;
        }

        public bool DeleteEntity<TEntity>(TEntity entity) where TEntity : EntityObject
        {
            try
            {
                context.DeleteObject(entity);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEntities<TEntity, T>(List<T> models, Func<TObjectContext, T, TEntity> cqGetById) where TEntity : EntityObject
        {
            bool isSuccess = true;

            foreach (T id in models)
            {
                using (TObjectContext ctx = new TObjectContext())
                {
                    var model = cqGetById.Invoke(ctx, id);
                    if (model != null)
                    {
                        try
                        {
                            ctx.DeleteObject(model);
                            ctx.SaveChanges();
                        }
                        catch
                        {
                            if (isSuccess)
                                isSuccess = false;
                        }
                    }
                }
            }

            return isSuccess;
        }                

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        #endregion
    }
}
