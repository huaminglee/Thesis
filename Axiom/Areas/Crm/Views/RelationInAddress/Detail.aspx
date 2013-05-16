<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Crm.RelationInAddressesViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>RelatedRelationDetail</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ext:ViewPort ID="ViewPortDetail" runat="server" Layout="fit" IDMode="Static">
        <Items>
                    <ax:DetailPanel 
                        ID="pnlRelatedRelationDetail" 
                        CheckDirty="true"
                        FormPanelID="RelatedRelationForm"
                        IDProperty="ID"
                        Url="/Crm/RelationInAddress/Detail"
                        DeleteUrl="/Crm/RelationInAddress/Delete"                   
                        runat="server">
                        <Items>
                            <ax:FormPanel ID="RelatedRelationForm" Url="/Crm/RelationInAddress/Save" runat="server">
                                <Items>
                                    <ax:FieldSet runat="server" Title="Related Relation">
                                        <Items>
                                            <ax:ComboBox ID="RelationID" runat="server" FieldLabel="Relation" AllowBlank="false" AutoFocusOnAfterRender="true">
                                                <SelectedItemModel AutoDataBind="true" Text="<%# Model.RelationName %>" Value="<%# Model.RelationID %>" />
                                                <Store>
                                                    <ax:Store runat="server" Url="/Settings/BaseTable/GetRelations" />
                                                </Store>
                                            </ax:ComboBox>
                                        </Items>
                                    </ax:FieldSet>
                                    <ax:Hidden ID="AddressID" runat="server" SetQueryParam="true" />
                                    <ext:Hidden ID="ID" runat="server" Value="<%# Model.RelationInAddressesID %>" AutoDataBind="true" />
                                </Items>
                            </ax:FormPanel>
                        </Items>
                    </ax:DetailPanel>
        </Items>
    </ext:ViewPort>
</asp:Content>
