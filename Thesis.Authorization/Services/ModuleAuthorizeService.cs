﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.ViewModels;
using Thesis.Caching.Interfaces;
using Thesis.Caching.Providers;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace Thesis.Authorization.Services
{
    public class ModuleAuthorizeService
    {
        private static ICacheProvider cache;
        public static ICacheProvider Cache {
            get { if (cache == null) cache = new DefaultCacheProvider(); return cache; }
            set { cache = value; }
        }

        private static List<Guid> GetRoleIdsByUserName(string userName)
        {
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("pGetRoleIdsByUserName", cnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", userName);
            try
            {
                cnn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    List<Guid> models = new List<Guid>();
                    while (rdr.Read())
                    {
                        models.Add(rdr.GetGuid(0));
                    }
                    rdr.Close();
                    cnn.Close();
                    return models;
                }
            }
            catch
            {                
            }

            return null;
        }

        public static List<ModulesInRolesViewModel> GetAllModulesPermissions()
        {
            List<ModulesInRolesViewModel> models = new List<ModulesInRolesViewModel>();
            List<ModuleViewModel> moduleViewModel = GetModules();
            if (moduleViewModel != null && moduleViewModel.Count > 0)
            {
                List<ProcessTypes> processTypes = new List<ProcessTypes>();
                processTypes.Add(ProcessTypes.View);
                processTypes.Add(ProcessTypes.Add);
                processTypes.Add(ProcessTypes.Update);
                processTypes.Add(ProcessTypes.Delete);

                foreach (var model in moduleViewModel)
                {
                    foreach (var roleId in Thesis.Common.Helpers.Ax.SuperRoles)
                    {
                        foreach (var processType in processTypes)
                        {
                            models.Add(new ModulesInRolesViewModel() { ModuleId = model.ModuleId, RoleId = roleId, ProcessType = processType });
                        }
                    }
                }
            }
            return models;  
        }

        public static List<ModulesInRolesViewModel> GetAllowedModulesInRolesByUser()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            object data = Cache.Get(string.Format("moduleInRoles_{0}", userName));
            if (data == null)
            {
                bool hasSuperRole = Thesis.Common.Helpers.Ax.HasSuperRoles(GetRoleIdsByUserName(userName));

                if (hasSuperRole)
                {
                    List<ModulesInRolesViewModel> models = GetAllModulesPermissions();
                    Cache.Set(string.Format("moduleInRoles_{0}", userName), models, int.MaxValue);
                    return models;                    
                }
                else
                {
                    SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("pGetModulesPermissionsByUserId", cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", Membership.GetUser(userName, false).ProviderUserKey);
                    try
                    {
                        cnn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            List<ModulesInRolesViewModel> models = new List<ModulesInRolesViewModel>();
                            ModulesInRolesViewModel model;
                            while (rdr.Read())
                            {
                                model = new ModulesInRolesViewModel();
                                model.ModuleId = rdr.GetInt32(0);
                                model.RoleId = rdr.GetGuid(1);
                                model.ProcessType = (ProcessTypes)rdr.GetInt32(2);
                                models.Add(model);
                            }
                            Cache.Set(string.Format("moduleInRoles_{0}", userName), models, int.MaxValue);
                            rdr.Close();
                            cnn.Close();
                            return models;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return data as List<ModulesInRolesViewModel>;
        }

        public static List<ModuleViewModel> GetModules()
        {
            object data = Cache.Get("modules");
            if (data == null)
            {
                SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                SqlCommand cmd = new SqlCommand("pGetAllModules", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                try
                {
                    cnn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        List<ModuleViewModel> models = new List<ModuleViewModel>();
                        ModuleViewModel model;
                        while (rdr.Read())
                        {
                            model = new ModuleViewModel();
                            model.ModuleId = rdr.GetInt32(0);
                            model.Name = rdr.GetString(1);
                            models.Add(model);
                        }
                        Cache.Set("modules", models, int.MaxValue);
                        rdr.Close();
                        cnn.Close();
                        return models;
                    }
                }
                catch
                {
                    return null;
                }

            }
            return data as List<ModuleViewModel>;
        }

        public static int GetModuleIdByName(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) return 0;
            var modules = GetModules();
            if (modules == null || modules.Count == 0) return 0;
            return modules.Where(p => p.Name.ToLower() == moduleName.ToLower()).Select(p => p.ModuleId).FirstOrDefault();            
        }

        public static List<int> GetAllowedModulesByUser()
        {
            var moduleInRolesViewModel = GetAllowedModulesInRolesByUser();
            if (moduleInRolesViewModel == null) return null;
            return moduleInRolesViewModel.Where(p => p.ProcessType == ProcessTypes.View).Select(p => p.ModuleId).ToList();
        }

        public static List<ProcessTypes> GetModulePermissionsByModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) return null;
            int moduleId = GetModuleIdByName(moduleName);
            if (moduleId == 0) return null;
            return GetModulePermissionsByModule(moduleId);
        }

        public static List<ProcessTypes> GetModulePermissionsByModule(int moduleId)
        {
            if (moduleId <= 0) return null;
            var moduleInRolesViewModel = GetAllowedModulesInRolesByUser();
            if (moduleInRolesViewModel == null) return null;
            return moduleInRolesViewModel.Where(p => p.ModuleId == moduleId).Select(p => p.ProcessType).ToList();
        }

        public static bool HasPermission(string moduleName, ProcessTypes process)
        {
            if (string.IsNullOrEmpty(moduleName)) return false;
            int moduleId = GetModuleIdByName(moduleName);
            if (moduleId == 0) return false;
            return HasPermission(moduleId, process);
        }

        public static bool HasPermission(int moduleId, ProcessTypes process)
        {
            if (moduleId <= 0) return false;
            var moduleInRolesViewModel = GetAllowedModulesInRolesByUser();
            if (moduleInRolesViewModel == null) return false;
            return moduleInRolesViewModel.Where(p => p.ModuleId == moduleId && p.ProcessType == process).FirstOrDefault() != null;
        }
    }
}
