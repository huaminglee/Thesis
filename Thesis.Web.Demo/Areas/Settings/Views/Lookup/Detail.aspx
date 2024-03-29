﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Settings.LookupViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ext:ViewPort ID="viewPortDetail" runat="server" Layout="fit" IDMode="Static">
        <Items>
            <ax:DetailPanel 
                ID="pnlLookupDetail"
                FormPanelID="LookupForm"
                IDProperty="LookupId"
                Url="/Settings/Lookup/Detail"
                DeleteUrl="/Settings/Lookup/Delete"
                DisableClose="true"
                AfterSuccessHandler="OnAfterSuccessHandler(form, action); return;"
                BeforeCloseHandler="OnBeforeCloseHandler(); return;"
                runat="server">
                <Items>
                    <ax:FormPanel ID="LookupForm" Url="/Settings/Lookup/Save" runat="server">
                        <Items>
                            <ax:FieldSet runat="server" Title="Lookup Detail">
                                <Items>
                                    <ext:TextField ID="Text_en_us" FieldLabel="Text" Text='<%# Model.LookupResources.Any(p => p.LanguageCode == "en-US") ? Model.LookupResources.Where(p => p.LanguageCode == "en-US").First().Text : string.Empty %>' 
                                        AllowBlank="false" MaxLength="500" runat="server" AutoDataBind="true" IndicatorIcon="FlagGb" />
                                    <ext:TextField ID="Text_nl_nl" FieldLabel="Tekst" Text='<%# Model.LookupResources.Any(p => p.LanguageCode == "nl-NL") ? Model.LookupResources.Where(p => p.LanguageCode == "nl-NL").First().Text : string.Empty %>' 
                                        AllowBlank="false" MaxLength="500" runat="server" AutoDataBind="true" IndicatorIcon="FlagNl" />
                                </Items>
                            </ax:FieldSet>
                            <ax:Hidden ID="lookupTypeId" SetQueryParam="true" runat="server" />
                            <ext:Hidden ID="LookupId" runat="server" Value="<%# Model.LookupId.ToString() %>" AutoDataBind="true" />                            
                        </Items>
                    </ax:FormPanel>
                </Items>
            </ax:DetailPanel>
            <ax:Hidden ID="gridPanelID" runat="server" SetQueryParam="true" />
        </Items>
    </ext:ViewPort>
    <script type="text/javascript">
        function OnAfterSuccessHandler(form, action) {
            UpdateGridRecord(action);

            if (action.options.params && action.options.params.setNew) {
                if (action.result.extraParams.title) Ax.ChangeTitle(window, 'New');
                window.location = Ax.QueryFormat('/Settings/Lookup/Detail', '?', '/0') + window.location.search;
            }
        }

        function OnBeforeCloseHandler() {
            var grdPnlId = Ext.getCmp('gridPanelID').value;
            if (!Ax.IsNullOrEmpty(grdPnlId)) {
                eval("window.parent." + grdPnlId + ".store.reload();");
            }
        }

        function UpdateGridRecord(action) {           
            var grdPnlId = Ext.getCmp('gridPanelID').value;
            if (!Ax.IsNullOrEmpty(grdPnlId)) {
                var id = Ext.getCmp('LookupId').value;
                eval("var grid = window.parent." + grdPnlId);
                var gridStore = grid.store;

                if (action.result.extraParams.newID) {
                    gridStore.insertRecord(0, { LookupId: action.result.extraParams.newID, Text: action.result.extraParams.title });
                    gridStore.commitChanges();
                } else {
                    var gridStoreLength = gridStore.data.items.length;
                    for (var i = 0; i < gridStoreLength; i++) {
                        var row = gridStore.data.items[i];
                        if (row.id == id) {
                            if (action.result.extraParams.title)
                                row.data.Text = action.result.extraParams.title;
                            break;
                        }
                    }
                    grid.getView().refresh();
                }
            }
        }
    </script>
</asp:Content>

