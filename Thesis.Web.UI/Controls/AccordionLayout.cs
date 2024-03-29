﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.Services;

namespace Thesis.Web.UI.Controls
{
    public class AccordionLayout : Ext.Net.AccordionLayout, IBaseControl
    {
        public AccordionLayout()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool CheckRoles { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (CheckRoles)
                {
                    if (this.Items != null)
                    {
                        List<int> modules = ModuleAuthorizeService.GetAllowedModulesByUser();
                        MenuPanel menuPanel = null;
                        MenuItem menuItem = null;
                        bool hasVisibleItem = false;
                        int moduleId;
                        foreach (var panel in this.Items)
                        {
                            menuPanel = panel as MenuPanel;
                            if (menuPanel != null && menuPanel.Menu != null)
                            {
                                hasVisibleItem = false;
                                foreach (var item in menuPanel.Menu.Items)
                                {
                                    menuItem = item as MenuItem;
                                    if (menuItem != null)
                                    {
                                        moduleId = ModuleAuthorizeService.GetModuleIdByName(menuItem.Module);
                                        item.Visible = moduleId == 0 || (modules != null && modules.IndexOf(moduleId) != -1);
                                        if (!hasVisibleItem) hasVisibleItem = item.Visible;
                                    }
                                }
                                panel.Visible = hasVisibleItem;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
