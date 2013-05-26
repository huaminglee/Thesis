<%@ Page Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Addresses</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <ax:ViewPort ID="viewPortList" runat="server">
        <Items>
            <ax:TabPanel ID="tpAddressList" runat="server" IDMode="Static">            
                <ListPage>                
                    <ax:GridPanel
                        ID="grdPnlAddresses"
                        runat="server" 
                        Title="Addresses"
                        Module="Address"
                        EnableSearch="true" 
                        FirstRowSelectOnLoad="true"
                        IDProperty="AddressID"
                        DeleteUrl="/Crm/Address/Delete">
                        <DetailPage TabPanelID="tpAddressList" Title="Street" Url="/Crm/Address/Detail" />
                        <Store>                       
                            <ax:Store runat="server" Url="/Crm/Address/GetByFilter" />
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>                                                                                                            
                                <ext:Column ColumnID="AddressType" DataIndex="AddressType" Header="Type" />
                                <ext:Column ColumnID="Street" DataIndex="Street" Header="Street" />
                                <ext:Column ColumnID="HouseNumber" DataIndex="HouseNumber" Header="House Number" />
                                <ext:Column ColumnID="Addition" DataIndex="Addition" Header="Addition" />
                                <ext:Column ColumnID="Postalcode" DataIndex="Postalcode" Header="Postalcode" />
                                <ext:Column ColumnID="City" DataIndex="City" Header="City" />
                                <ext:Column ColumnID="Relation" DataIndex="Relation" Header="Relation" />
                                <ext:Column ColumnID="Phone" DataIndex="Phone" Header="Phone" />
                            </Columns>
                        </ColumnModel>
                    </ax:GridPanel>
                </ListPage>
            </ax:TabPanel>
        </Items>
    </ax:ViewPort>
</asp:Content>
