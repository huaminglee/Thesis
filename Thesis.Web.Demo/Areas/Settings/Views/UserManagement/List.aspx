<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>User Management</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ax:ViewPort ID="viewPortList" runat="server">
        <Items>
            <ax:TabPanel ID="tpUserManagementList" runat="server" IDMode="Static">            
                <ListPage>                
                    <ax:GridPanel
                        ID="grdPnlUsers" 
                        runat="server" 
                        Title="Users" 
                        Module="UserManagement"
                        EnableSearch="true"
                        FirstRowSelectOnLoad="true"
                        IDProperty="UserId"
                        DeleteUrl="/Settings/UserManagement/Delete">
                        <DetailPage TabPanelID="tpUserManagementList" Title="UserName" Url="/Settings/UserManagement/Detail" />
                        <Store>                       
                            <ax:Store runat="server" Url="/Settings/UserManagement/GetByFilter">
                                <BaseParams>
                                     <ext:Parameter Name="dir" Value="ASC" />
                                     <ext:Parameter Name="sort" Value="UserName" />
                                </BaseParams>
                            </ax:Store>
                        </Store>
                        <ColumnModel runat="server">
                            <Columns>                                                                                                            
                                <ext:Column ColumnID="UserName" DataIndex="UserName" Header="User Name" />
                                <ext:Column Width="170" ColumnID="Email" DataIndex="Email" Header="Email" />
                                <ext:DateColumn ColumnID="LastLoginDate" DataIndex="LastLoginDate" Header="Last Login Date" />
                                <ext:DateColumn ColumnID="CreateDate" DataIndex="CreateDate" Header="Create Date" />
                            </Columns>
                        </ColumnModel>
                    </ax:GridPanel>
                </ListPage>
            </ax:TabPanel>
        </Items>
    </ax:ViewPort>
</asp:Content>
