using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using Thesis.Authorization.Enumerations;
using Thesis.Authorization.Services;
using Thesis.Web.UI.Helpers;
using Thesis.Common.Enumerations;

namespace Thesis.Web.UI.Controls
{
    public class DetailPanel : Ext.Net.Panel, IBaseControl
    {
        public DetailPanel()
        {
            SetDefaultValues = true;
            GridPanelID = System.Web.HttpContext.Current.Request.QueryString["gridPanelID"];
            HasViewPermission = HasAddPermission = HasUpdatePermission = HasDeletePermission = true;
        }

        protected override void OnInit(EventArgs e)
        {
            #region Authorization

            if (!string.IsNullOrEmpty(this.Module))
            {
                List<ProcessTypes> permissions = ModuleAuthorizeService.GetModulePermissionsByModule(this.Module);
                if (permissions == null)
                {
                    HasViewPermission = false;                        
                    this.Visible = false;
                    return;
                }
                else
                {
                    HasViewPermission = permissions.IndexOf(ProcessTypes.View) != -1;
                    if (!HasViewPermission) { this.Visible = false; return; }
                    HasAddPermission = permissions.IndexOf(ProcessTypes.Add) != -1;
                    HasUpdatePermission = permissions.IndexOf(ProcessTypes.Update) != -1;
                    HasDeletePermission = permissions.IndexOf(ProcessTypes.Delete) != -1;
                }
            }

            #endregion

            LoadDefaultValues();
            base.OnInit(e);
        }

        protected override void OnClientInit(bool reinit)
        {
            if (!HasViewPermission) return;
            if (!IsNew.HasValue)
            {
                int? idValue = Thesis.Common.Helpers.Ax.ConvertNullableIntValueWithDefault(System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["id"]);
                if (idValue.HasValue)
                {
                    IsNew = idValue.Value <= 0;
                }
            }
            LoadData();
            base.OnClientInit(reinit);
        }
        
        #region Properties

        public bool CheckDirty { get; set; }

        public int CheckDirtyBuffer { get; set; }

        public bool Debug { get; set; }

        public string ToolbarOrder { get; set; }

        public bool? IsNew { get; set; }

        #region Authorization Properties

        public string Module { get; set; }
        private bool HasViewPermission { get; set; }
        private bool HasAddPermission { get; set; }
        private bool HasUpdatePermission { get; set; }
        private bool HasDeletePermission { get; set; }

        #endregion

        public string SetDefaultUIValuesFn { get; set; }
        public bool AutoClose { get; set; }
        public bool EnableBorder { get; set; }
        public bool DisableTopBar { get; set; }
        public bool DisableSuccessHandler { get; set; }
        public bool DisableFailureHandler { get; set; }
        
        public string BeforeDeleteHandler { get; set; }
        public string DeleteFailureHandler { get; set; }
        public string DeleteSuccessHandler { get; set; }

        public string BeforeCloseHandler { get; set; }

        public string BeforeSaveHandler { get; set; }
        public string AfterSuccessHandler { get; set; }
        public string AfterFailureHandler { get; set; }

        private string DeleteFilesScript
        {
            get
            {
                StringBuilder deleteFiles = new StringBuilder();
                deleteFiles.Append(@"var _fileUploadIds = Ax.GetFileUploadIdsByForm(form); 
                                        if(_fileUploadIds.length > 0) { 
                                            var fileUploadIds = _fileUploadIds.substring(0, _fileUploadIds.length - 1).split(',');
                                            var deletedIds = '';
                                            for(var i = 0; i < fileUploadIds.length; i++) {
                                                var fileUploadId = fileUploadIds[i].substring(0, fileUploadIds[i].length - 5).replace('fileUpload_', '');
                                                var deletedFileIdsCmp = Ext.getCmp('DeletedFileIds_' + fileUploadId);
                                                var deletedIds = deletedFileIdsCmp.getValue();
                                                if(deletedIds == '') continue;
                                                if(deletedIds.substring(deletedIds.length - 1) == ',') deletedIds = deletedIds.substring(0, deletedIds.length - 1);
                                                deletedIds = '[' + deletedIds + ']';
                                                Ext.net.DirectMethod.request({
                                                                                url          : Ext.getCmp('DeleteUrl_' + fileUploadId).getValue(),
                                                                                cleanRequest : true,
                                                                                params       : { fileIds : deletedIds }
                                                                        });
                                                deletedFileIdsCmp.setValue('');
                                            }
                                        }");

                return deleteFiles.ToString();
            }
        }

        private string successHandler;
        public string SuccessHandler { 
            get 
            {
                if (string.IsNullOrEmpty(successHandler))
                {
                     
                    StringBuilder sb = new StringBuilder();                    
                    sb.Append("if (action.options.params && action.options.params.setNew) { ");
                        sb.Append("if (action.result.extraParams.newID) {");
                        sb.Append(IDProperty + ".setValue(action.result.extraParams.newID); }");
                        sb.Append(DeleteFilesScript + AfterSuccessHandler);
                        sb.Append("Ax.AddNewTab(window, { title: 'New', url: '" + (Url ?? string.Empty) + "', gridPanelID: '" + (GridPanelID ?? string.Empty) + "' });");                    
                    sb.Append("} else if (action.result && action.result.extraParams.msg) {");
                        sb.Append("if (action.result.extraParams.newID) {");
                            sb.Append(IDProperty + ".setValue(action.result.extraParams.newID);");
                            if(!HasUpdatePermission)
                                sb.Append(string.Format("#{{btnSave{0}}}.hide(); #{{btnSaveAndNext{0}}}.events.click.clearListeners(); #{{btnSaveAndNext{0}}}.addListener('click', function () {{ {1} }}); #{{btnSaveAndNext{0}}}.setText('New');", ID, "Ax.AddNewTab(window, { title: 'New', url: '" + (Url ?? string.Empty) + "', gridPanelID: '" + (GridPanelID ?? string.Empty) + "' });"));
                            if (!string.IsNullOrEmpty(SetDefaultUIValuesFn))
                                sb.Append(string.Format("{0}();", SetDefaultUIValuesFn));
                            if (!string.IsNullOrEmpty(IDProperty))
                                sb.Append(string.Format("var __isNew = {0}.value == '{1}'; var __btnDel = Ext.getCmp('btnDelete{2}'); if(__btnDel) __btnDel.setDisabled(__isNew);", IDProperty, (this.IdType == IdType.Guid ? Guid.Empty.ToString() : "0"), this.ID));
                        sb.Append("}");
                        sb.Append("if(action.result.extraParams.title) { Ax.ChangeTitle(window, action.result.extraParams.title); }");
                        sb.Append("Ax.ShowNotification('Success', action.result.extraParams.msg, 'icon-information');");
                        sb.Append(DeleteFilesScript + AfterSuccessHandler);
                    sb.Append("}");
                    
                    successHandler = sb.ToString();
                }

                return successHandler;
            } 
            set 
            {
                successHandler = value;
            } 
        }

        private string failureHandler;
        public string FailureHandler
        {
            get 
            {
                if (string.IsNullOrEmpty(failureHandler))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("if (action.result && action.result.extraParams.msg) {");
                    if (this.Debug)
                    {
                        sb.Append("var errorMessage = ''; var errors = action.result.errors; if(errors && errors.length > 0) {");
                        sb.Append("for(var i=0; i<errors.length;i++) { errorMessage += 'id:' + errors[i].id + ' ' + 'msg:' + errors[i].msg + '<br>'; } } ");
                        sb.Append("Ax.ShowMessage('Save Error', errorMessage == '' ? 'Field error not found.' : errorMessage, Ext.Msg.OK, Ext.MessageBox.ERROR);");
                    }
                    else
                        sb.Append("Ax.ShowNotification('Save Error', action.result.extraParams.msg, 'icon-exclamation');");
               
                    failureHandler = sb.Append("}").ToString();
                }

                return failureHandler;
            }
            set 
            {
                failureHandler = value;
            }
        }

        private string closeHandler;
        public string CloseHandler
        {
            get
            {
                if (string.IsNullOrEmpty(closeHandler))
                    return (BeforeCloseHandler ?? string.Empty) + " Ax.CloseTab(window, '" + (GridPanelID ?? string.Empty) + "');";

                return closeHandler;
            }
            set { closeHandler = value; }
        }

        public string FormPanelID { get; set; }
        public string DeleteUrl { get; set; }
        public string IDProperty { get; set; }
        public IdType IdType { get; set; }
        public string GridPanelID { get; set; }
        public string Url { get; set; }
        public bool DisableKeyMap { get; set; }

        public bool DisableSave { get; set; }
        public bool DisableSaveAndNew { get; set; }
        public bool DisableDelete { get; set; }
        public bool DisableClose { get; set; }

        private string Success
        {
            get 
            {
                string success = string.Empty;
                if (!string.IsNullOrEmpty(SuccessHandler))
                {
                    success = "function (form, action) { #{" + ID + "}.getEl().unmask(); " + SuccessHandler + (CheckDirty ? string.Format("#{{hdnFormObject_{0}}}.setValue(Ax.ClearDirtyForm(#{{{1}}}));", this.ID, this.FormPanelID) : string.Empty) + " }";
                }
                if (success.Length > 0)
                    success = string.Format(", success: {0}", success);

                return success;
            }
        }

        private string Failure
        {
            get
            {
                string failure = string.Empty;
                if (!string.IsNullOrEmpty(FailureHandler))
                    failure = "function (form, action) { #{" + ID + "}.getEl().unmask(); " + FailureHandler + (AfterFailureHandler ?? string.Empty) + " }";

                if (failure.Length > 0)
                    failure = string.Format(", failure: {0}", failure);

                return failure;
            }
        }

        #endregion

        #region Methods

        private void LoadData()
        {
            this.SuccessHandler = null;
            this.CloseHandler = null;
           
            string flr = Failure;
            string cls = CloseHandler;
            string scss = AutoClose ? string.Format(", success: function(form, action){{ {0} {1} {2} {3} }}", DeleteFilesScript, (AfterSuccessHandler ?? string.Empty), (CheckDirty ? string.Format("#{{hdnFormObject_{0}}}.setValue(Ax.ClearDirtyForm(#{{{1}}}));", this.ID, this.FormPanelID) : string.Empty), cls) : Success;

            StringBuilder clsHandler = new StringBuilder();
            if (this.CheckDirty)
            {
                clsHandler.Append(string.Format("var renderedFormObj = #{{hdnFormObject_{0}}}.getValue(); var currentFormObj = Ax.ClearDirtyForm(#{{{1}}});", this.ID, this.FormPanelID));
                //clsHandler.Append("debugger;");
                clsHandler.Append("if(!Ax.IsNullOrEmpty(renderedFormObj) && renderedFormObj != currentFormObj) {");
                clsHandler.Append("var dlg = Ext.Msg.confirm('Confirm', 'You have a uncommited changes. Do you want to close this tab?', function (btn) { if(btn == 'yes') { " + cls + " } else { " + (FormPanelID ?? string.Empty) + ".focus(); } }).getDialog();");
                clsHandler.Append("dlg.defaultButton = 1;");
                clsHandler.Append("dlg.focus();");
                clsHandler.Append("} else { " + cls + " }");
            }
            else
            {
                clsHandler.Append("var dlg = Ext.Msg.confirm('Confirm', 'Do you want to close this tab?', function (btn) { if(btn == 'yes') { " + cls + " } else { " + (FormPanelID ?? string.Empty) + ".focus(); } }).getDialog();");
                clsHandler.Append("dlg.defaultButton = 1;");
                clsHandler.Append("dlg.focus();");
            }

            bool isIdName = !string.IsNullOrEmpty(IDProperty) && IDProperty.ToLower(new System.Globalization.CultureInfo("en-US")) == "id";

            #region TopBar

            if (!DisableTopBar)
            {
                if (!DisableSave && (HasAddPermission || HasUpdatePermission))
                {
                    SubmitButton btnSave = TopBar.Toolbar.Items.Where(i => i.ID == string.Format("btnSave{0}", ID)).First() as SubmitButton;

                    if (IsNew.HasValue && IsNew.Value == false && !HasUpdatePermission)
                        btnSave.Visible = false;
                    else
                        btnSave.Listeners.Click.Handler = "#{" + ID + "}.getEl().mask('Saving...'); if(Ax.IsValidForm(#{" + FormPanelID + "})) {" + (BeforeSaveHandler ?? string.Empty) + "#{" + FormPanelID + "}.form.submit({ params: { setNew:false" + (!isIdName ? ", id:#{" + IDProperty + "}.getValue()" : string.Empty) + " }" + scss + flr + " }); } else #{" + ID + "}.getEl().unmask();";
                }

                if (!DisableSaveAndNew && HasAddPermission)
                {
                    SubmitButton btnSaveAndNext = TopBar.Toolbar.Items.Where(i => i.ID == string.Format("btnSaveAndNext{0}", ID)).First() as SubmitButton;

                    bool setClickHandler = false;

                    if (IsNew.HasValue)
                    {
                        if (IsNew.Value == false && !HasUpdatePermission)
                        {
                            btnSaveAndNext.Text = "New";
                            btnSaveAndNext.Listeners.Click.Handler = "Ax.AddNewTab(window, { title: 'New', url: '" + (Url ?? string.Empty) + "', gridPanelID: '" + (GridPanelID ?? string.Empty) + "' });";
                            setClickHandler = true;
                        }
                    }

                    if (btnSaveAndNext.Visible && !setClickHandler)
                        btnSaveAndNext.Listeners.Click.Handler = "#{" + ID + "}.getEl().mask('Saving...'); if(Ax.IsValidForm(#{" + FormPanelID + "})) {" + (BeforeSaveHandler ?? string.Empty) + "#{" + FormPanelID + "}.form.submit({ params: { setNew:true" + (!isIdName ? ", id:#{" + IDProperty + "}.getValue()" : string.Empty) + " }" + scss + flr + " }); } else #{" + ID + "}.getEl().unmask();";
                }

                if (!DisableDelete && HasDeletePermission)
                {
                    Ext.Net.Button btnDelete = TopBar.Toolbar.Items.Where(i => i.ID == string.Format("btnDelete{0}", ID)).First() as Ext.Net.Button;
                    btnDelete.DirectEvents.Click.Success = cls + (DeleteSuccessHandler ?? string.Empty);
                }

                if (!DisableClose)
                {
                    Ext.Net.Button btnClose = TopBar.Toolbar.Items.Where(i => i.ID == string.Format("btnClose{0}", ID)).First() as Ext.Net.Button;
                    btnClose.Listeners.Click.Delay = 100;
                    btnClose.Listeners.Click.Handler = clsHandler.ToString();
                }
            }

            #endregion

            #region KeyMap

            if (!DisableKeyMap)
            {
                if (!DisableSave && (HasAddPermission || HasUpdatePermission))
                {
                    bool hasSaveKey = true;

                    if (IsNew.HasValue)
                    {
                        if (IsNew.Value == false && !HasUpdatePermission)
                        {
                            hasSaveKey = false;
                        }
                    }

                    if (hasSaveKey)
                    {
                        Ext.Net.KeyBinding saveKey = new Ext.Net.KeyBinding();
                        saveKey.StopEvent = true;
                        saveKey.Ctrl = true;
                        saveKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.S });
                        saveKey.Listeners.Event.Handler = "#{" + (FormPanelID ?? string.Empty) + "}.focus(); #{" + ID + "}.getEl().mask('Saving...'); if(Ax.IsValidForm(#{" + (FormPanelID ?? string.Empty) + "})) {" + (BeforeSaveHandler ?? string.Empty) + "#{" + (FormPanelID ?? string.Empty) + "}.form.submit({ params: { setNew:false" + (!isIdName ? ", id:#{" + IDProperty + "}.getValue()" : string.Empty) + " }" + scss + flr + " }); } else #{" + ID + "}.getEl().unmask();";

                        KeyMap.Add(saveKey);
                    }
                }

                if (!DisableSaveAndNew && HasAddPermission)
                {
                    Ext.Net.KeyBinding saveAndNextKey = new Ext.Net.KeyBinding();
                    saveAndNextKey.StopEvent = true;
                    saveAndNextKey.Ctrl = true;
                    saveAndNextKey.Shift = true;
                    saveAndNextKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.N });

                    bool hasKeyEvent = false;

                    if (IsNew.HasValue)
                    {
                        if (IsNew.Value == false && !HasUpdatePermission)
                        {
                            hasKeyEvent = true;
                            saveAndNextKey.Listeners.Event.Handler = "Ax.AddNewTab(window, { title: 'New', url: '" + (Url ?? string.Empty) + "', gridPanelID: '" + (GridPanelID ?? string.Empty) + "' });";
                        }
                    }

                    if(!hasKeyEvent)
                        saveAndNextKey.Listeners.Event.Handler = "#{" + (FormPanelID ?? string.Empty) + "}.focus(); #{" + ID + "}.getEl().mask('Saving...'); if(Ax.IsValidForm(#{" + (FormPanelID ?? string.Empty) + "})) {" + (BeforeSaveHandler ?? string.Empty) + "#{" + (FormPanelID ?? string.Empty) + "}.form.submit({ params: { setNew:true" + (!isIdName ? ", id:#{" + IDProperty + "}.getValue()" : string.Empty) + " }" + scss + flr + " }); } else #{" + ID + "}.getEl().unmask();";

                    KeyMap.Add(saveAndNextKey);
                }

                if (!DisableClose)
                {
                    Ext.Net.KeyBinding closeKey = new Ext.Net.KeyBinding();
                    closeKey.StopEvent = true;
                    closeKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.ESC });
                    closeKey.Listeners.Event.Handler = clsHandler.ToString();

                    KeyMap.Add(closeKey);
                }
            }

            #endregion
        }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                Border = EnableBorder;
                if (this.IdType == 0)
                    this.IdType = IdType.Int;

                if (string.IsNullOrEmpty(Layout))
                    Layout = "fit";

                #region TopBar

                if (!DisableTopBar)
                {
                    Ext.Net.Toolbar toolBar = null;
                    bool hasToolBar = this.TopBar != null && this.TopBar.Count > 0;
                    if (hasToolBar)
                        toolBar = (Ext.Net.Toolbar)this.TopBar.First();
                    else
                        toolBar = new Ext.Net.Toolbar { ID = string.Format("toolbar{0}", ID) };

                    SubmitButton btnSave = null;
                    if (!DisableSave && (HasAddPermission || HasUpdatePermission))
                    {
                        btnSave = new SubmitButton
                        {
                            ID = string.Format("btnSave{0}", ID),
                            Text = "Save",
                            FormPanelID = this.FormPanelID,
                            DisableSuccessHandler = this.DisableSuccessHandler,
                            DisableFailureHandler = this.DisableFailureHandler
                        };
                    }

                    SubmitButton btnSaveAndNext = null;
                    if (!DisableSaveAndNew && HasAddPermission)
                    {
                        btnSaveAndNext = new SubmitButton
                        {
                            ID = string.Format("btnSaveAndNext{0}", ID),
                            Text = "Save and New",
                            FormPanelID = this.FormPanelID,
                            DisableSuccessHandler = this.DisableSuccessHandler,
                            DisableFailureHandler = this.DisableFailureHandler,
                            Icon = Ext.Net.Icon.Add,
                            SetNew = true
                        };
                    }

                    Ext.Net.Button btnDelete = null;
                    if (!DisableDelete && HasDeletePermission)
                    {
                        btnDelete = new Ext.Net.Button { 
                            ID = string.Format("btnDelete{0}", ID),
                            Text = "Delete",
                            Icon = Ext.Net.Icon.Cross
                        };

                        btnDelete.DirectEvents.Click.Url = DeleteUrl;
                        btnDelete.DirectEvents.Click.CleanRequest = true;
                        btnDelete.DirectEvents.Click.Before = (BeforeDeleteHandler ?? string.Empty) + "#{" + ID + "}.getEl().mask('Deleting...');";
                        btnDelete.DirectEvents.Click.Failure = "#{" + ID + "}.getEl().unmask(); if(result.extraParams && result.extraParams.hasPermission && result.extraParams.hasPermission == '0') { Ax.ShowNotification('Warning', result.extraParams.msg, 'icon-exclamation');  } else { Ax.ShowNotification('Warning', result.extraParams.msg, 'icon-exclamation'); }" + (DeleteFailureHandler ?? string.Empty);

                        btnDelete.DirectEvents.Click.Confirmation.ConfirmRequest = true;
                        btnDelete.DirectEvents.Click.Confirmation.Title = "Alert";
                        btnDelete.DirectEvents.Click.Confirmation.Message = "Are you sure?";

                        btnDelete.DirectEvents.Click.ExtraParams.Add(
                            new Ext.Net.Parameter {
                                Name = "rows",
                                Value = "[#{" + IDProperty + "}.value]",
                                Mode = Ext.Net.ParameterMode.Raw
                            });                           
                    }

                    Ext.Net.Button btnClose = null;
                    if (!DisableClose)
                    {
                        btnClose = new Ext.Net.Button {
                            ID = string.Format("btnClose{0}", ID),
                            Text = "Close",
                            Icon = Ext.Net.Icon.Cancel
                        };
                    }

                    #region ToolbarOrder

                    bool isOrdered = false;

                    if (!string.IsNullOrEmpty(ToolbarOrder) && ToolbarOrder.IndexOf(',') != -1)
                    {
                        string[] buttons = ToolbarOrder.Split(',');
                        int totalItemCount = toolBar.Items.Count;
                        int controlItemCount = 4;
                        if (buttons.Length == totalItemCount + controlItemCount)
                        {
                            isOrdered = true;
                            Ext.Net.Component[] toolbarItems = new Ext.Net.Component[totalItemCount];
                            toolBar.Items.CopyTo(toolbarItems);
                            toolBar.Items.Clear();
                            string controlIndex;
                            int index = 0;
                            int j = 0;
                            for (int i = 0; i < buttons.Length; i++)
                            {
                                controlIndex = buttons[i].ToLower().Trim();

                                if (controlIndex == "0")
                                {
                                    if (btnSave != null) { toolBar.Items.Insert(j, btnSave); j++; }
                                }
                                else if (controlIndex == "1")
                                {
                                    if (btnSaveAndNext != null) { toolBar.Items.Insert(j, btnSaveAndNext); j++; }
                                }
                                else if (controlIndex == "2")
                                {
                                    if (btnDelete != null) { toolBar.Items.Insert(j, btnDelete); j++; }
                                }
                                else if (controlIndex == "3")
                                {
                                    if (btnClose != null) { toolBar.Items.Insert(j, btnClose); j++; }
                                }
                                else
                                {
                                    if (Thesis.Common.Helpers.Ax.TryParse(controlIndex, ref index) && index >= controlItemCount)
                                    {
                                        toolBar.Items.Insert(j, toolbarItems[index - controlItemCount]);
                                        j++;
                                    }
                                }
                            }
                        }
                    }

                    if(!isOrdered)
                    {
                        int index = 0;
                        if (btnSave != null) { toolBar.Items.Insert(index, btnSave); index++; }
                        if (btnSaveAndNext != null) { toolBar.Items.Insert(index, btnSaveAndNext); index++; }
                        if (btnDelete != null) { toolBar.Items.Insert(index, btnDelete); index++; }
                        if (btnClose != null) { toolBar.Items.Add(btnClose); }
                    }

                    #endregion

                    if (!hasToolBar && toolBar.Items.Count > 0)
                        TopBar.Add(toolBar);
                }

                #endregion
      
                if (!string.IsNullOrEmpty(IDProperty))
                    Listeners.Render.Handler += string.Format("var __isNew = {0}.value == '{1}'; var __btnDel = Ext.getCmp('btnDelete{2}'); if(__btnDel) __btnDel.setDisabled(__isNew);", IDProperty, (this.IdType == IdType.Guid ? Guid.Empty.ToString() : "0"), this.ID);

                if (!string.IsNullOrEmpty(SetDefaultUIValuesFn))
                    Listeners.Render.Handler += string.Format("{0}();", SetDefaultUIValuesFn);

                if (CheckDirty)
                {
                    this.Items.Add(new Ext.Net.Hidden() { ID = string.Format("hdnFormObject_{0}", this.ID) });
                    Listeners.AfterRender.Handler += string.Format("#{{hdnFormObject_{0}}}.setValue(Ax.ClearDirtyForm(#{{{1}}}));", this.ID, this.FormPanelID);
                    Listeners.AfterRender.Buffer = CheckDirtyBuffer > 0 ? CheckDirtyBuffer : 650;
                }
            }
        }

        #endregion
    }
}
