﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.Services;
using Thesis.Web.UI.Helpers;

namespace Thesis.Web.UI.Controls
{
    public class GridPanel : Ext.Net.GridPanel, IBaseControl, IStoreBinder
    {
        public GridPanel()
        {
            SetDefaultParams();
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            LoadStoreOnActive();
            base.OnInit(e);
        }

        #region Properties

        public string BeforeDeleteHandler { get; set; }
        public string DeleteFailureHandler { get; set; }
        public string DeleteSuccessHandler { get; set; }

        public string AfterAddHandler { get; set; }
        public string BeforeAddHandler { get; set; }

        public string ToolbarOrder { get; set; }

        public string Module { get; set; }

        public string IDProperty { get; set; }
        public DetailPageItem DetailPage { get; set; }
        public string DeleteUrl { get; set; }
        public string FormID { get; set; }

        #region Search

        public bool ToggleSearch { get; set; }
        public bool EnableSearch { get; set; }
        public Ext.Net.CollapseMode SearchCollapseMode { get; set; }

        #endregion

        public int SelectedFilterColumnIndex { get; set; }
        public bool AutoFillColumns { get; set; }
        public bool SingleSelect { get; set; }
        public bool FirstRowSelectOnLoad { get; set; }
        public bool DisableStripeRows { get; set; }
        public bool EnableBorder { get; set; }
        public bool DisableTrackMouseOver { get; set; }
        public bool DisableMask { get; set; }
        public bool LoadOnActive { get; set; }
        public bool AutoLoadStore { get; set; }
        public int PageSize { get; set; }

        public bool DisableTopbar { get; set; }
        public bool DisableAdd { get; set; }
        public bool DisableDelete { get; set; }
        public bool DisableExport { get; set; }
        public bool DisableExcelExport { get; set; }
        public bool DisableXmlExport { get; set; }
        public bool DisableCsvExport { get; set; }

        public bool DisableSelectionModel { get; set; }

        public bool DisableBottomBar { get; set; }

        public bool DisableKeyMap { get; set; }

        private string Params
        {
            get
            {
                if (DetailPage == null)
                    return string.Empty;

                var queryParams = DetailPage.BaseParams;

                if (queryParams == null || queryParams.Count == 0)
                    return string.Empty;

                bool isValueMode = false;

                StringBuilder sb = new StringBuilder("+'?'+");
                foreach (Ext.Net.Parameter param in queryParams.ToList())
                {
                    isValueMode = param.Mode == Ext.Net.ParameterMode.Value;

                    sb.Append("'" + (param.Name ?? string.Empty) + "=" +
                        (isValueMode ? string.Empty : "'+") +
                        (param.Value ?? string.Empty) +
                        (isValueMode ? "&'+" : "+'&'+")
                        );
                }

                return sb.ToString().Substring(0, sb.Length - (isValueMode ? 3 : 5)) + (isValueMode ? "'" : string.Empty);
            }
        }

        //
        // Summary:
        //     An object containing properties which are to be sent as parameters on any
        //     HTTP request.
        [Description("An object containing properties which are to be sent as parameters on any HTTP request.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Ext.Net.ViewStateMember]
        [Ext.Net.Meta]
        [Category("3. Store")]
        [NotifyParentProperty(true)]
        public Ext.Net.ParameterCollection FilterParams { get; set; }

        #endregion

        #region Methods

        private void SetDefaultParams()
        {
            SetDefaultValues = true;
            AutoLoadStore = true;
            PageSize = 50;
            ToggleSearch = true;
            SearchCollapseMode = Ext.Net.CollapseMode.Mini;
            AutoFillColumns = true;
        }

        private void DeleteDirectEvent(Ext.Net.ComponentDirectEvent directEvent)
        { 
            directEvent.Url = DeleteUrl;
            directEvent.CleanRequest = true;
            directEvent.Before = (BeforeDeleteHandler ?? string.Empty) + "#{" + ID + "}.getEl().mask('Deleting...');";
            directEvent.Failure = "#{" + ID + "}.getEl().unmask(); if(result.extraParams && result.extraParams.hasPermission && result.extraParams.hasPermission == '0') { Ax.ShowNotification('Warning', result.extraParams.msg, 'icon-exclamation'); " + "#{" + (this.ID ?? string.Empty) + "}.view.focusRow(0);" + "  } else { Ax.ShowNotification('Warning', result.extraParams.msg, 'icon-exclamation'); " + "#{" + (this.ID ?? string.Empty) + "}.view.focusRow(0);" + " if(result.extraParams && result.extraParams.isSingle && result.extraParams.isSingle == '0') { #{store" + this.ID + "}.reload(); } }" + (DeleteFailureHandler ?? string.Empty);
            directEvent.Success = "#{" + ID + "}.getEl().unmask(); var btnDel = Ext.getCmp('" + string.Format("btnDelete{0}", this.ID) + "'); if(btnDel){ btnDel.disable(); } #{store" + this.ID + "}.reload();" + (DeleteSuccessHandler ?? string.Empty);

            directEvent.Confirmation.Cancel = "#{" + (this.ID ?? string.Empty) + "}.view.focusRow(0);";
            directEvent.Confirmation.ConfirmRequest = true;
            directEvent.Confirmation.Title = "Alert";
            directEvent.Confirmation.Message = "Are you sure?";

            directEvent.ExtraParams.Add(
                new Ext.Net.Parameter {
                    Name = "rows",
                    Value = "Ax.SelectedRecords(#{" + this.ID + "})",
                    Mode = Ext.Net.ParameterMode.Raw
                });
        }

        private string DetailPageScript(bool isNew)
        {
            return DetailPageScript(isNew, "item", "rowIndex");
        }

        private string DetailPageScript(bool isNew, string itemID, string rowIndex)
        {
            string scrpt = BeforeAddHandler ?? string.Empty;
            if (!string.IsNullOrEmpty(DetailPage.TabPanelID))
            {
                scrpt = isNew ?
                    "Ax.AddDetailTab({ title: 'New', url: " + GetDetailUrl(DetailPage.Url) + ", tabName: '" + (DetailPage.TabPanelID ?? string.Empty) + "', gridPanelID: '" + (this.ID ?? string.Empty) + "' });" :
                    "Ax.AddDetailTab({ title: '" + (DetailPage.Title ?? string.Empty) + "', url: " + GetDetailUrl(DetailPage.Url) + ", tabName: '" + (DetailPage.TabPanelID ?? string.Empty) + "', gridPanelID: '" + (this.ID ?? string.Empty) + "' }, " + rowIndex + ", " + itemID + ");";
            }
            else if (!string.IsNullOrEmpty(DetailPage.WindowID))
            {
                scrpt = isNew ?
                  "Ax.LoadWindow({ title: 'New', url: " + GetDetailUrl(DetailPage.Url) + ", gridPanelID: '" + (this.ID ?? string.Empty) + "' }, #{" + DetailPage.WindowID + "});" :
                  "Ax.LoadWindow({ title: '" + (DetailPage.Title ?? string.Empty) + "', url: " + GetDetailUrl(DetailPage.Url) + ", gridPanelID: '" + (this.ID ?? string.Empty) + "' }, #{" + DetailPage.WindowID + "}, " + rowIndex + ", " + itemID + ");";
            }
            else 
            {
                scrpt = isNew ?
                    "Ax.AddWindow({ title: 'New', url: " + GetDetailUrl(DetailPage.Url) + ", gridPanelID: '" + (this.ID ?? string.Empty) + "' });" :
                    "Ax.AddWindow({ title: '" + (DetailPage.Title ?? string.Empty) + "', url: " + GetDetailUrl(DetailPage.Url) + ", gridPanelID: '" + (this.ID ?? string.Empty) + "' }, " + rowIndex + ", " + itemID + ");";
            }
            return scrpt + (AfterAddHandler ?? string.Empty);
        }

        private string GetDetailUrl(string url)
        {
            if (url == null)
                return string.Empty;

            return "'" + url + "'" + Params;
        }

        private void LoadStoreOnActive()
        {
            if (!(LoadOnActive && base.ParentComponent is Panel))
                return;

            Panel panel = base.ParentComponent as Panel;

            if (panel == null)
                return;

            Ext.Net.StoreCollection storeColl = base.Store;

            if (storeColl != null && storeColl.Count > 0)
            {
                Ext.Net.Store store = storeColl[0];
                panel.Listeners.Activate.Handler += "if(!#{" + store.ID + "}.loaded) #{" + store.ID + "}.reload();";
            }
        }

        private string AddFilterItem(string id, string propertyName, string whereCondition, string filterCondition, string startValue, string endValue, bool isValueMode)
        {
            StringBuilder sb = new StringBuilder("{");
            sb.Append("\"Id\":\"" + id + "\",");
            sb.Append("\"PropertyName\":\"" + propertyName + "\",");
            sb.Append("\"WhereCondition\":\"" + whereCondition + "\",");
            sb.Append("\"FilterCondition\":\"" + filterCondition + "\",");
            sb.Append("\"StartValue\":\"" + FormatValue(startValue, isValueMode) + "\",");
            sb.Append("\"EndValue\":\"" + endValue + "\"}");
            return sb.ToString();
        }

        private string FormatValue(string value, bool isValueMode)
        {
            return isValueMode ? value : string.Format("' + {0} + '", value);
        }

        private void DisableVisible()
        {
            this.Visible = false;
            if (this.EnableSearch)
            {
                var borderLayout = base.ParentComponent as Ext.Net.BorderLayout;
                if (borderLayout != null)
                {
                    borderLayout.Visible = false;
                    var panel = borderLayout.ParentComponent as Ext.Net.Panel;
                    if (panel != null)
                    {
                        panel.Visible = false;
                        var fieldSet = panel.ParentComponent as Ext.Net.FieldSet;
                        if (fieldSet != null) fieldSet.Visible = false;
                    }
                }
            }
            else
            {
                var fieldSet = base.ParentComponent as Ext.Net.FieldSet;
                if (fieldSet != null) fieldSet.Visible = false;
            }
        }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                StripeRows = !DisableStripeRows;
                Border = EnableBorder;
                TrackMouseOver = !DisableTrackMouseOver;

                #region Permissions

                bool hasAddPermission = true, hasDeletePermission = true;
                if (!string.IsNullOrEmpty(this.Module))
                {
                    List<ProcessTypes> permissions = ModuleAuthorizeService.GetModulePermissionsByModule(this.Module);
                    if (permissions == null)
                    {
                        DisableVisible();
                        return;
                    }
                    else
                    {
                        if (permissions.IndexOf(ProcessTypes.View) == -1) { DisableVisible(); return; }
                        hasAddPermission = permissions.IndexOf(ProcessTypes.Add) != -1;
                        hasDeletePermission = permissions.IndexOf(ProcessTypes.Delete) != -1;
                    }
                }

                #endregion

                #region TopBar

                Ext.Net.Button btnDelete = null;

                if (!DisableTopbar)
                {
                    Ext.Net.Toolbar toolbar = null;
                    bool hasToolBar = this.TopBar != null && this.TopBar.Count > 0;
                    if (!hasToolBar)
                        toolbar = new Ext.Net.Toolbar() { ID = string.Format("toolbar{0}", ID) };
                    else
                    {
                        toolbar = (Ext.Net.Toolbar)this.TopBar.First();
                    }

                    #region BtnAdd

                    Ext.Net.Button btnAdd = null;
                    if (hasAddPermission && !DisableAdd && (DetailPage != null))
                    {
                        btnAdd = new Ext.Net.Button {
                            ID = string.Format("btnAdd{0}", this.ID),
                            Text = "Add",
                            Icon = Ext.Net.Icon.Add
                        };

                        if (DetailPage != null)
                            btnAdd.Listeners.Click.Handler = DetailPageScript(true);
                   }

                    #endregion

                    #region BtnDelete

                    if (hasDeletePermission && !DisableDelete && !string.IsNullOrEmpty(DeleteUrl))
                    {
                        btnDelete = new Ext.Net.Button {
                            ID = string.Format("btnDelete{0}", this.ID),
                            Text = "Delete",
                            Icon = Ext.Net.Icon.Delete,
                            Disabled = true
                        };

                        DeleteDirectEvent(btnDelete.DirectEvents.Click);
                    }

                    #endregion

                    #region ToogleSearch

                    Ext.Net.Button btnToogleSearch = null;
                    if (this.EnableSearch && this.ToggleSearch)
                    {
                        btnToogleSearch = new Ext.Net.Button() {
                            ID = string.Format("btnToogleSearch_{0}", this.ID),
                            Icon = Ext.Net.Icon.TableLightning,
                            Text="Filter",
                            EnableToggle=true
                        };
                        btnToogleSearch.Listeners.Click.Handler = string.Format("#{{panel_search_{0}}}.toggleCollapse();", this.ID);
                    }

                    #endregion

                    #region BtnExport

                    Ext.Net.Button btnExport = null;

                    if (!DisableExport && (!DisableExcelExport || !DisableXmlExport || !DisableCsvExport))
                    {
                        btnExport = new Ext.Net.Button {
                            ID = string.Format("btnExport{0}", this.ID),
                            Text = "Export",
                            Icon = Ext.Net.Icon.DatabaseGo
                        };

                        Ext.Net.Menu exportMenu = new Ext.Net.Menu();

                        if(!DisableExcelExport) exportMenu.Items.Add(new Ext.Net.MenuItem { Text = "Excel", Icon = Ext.Net.Icon.PageExcel });
                        if(!DisableXmlExport)   exportMenu.Items.Add(new Ext.Net.MenuItem { Text = "XML", Icon = Ext.Net.Icon.PageCode });
                        if(!DisableCsvExport)   exportMenu.Items.Add(new Ext.Net.MenuItem { Text = "CSV", Icon = Ext.Net.Icon.PageAttach });

                        exportMenu.DirectEvents.Click.IsUpload = true;
                        exportMenu.DirectEvents.Click.CleanRequest = true;
                        exportMenu.DirectEvents.Click.Url = ((Ext.Net.HttpProxy)this.Store[0].Proxy[0]).Url;
                        exportMenu.DirectEvents.Click.FormID = this.FormID ?? "proxyForm";

                        exportMenu.DirectEvents.Click.ExtraParams.AddRange(
                            new List<Ext.Net.Parameter> { 
                                new Ext.Net.Parameter{ Name="dir",  Value = "#{" + this.ID + "}.store.sortInfo.direction", Mode= Ext.Net.ParameterMode.Raw },
                                new Ext.Net.Parameter{ Name="sort", Value = "#{" + this.ID + "}.store.sortInfo.field", Mode= Ext.Net.ParameterMode.Raw },
                                new Ext.Net.Parameter{ Name="ExportFormat", Value = "menuItem.iconCls", Mode= Ext.Net.ParameterMode.Raw },
                                new Ext.Net.Parameter{ Name="IsExport", Value = "true" }                            
                            });

                        string[] exportParams = { "start", "limit", "dir", "sort", "exportformat", "isexport" };
                        
                        foreach (Ext.Net.Parameter param in this.Store.Primary.BaseParams)
                        {
                            if(Array.IndexOf(exportParams, param.Name.ToLower().Trim()) == -1)
                                exportMenu.DirectEvents.Click.ExtraParams.Add(param);
                        }

                        btnExport.Menu.Add(exportMenu);
                    }

                    #endregion

                    #region ToolbarOrder

                    bool isOrdered = false;

                    if (!string.IsNullOrEmpty(ToolbarOrder) && ToolbarOrder.IndexOf(',') != -1)
                    {
                        string[] buttons = ToolbarOrder.Split(',');
                        int totalItemCount = toolbar.Items.Count;
                        int controlItemCount = 3;
                        if (buttons.Length == totalItemCount + controlItemCount)
                        {
                            isOrdered = true;
                            Ext.Net.Component[] toolbarItems = new Ext.Net.Component[totalItemCount];
                            toolbar.Items.CopyTo(toolbarItems);
                            toolbar.Items.Clear();
                            string controlIndex;
                            int index = 0;
                            int j = 0;
                            for (int i = 0; i < buttons.Length; i++)
                            {
                                controlIndex = buttons[i].ToLower().Trim();

                                if (controlIndex == "0")
                                {
                                    if (btnAdd != null) { toolbar.Items.Insert(j, btnAdd); j++; }
                                }
                                else if (controlIndex == "1")
                                {
                                    if (btnDelete != null) { toolbar.Items.Insert(j, btnDelete); j++; }
                                }
                                else if (controlIndex == "2")
                                {
                                    if (btnToogleSearch != null) { toolbar.Items.Insert(j, btnToogleSearch); j++; }
                                }
                                else
                                {
                                    if (Thesis.Common.Helpers.Ax.TryParse(controlIndex, ref index) && index >= controlItemCount)
                                    {
                                        toolbar.Items.Insert(j, toolbarItems[index - controlItemCount]);
                                        j++;
                                    }
                                }
                            }
                        }
                    }

                    if (!isOrdered)
                    {
                        int index = 0;
                        if (btnAdd != null) { toolbar.Items.Insert(index, btnAdd); index++; }
                        if (btnDelete != null) { toolbar.Items.Insert(index, btnDelete); index++; }
                        if (btnToogleSearch != null) { toolbar.Items.Insert(index, btnToogleSearch); }
                    }

                    #endregion

                    toolbar.Items.Add(new Ext.Net.ToolbarFill());
                    if (btnExport != null) toolbar.Items.Add(btnExport);

                    if (!hasToolBar)
                        TopBar.Add(toolbar);
                }
                
                #endregion

                #region SelectionModel

                if (!DisableSelectionModel && SelectionModel.Count == 0)
                {
                    Ext.Net.RowSelectionModel rowSelectionModel = new Ext.Net.RowSelectionModel();
                    rowSelectionModel.SingleSelect = SingleSelect;

                    if (btnDelete != null)
                    {
                        rowSelectionModel.Listeners.RowSelect.Handler = "#{" + btnDelete.ID + "}.enable();";
                        rowSelectionModel.Listeners.RowDeselect.Handler = "if (!#{" + this.ID + "}.hasSelection()) {#{" + btnDelete.ID + "}.disable();}";
                    }

                    SelectionModel.Add(rowSelectionModel);
                }

                #endregion
                
                #region BottomBar

                if (!DisableBottomBar && BottomBar.Count == 0)
                    BottomBar.Add(new Ext.Net.PagingToolbar { PageSize = this.PageSize });

                #endregion

                #region KeyMap

                if (!DisableKeyMap)
                {
                    if (hasDeletePermission && !DisableDelete && !string.IsNullOrEmpty(DeleteUrl))
                    {
                        if (btnDelete != null)
                        {
                            Ext.Net.KeyBinding deleteKey = new Ext.Net.KeyBinding();
                            deleteKey.StopEvent = true;
                            deleteKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.DELETE });
                            deleteKey.Listeners.Event.Handler = "#{" + btnDelete.ID + "}.fireEvent('click', #{" + btnDelete.ID + "});";

                            KeyMap.Add(deleteKey);
                        }
                        else
                        {
                            Listeners.KeyDown.StopEvent = true;
                            Listeners.KeyDown.Handler = "return e.browserEvent.keyCode == 46 && #{" + this.ID + "}.hasSelection();";

                            DeleteDirectEvent(DirectEvents.KeyDown);
                        }
                    }

                    if (hasAddPermission && !DisableAdd && (DetailPage != null))
                    {
                        Ext.Net.KeyBinding newRecord = new Ext.Net.KeyBinding();
                        newRecord.StopEvent = true;
                        newRecord.Ctrl = true;
                        newRecord.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.N });
                        newRecord.Listeners.Event.Handler = DetailPageScript(true);

                        KeyMap.Add(newRecord);
                    }

                    if(DetailPage != null)
                    {
                        Ext.Net.KeyBinding detailKey = new Ext.Net.KeyBinding();
                        detailKey.StopEvent = true;
                        detailKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.ENTER });
                        detailKey.Listeners.Event.Handler = DetailPageScript(false, "#{" + (this.ID ?? string.Empty) + "}", "Ax.GetSelectedRowIndex(#{" + (this.ID ?? string.Empty) + "})");
                        
                        KeyMap.Add(detailKey);
                    }
                }

                #endregion

                #region View

                if (AutoFillColumns && View.Count == 0)
                    View.Add(new Ext.Net.GridView() { AutoFill=true });

                #endregion

                if (DetailPage != null)
                    Listeners.RowDblClick.Handler = DetailPageScript(false) + Listeners.RowDblClick.Handler;

                LoadMask.ShowMask = !DisableMask;
            }
        }

        #endregion

        #region IStoreBinder Members

        public void BindStore(Store store)
        {
            store.AutoLoad = !LoadOnActive && AutoLoadStore;
            store.ID = string.Format("store{0}", this.ID);
            store.RemoteSort = true;
            store.UseIdConfirmation = true;

            if (store.Reader.Count == 0)
            {
                store.Reader.Add(new Ext.Net.JsonReader {
                    IDProperty = this.IDProperty,
                    Root = "data",
                    TotalProperty = "total"
                });

                foreach (Ext.Net.ColumnBase column in ColumnModel.Columns)
                    store.AddField(new Ext.Net.RecordField { Name = column.DataIndex, Type = UIHelper.GetFieldType(column) });
            }

            if (!this.DisableBottomBar) {
                if (!store.BaseParams.Any(p => p.Name.ToLower() == "start")) store.BaseParams.Add(new Ext.Net.Parameter { Name = "start", Value = "0", Mode = Ext.Net.ParameterMode.Raw });
                if (!store.BaseParams.Any(p => p.Name.ToLower() == "limit")) store.BaseParams.Add(new Ext.Net.Parameter { Name = "limit", Value = this.PageSize.ToString(), Mode = Ext.Net.ParameterMode.Raw });
            }

            var dirParam = store.BaseParams.Where(p => p.Name.ToLower() == "dir").FirstOrDefault();
            if (dirParam == null) store.BaseParams.Add(new Ext.Net.Parameter { Name = "dir", Value = "DESC" });

            var sortParam = store.BaseParams.Where(p => p.Name.ToLower() == "sort").FirstOrDefault();
            if (sortParam == null) store.BaseParams.Add(new Ext.Net.Parameter { Name = "sort", Value = this.IDProperty });

            store.SortInfo.Field = sortParam == null ? this.IDProperty : sortParam.Value;
            store.SortInfo.Direction = dirParam == null || dirParam.Value.ToLower() == "desc" ? Ext.Net.SortDirection.DESC : Ext.Net.SortDirection.ASC;

            if (FilterParams != null && FilterParams.Count > 0)
            {
                StringBuilder param = new StringBuilder();

                bool isValueMode = false, isFirst = true;
                foreach (var prm in FilterParams)
                {
                    isValueMode = prm.Mode == Ext.Net.ParameterMode.Value;
                    if (!isFirst) param.Append(",");
                    param.Append(AddFilterItem(string.Empty, prm.Name, "And", "Equals", prm.Value, string.Empty, isValueMode));
                    if (isFirst) isFirst = false;
                }

                store.BaseParams.Add(new Ext.Net.Parameter { Name = "FilterBase", Value = string.Format("'{0}'", param.ToString()), Mode = Ext.Net.ParameterMode.Raw });                    
            }

            if (this.EnableSearch)
                store.BaseParams.Add(new Ext.Net.Parameter { Name = "Filter", Value = string.Format("#{{hidden_SearchCondition_{0}}}.value", this.ID), Mode = Ext.Net.ParameterMode.Raw });

            if (FirstRowSelectOnLoad)
                store.Listeners.Load.Handler = "if (!#{" + (this.ID ?? string.Empty) + "}.hasSelection()) { #{" + (this.ID ?? string.Empty) + "}.getSelectionModel().selectFirstRow(); } #{" + (this.ID ?? string.Empty) + "}.view.focusRow(0);" + store.Listeners.Load.Handler;

            if (LoadOnActive)
                store.Listeners.Load.Handler += "this.loaded = true;";

        }

        #endregion

        #region Helper Objects

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public class DetailPageItem
        {
            public string TabPanelID { get; set; }
            public string WindowID { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }

            //
            // Summary:
            //     An object containing properties which are to be sent as parameters on any
            //     HTTP request.
            [Description("An object containing properties which are to be sent as parameters on any HTTP request.")]
            [PersistenceMode(PersistenceMode.InnerProperty)]
            [Ext.Net.ViewStateMember]
            [Ext.Net.Meta]
            [Category("3. Store")]
            [NotifyParentProperty(true)]
            public Ext.Net.ParameterCollection BaseParams { get; set; }
        }
        
        public enum DetailPageType
        { 
            Tab = 1, Window = 2
        }

        #endregion
    }
}
