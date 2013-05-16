using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using Thesis.Common.Helpers;

namespace Thesis.Web.UI.Controls
{
    public class FileUpload : Ext.Net.FormPanel, IBaseControl
    {
        public FileUpload()
        {
            SetDefaultValues = true;
            AllowBlank = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        protected override void OnClientInit(bool reinit)
        {
            if (SelectedFileFromModel.HasValue && SelectedFileFromModel.Value > 0)
            {
                HBox hBoxLoading = new HBox() { ID = string.Format("loadingControl_{0}", this.ID) };

                Ext.Net.Image imageLoading = new Ext.Net.Image() { 
                    ImageUrl = "/Content/loading.gif",
                    StyleSpec = "padding-top:1px;"
                };

                hBoxLoading.Items.Add(imageLoading);

                Ext.Net.Label labelLoading = new Ext.Net.Label() { 
                    StyleSpec = "padding-left:4px; padding-top:1px;",
                    Text = "Loading..." 
                };

                hBoxLoading.Items.Add(labelLoading);

                this.Items.Add(hBoxLoading);

                base.Listeners.BeforeRender.Handler = string.Format(@"#{{uploadControl_{0}}}.hide(); #{{downloadControl_{0}}}.hide(); #{{{3}}}.setValue({1});
                                                                        Ext.net.DirectMethod.request({{
                                                                                url          : '{2}',
                                                                                cleanRequest : true,
                                                                                params       : {{
                                                                                    id : {1}
                                                                                }},
                                                                                success : function(result) {{
                                                                                    #{{loadingControl_{0}}}.hide();
                                                                                    #{{{3}}}.setValue(result.extraParams.FileID);
                                                                                    #{{buttonDownload_{0}}}.setText(result.extraParams.FileName);
                                                                                    Ext.net.ResourceMgr.registerIcon(result.extraParams.Icon);
                                                                                    #{{buttonDownload_{0}}}.setIconClass(result.extraParams.IconCls);
                                                                                    #{{downloadControl_{0}}}.doLayout(true, true);
                                                                                    #{{fileUpload_{0}}}.allowBlank = true;
                                                                                    #{{uploadControl_{0}}}.hide();
                                                                                    #{{downloadControl_{0}}}.show();
                                                                                }},
                                                                                failure : function (errorResponse) {{
                                                                                    #{{loadingControl_{0}}}.hide();
                                                                                    #{{{3}}}.setValue(null);
                                                                                    #{{uploadControl_{0}}}.show();
                                                                                    #{{downloadControl_{0}}}.hide();
                                                                                }}
                                                                        }});", this.ID, SelectedFileFromModel.Value, (GetFileByIDUrl ?? "/Files/GetFileByID/"), this.ID.Replace("_Form",""));
            }
       
            base.OnClientInit(reinit);
        }

        #region Properties

        public string UploadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string GetFileByIDUrl { get; set; }
        public string AllowedFileTypes { get; set; }
        public int AllowedFileSize { get; set; }
        public bool AllowBlank { get; set; }
        public FileTypes? FileType { get; set; }
        public int? SelectedFileFromModel { get; set; }

        public string BeforeUploadHandler { get; set; }
        public string UploadSuccessHandler { get; set; }
        public string UploadFailureHandler { get; set; }

        public string BeforeDeleteHandler { get; set; }
        public string DeleteSuccessHandler { get; set; }
        public string DeleteFailureHandler { get; set; }

        #endregion

        #region Methods

        private string GetFileTypes(FileTypes? type)
        {
            if (!type.HasValue) return string.Empty;

            string allowedTypes = string.Empty;

            if (type.Value == FileTypes.Image)
                allowedTypes = Ax.GetAppSetting("AllowedImageTypes");
            else if (type.Value == FileTypes.Document)
                allowedTypes = Ax.GetAppSetting("AllowedDocumentTypes");
            else if (type.Value == FileTypes.Video)
                allowedTypes = Ax.GetAppSetting("AllowedVideoTypes");

            return allowedTypes ?? string.Empty;
        }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                base.Border = false;
                base.FileUpload = true;

                Ext.Net.Hidden hiddenFileID = new Ext.Net.Hidden() { ID = this.ID, Value = "" };
                this.ID = this.ID + "_Form";
                
                #region Upload

                HBox hBoxUploadControl = new HBox() { ID = string.Format("uploadControl_{0}", this.ID) };

                // width set edilebilir olmalı
                Ext.Net.FileUploadField fileUpload = new Ext.Net.FileUploadField() { 
                    ID = string.Format("fileUpload_{0}", this.ID), 
                    Width = 206, 
                    Icon = Ext.Net.Icon.Attach,
                    AllowBlank = this.AllowBlank
                };

                Ext.Net.Button buttonUpload = new Ext.Net.Button() { ID = string.Format("buttonUpload_{0}", this.ID), Text = "Upload", StyleSpec = "padding-left:3px;", Icon = Ext.Net.Icon.FolderGo };
                buttonUpload.DirectEvents.Click.Url = UploadUrl ?? "/Files/FileUpload/";
                buttonUpload.DirectEvents.Click.IsUpload = true;
                buttonUpload.DirectEvents.Click.CleanRequest = true;
                buttonUpload.DirectEvents.Click.Method = Ext.Net.HttpMethod.POST;
                buttonUpload.DirectEvents.Click.Before = (BeforeUploadHandler ?? string.Empty) + "if (!#{" + this.ID + "}.getForm().isValid()) { return false; } Ext.Msg.wait('Uploading your file...', 'Uploading');";
                buttonUpload.DirectEvents.Click.Failure = "Ext.MessageBox.hide(); if(result.errors && result.errors.length > 0) { #{" + fileUpload.ID + "}.markInvalid(result.errors[0].msg); }" + (UploadFailureHandler ?? string.Empty);
                buttonUpload.DirectEvents.Click.Success = string.Format(@"{6} #{{{0}}}.setValue(result.extraParams.FileID);
                                                            #{{{1}}}.setText(result.extraParams.FileName);
                                                            Ext.net.ResourceMgr.registerIcon(result.extraParams.Icon);
                                                            #{{{1}}}.setIconClass(result.extraParams.IconCls);
                                                            #{{downloadControl_{2}}}.doLayout(true, true);
                                                            Ext.MessageBox.hide();
                                                            #{{{3}}}.allowBlank = true;
                                                            #{{{3}}}.reset();
                                                            #{{{4}}}.hide();
                                                            #{{{5}}}.show(); ", hiddenFileID.ID, string.Format("buttonDownload_{0}", this.ID), this.ID, fileUpload.ID, hBoxUploadControl.ID, string.Format("downloadControl_{0}", this.ID), (UploadSuccessHandler ?? string.Empty));

                string allowedTypes = GetFileTypes(this.FileType);
                if (!string.IsNullOrEmpty(allowedTypes))
                    this.AllowedFileTypes = allowedTypes + (!string.IsNullOrEmpty(this.AllowedFileTypes) ? "," + this.AllowedFileTypes : string.Empty);

                buttonUpload.DirectEvents.Click.ExtraParams.AddRange(new List<Ext.Net.Parameter>() { 
                    new Ext.Net.Parameter() { Name = "AllowedFileTypes", Value = AllowedFileTypes },
                    new Ext.Net.Parameter() { Name = "AllowedFileSize", Value = AllowedFileSize.ToString() },
                    new Ext.Net.Parameter() { Name = "FieldID", Value = fileUpload.ID },
                    new Ext.Net.Parameter() { Name = "FileID", Value = "0", Mode = Ext.Net.ParameterMode.Value }
                });

                hBoxUploadControl.Items.Add(fileUpload);
                hBoxUploadControl.Items.Add(buttonUpload);

                this.Items.Add(hBoxUploadControl);

                #endregion

                #region Download

                HBox hBoxDownloadControl = new HBox() { ID = string.Format("downloadControl_{0}", this.ID), StyleSpec = "margin-left: -3px; margin-top: 1px;", Hidden = true };

                Ext.Net.LinkButton buttonDownload = new Ext.Net.LinkButton() { ID = string.Format("buttonDownload_{0}", this.ID), Text = "Download" };
                buttonDownload.DirectEvents.Click.Url = DownloadUrl ?? "/Files/FileDownload";
                buttonDownload.DirectEvents.Click.CleanRequest = true;
                buttonDownload.DirectEvents.Click.Method = Ext.Net.HttpMethod.POST;
                buttonDownload.DirectEvents.Click.FormID = "proxyForm";
                buttonDownload.DirectEvents.Click.IsUpload = true;
                buttonDownload.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter() { Name = "id", Value = "#{" + hiddenFileID.ID + "}.getValue()", Mode = Ext.Net.ParameterMode.Raw });

                Ext.Net.ImageButton deleteButton = new Ext.Net.ImageButton() { ID = string.Format("deleteButton_{0}", this.ID), ImageUrl = "/Content/icon-cross.png", StyleSpec = "padding-left:5px;" };

                StringBuilder clickHandler = new StringBuilder();
                clickHandler.Append((BeforeDeleteHandler ?? string.Empty));//Before
                clickHandler.Append(string.Format("#{{DeletedFileIds_{0}}}.setValue(#{{DeletedFileIds_{0}}}.getValue() + #{{{0}}}.getValue() + ',');", hiddenFileID.ID)); // Click
                clickHandler.Append(string.Format(@"#{{{0}}}.allowBlank = {3};
                                        #{{{0}}}.reset();
                                        #{{{1}}}.show();
                                        #{{{2}}}.hide();
                                        #{{{4}}}.setValue(null);
                                        if(Ext.isIE) {{ #{{{0}}}.el.setStyle('width', '122px'); }}
                                        {5}", fileUpload.ID, hBoxUploadControl.ID, hBoxDownloadControl.ID, fileUpload.AllowBlank.ToString().ToLower(), hiddenFileID.ID, (DeleteSuccessHandler ?? string.Empty))); // Success

                StringBuilder sbConfirmation = new StringBuilder();
                sbConfirmation.Append("var dlg = Ext.Msg.confirm('Confirm', 'Do you want to delete this file?', function (btn) { if(btn == 'yes') { " + clickHandler.ToString() + " } else { #{" + this.ID + "}.focus(); } }).getDialog();");
                sbConfirmation.Append("dlg.defaultButton = 1;");
                sbConfirmation.Append("dlg.focus();");

                deleteButton.Listeners.Click.Handler = sbConfirmation.ToString();

                hBoxDownloadControl.Items.Add(buttonDownload);
                hBoxDownloadControl.Items.Add(deleteButton);
                hBoxDownloadControl.Items.Add(hiddenFileID);
                hBoxDownloadControl.Items.Add(new Ext.Net.Hidden() { ID = string.Format("DeletedFileIds_{0}", hiddenFileID.ID) });
                hBoxDownloadControl.Items.Add(new Ext.Net.Hidden() { ID = string.Format("DeleteUrl_{0}", hiddenFileID.ID), Value = DeleteUrl ?? "/Files/DeleteFile/" });

                this.Items.Add(hBoxDownloadControl);

                #endregion

            }
        }

        #endregion

        #region FileType

        public enum FileTypes
        { 
            Image = 1,
            Document = 2,
            Video
        }

        #endregion
    }
}
