﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>List</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ax:ViewPort ID="viewPortList" runat="server">
        <Items>
            <ax:TabPanel ID="tpRoles" runat="server" IDMode="Static">            
                <ListPage>                
                    <ax:GridPanel
                        ID="grdPnlRoles" 
                        runat="server" 
                        Title="Roles" 
                        Module="RoleManagement"
                        EnableSearch="true"
                        FirstRowSelectOnLoad="true"
                        AutoExpandColumn="RoleName"
                        IDProperty="RoleId"
                        DeleteUrl="/Settings/RoleManagement/Delete">
                        <DetailPage TabPanelID="tpRoles" Title="RoleName" Url="/Settings/RoleManagement/Detail" />
                        <Store>                       
                            <ax:Store runat="server" Url="/Settings/RoleManagement/GetByFilter" />
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>                                
                                <ext:Column ColumnID="RoleName" DataIndex="RoleName" Header="Role Name" />
                            </Columns>
                        </ColumnModel>
                    </ax:GridPanel>
                </ListPage>
            </ax:TabPanel>
        </Items>
    </ax:ViewPort>
</asp:Content>
