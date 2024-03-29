﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Entities;
using System.Collections;
using Thesis.Common.ViewModels;
using System.Data.Objects;
using Thesis.Authorization.ViewModels;
using Thesis.Authorization.Enumerations;
using Thesis.Caching.Interfaces;
using Thesis.Caching.Providers;
using Thesis.Common.Models;
using System.Web.Security;

namespace Settings
{
    public class RoleManagementRepository : BaseRepository<ThesisObjectContext>, IRoleManagementRepository
    {
        private ICacheProvider Cache { get; set; }

        #region Contructors

        public RoleManagementRepository() : this(new DefaultCacheProvider())
        {

        }

        public RoleManagementRepository(ICacheProvider cacheProvider)
        {
            Cache = cacheProvider;                
        }

        #endregion

        #region Compiled Queries

        static readonly Func<ThesisObjectContext, Guid, aspnet_Roles> cqGetById = CompiledQuery.Compile<ThesisObjectContext, Guid, aspnet_Roles>(
            (ctx, roleId) => ctx.aspnet_Roles.Where(ti => ti.RoleId == roleId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, Guid, RoleManagementViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, Guid, RoleManagementViewModel>(
                (ctx, roleId) => ctx.aspnet_Roles.Where(ti => ti.RoleId == roleId).Select(p => new RoleManagementViewModel
                {
                    RoleId = p.RoleId,
                    RoleName = p.RoleName,
                    Description = p.Description                    
                }).FirstOrDefault());

        #endregion

        #region IRepository<RoleManagementViewModel,Guid> Members

        public RoleManagementViewModel LoadViewModel(Guid id)
        {
            var viewModel = cqLoadViewModel(context, id);
            if (viewModel != null)
            {
                bool hasSuperRole = Thesis.Common.Helpers.Ax.HasSuperRoles(id);
                if (hasSuperRole)                
                    viewModel.ModulesInRolesViewModel = Thesis.Authorization.Services.ModuleAuthorizeService.GetAllModulesPermissions();                
                else
                    viewModel.ModulesInRolesViewModel = context.ModulesInRoles.Where(m => m.RoleId == id).Select(m => new ModulesInRolesViewModel { ModuleId = m.ModuleId, RoleId = id, ProcessType = (ProcessTypes)m.ProcessTypeId }).ToList();
            }
            return viewModel;
        }

        public bool Save(bool isNew, Guid id, RoleManagementViewModel viewModel, List<RuleViolation> validationResults)
        {
            var provider = Roles.Provider;
            if (isNew && provider.RoleExists(viewModel.RoleName))
            {
                validationResults.Add(new RuleViolation("Role name already exists.", "RoleName"));
                return false;
            }

            if (isNew)
            {
                provider.CreateRole(viewModel.RoleName);
                var role = context.aspnet_Roles.Where(p => p.RoleName == viewModel.RoleName).FirstOrDefault();
                if (role == null)
                    return false;

                role.Description = viewModel.Description;
                context.SaveChanges();

                viewModel.RoleId = role.RoleId;

                UpdateRoleItems(viewModel, provider);
                ClearRoleCaches(viewModel.RoleName);
                return true;
            }
            else
            {
                bool isSuccess = SaveViewModel(isNew, id, cqGetById, viewModel, validationResults);
                if (isSuccess)
                {
                    UpdateRoleItems(viewModel, provider);
                    ClearRoleCaches(viewModel.RoleName);
                }
                return isSuccess;
            }
        }

        public bool Delete(List<Guid> models)
        {
            ClearRoleCaches(models);
            return DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            Guid[] roleIds = context.aspnet_Users.Where(p => p.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault()
                .aspnet_Roles.Select(p => p.RoleId).ToArray();

            bool hasSuperRole = Thesis.Common.Helpers.Ax.HasSuperRoles(roleIds);

            if (hasSuperRole)
            {
                var query = context.aspnet_Roles.OrderBy(p => p.RoleName).Select(p => new RoleManagementListViewModel
                {
                    RoleId = p.RoleId,
                    RoleName = p.RoleName,
                    Description = p.Description
                });

                return query.ToList(viewModel, out totalCount);
            }
            else
            {
                var query = context.RolesInTree.Where(p => roleIds.Contains(p.ParentRoleId)).OrderBy(p => p.aspnet_Roles.RoleName).Select(p => new RoleManagementListViewModel
                {
                    RoleId = p.RoleId,
                    RoleName = p.aspnet_Roles.RoleName,
                    Description = p.aspnet_Roles.Description
                });

                return query.ToList(viewModel, out totalCount);
            }
        }

        #endregion

        #region Methods

        private void UpdateRoleItems(RoleManagementViewModel viewModel, RoleProvider provider)
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var rolesForUser = provider.GetRolesForUser(userName);
            var roleIds = context.aspnet_Roles.Where(p => rolesForUser.Contains(p.RoleName)).Select(p => p.RoleId).ToList();

            foreach (var roleId in roleIds)
            {
                RolesInTree rolesInTree = new RolesInTree()
                {
                    RoleId = viewModel.RoleId,
                    ParentRoleId = roleId
                };

                context.AddToRolesInTree(rolesInTree);
            }

            foreach (var item in viewModel.ModulesInRolesViewModel)
            {
                ModulesInRoles modulesInRoles = new ModulesInRoles()
                {
                    ModuleId = item.ModuleId,
                    RoleId = viewModel.RoleId,
                    ProcessTypeId = (int)item.ProcessType
                };

                context.AddToModulesInRoles(modulesInRoles);
            }

            context.SaveChanges();
        }

        private void ClearRoleCaches(List<Guid> roleIds)
        {
            foreach (Guid roleId in roleIds)
                ClearRoleCaches(cqGetById(context, roleId).RoleName);          
        }

        private void ClearRoleCaches(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return;
            string[] users = Roles.Provider.GetUsersInRole(roleName);
            for (int i = 0; i < users.Length; i++)
                Cache.Remove(string.Format("moduleInRoles_{0}", users[i]));
        }
        
        #endregion

        #region IRoleManagementRepository Members

        public IEnumerable GetRoles(ComboBoxViewModel viewModel, out int totalCount)
        {
            Guid[] roleIds = context.aspnet_Users.Where(p => p.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault()
                .aspnet_Roles.Select(p => p.RoleId).ToArray();

            bool hasSuperRole = Thesis.Common.Helpers.Ax.HasSuperRoles(roleIds);

            if (hasSuperRole)
            {
                var query = context.aspnet_Roles.OrderBy(p => p.RoleName).Select(p => new SelectItemViewModel<Guid>
                    {
                        Value = p.RoleId,
                        Text = p.RoleName
                    });

                return query.ToList(viewModel, out totalCount);
            }
            else
            {
                var query = context.RolesInTree.Where(p => roleIds.Contains(p.ParentRoleId)).OrderBy(p => p.aspnet_Roles.RoleName).Select(p => new SelectItemViewModel<Guid>
                    {
                        Value = p.RoleId,
                        Text = p.aspnet_Roles.RoleName
                    });

                return query.ToList(viewModel, out totalCount);
            }
        }

        public IEnumerable GetRolesByUserId(Guid userId, IDataViewModel viewModel, out int totalCount)
        {            
            var query = context.aspnet_Roles.Where(p => p.aspnet_Users.Any(r => r.UserId == userId)).OrderBy(p => p.RoleName).Select(p => new
            { 
                RoleId = p.RoleId,
                RoleName = p.RoleName
            });
           
            return query.ToList(viewModel, out totalCount);
        }

        #endregion
    }
}
