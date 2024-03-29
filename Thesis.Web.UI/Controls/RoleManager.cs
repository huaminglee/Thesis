﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using Thesis.Authorization.ViewModels;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.Services;

namespace Thesis.Web.UI.Controls
{
    public class RoleManager : Ext.Net.Panel, IBaseControl
    {
        public RoleManager()
        {
            SetDefaultValues = true;
        }

        protected override void OnClientInit(bool reinit)
        {
            LoadDefaultValues();
            base.OnClientInit(reinit);
        }

        #region Properties

        public List<ModulesInRolesViewModel> ModulesInRoles { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                Border = false;
                this.Width = 460;

                List<int> modules = ModuleAuthorizeService.GetAllowedModulesByUser();

                Ext.Net.FieldSet fieldSet = null;
                Ext.Net.CheckboxGroup checkBoxGroup = null;
                int moduleId;
                ProcessTypes processType = 0;
                List<ProcessTypes> processTypes = null;
                ModulesInRolesViewModel modulesInRolesViewModel = null;
                bool hasModuleInRoles = ModulesInRoles != null && ModulesInRoles.Count > 0;              
                foreach (var item in this.Items)
                {
                    if (item is Ext.Net.FieldSet)
                    {
                        fieldSet = item as Ext.Net.FieldSet;
                        foreach (var cbg in fieldSet.Items)
                        {
                            if (cbg is Ext.Net.CheckboxGroup)
                            {
                                checkBoxGroup = cbg as Ext.Net.CheckboxGroup;

                                moduleId = ModuleAuthorizeService.GetModuleIdByName(checkBoxGroup.ID);
                                if (moduleId > 0)
                                {
                                    if (modules == null || modules.IndexOf(moduleId) == -1)
                                    {
                                        cbg.Visible = false;
                                        continue;
                                    }
                                }
                                else
                                    continue;

                                processTypes = ModuleAuthorizeService.GetModulePermissionsByModule(moduleId);

                                foreach (var cb in checkBoxGroup.Items)
                                {
                                    if (Enum.IsDefined(typeof(ProcessTypes), cb.ID.Split('_')[1]))
                                    {
                                        processType = (ProcessTypes)Enum.Parse(typeof(ProcessTypes), cb.ID.Split('_')[1]);

                                        if (processTypes == null || processTypes.IndexOf(processType) == -1)
                                        {
                                            cb.Checked = false;
                                            cb.Disabled = true;
                                            continue;
                                        }

                                        if (hasModuleInRoles)
                                        {
                                            modulesInRolesViewModel = ModulesInRoles.Where(p => p.ModuleId == moduleId && p.ProcessType == processType).FirstOrDefault();

                                            if (modulesInRolesViewModel != null)
                                                cb.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (fieldSet.Items.Where(p => p is Ext.Net.CheckboxGroup && p.Visible == true).Count() == 0)
                            fieldSet.Visible = false;
                        else
                        {
                            bool hasAllModule = true;
                            foreach (var cg in fieldSet.Items)
                            {
                                if (cg is Ext.Net.CheckboxGroup)
                                {
                                    hasAllModule = (cg as Ext.Net.CheckboxGroup).Items.Where(p => p.Checked == false).Count() == 0;
                                    if (!hasAllModule)
                                        break;
                                }
                            }
                            if (hasAllModule)
                                fieldSet.Listeners.AfterRender.Handler = "item.checkbox.dom.checked = true;";
                        }
                    }
                }
            }
        }

        #endregion
    }
}
