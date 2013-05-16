<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Settings.RoleManagementViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Detail</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ext:ViewPort ID="viewPortDetail" runat="server" Layout="fit" IDMode="Static">
        <Items>
            <ext:TabPanel ID="RoleManagementPanel" runat="server" Border="false" LayoutOnTabChange="true">
                <Items>
                    <ax:DetailPanel 
                        ID="pnlRoleManagementDetail" 
                        runat="server"
                        Title="Detail" 
                        CheckDirty="true"                        
                        FormPanelID="RoleManagementForm"
                        Module="RoleManagement"
                        IDProperty="ID"
                        IdType="Guid"
                        Url="/Settings/RoleManagement/Detail"
                        DeleteUrl="/Settings/RoleManagement/Delete">
                        <Items>
                            <ax:FormPanel ID="RoleManagementForm" Url="/Settings/RoleManagement/Save" runat="server">
                                <Items>
                                    <ax:FieldSet runat="server" Title="Role">
                                        <Items>
                                            <ext:TextField ID="RoleName" AutoFocus="true" runat="server" FieldLabel="Role Name" MaxLength="255" AllowBlank="false" AutoDataBind="true" Text="<%# Model.RoleName %>" />
                                            <ext:TextArea ID="Description" runat="server" FieldLabel="Description" Text="<%# Model.Description %>" AutoDataBind="true" Width="165" />
                                        </Items>
                                    </ax:FieldSet>
                                    <ax:RoleManager runat="server" ModulesInRoles="<%# Model.ModulesInRolesViewModel %>" AutoDataBind="true">
                                        <Items>
                                            <ax:ModuleGroup runat="server" Title="CRM">
                                                <Items>
                                                    <ax:ModuleRoles ID="Address" runat="server" FieldLabel="Addresses" />
                                                </Items>
                                            </ax:ModuleGroup>
                                            <ax:ModuleGroup runat="server" Title="Activities">
                                                <Items>
                                                    <ax:ModuleRoles ID="Activity" runat="server" FieldLabel="Activity" />
                                                </Items>
                                            </ax:ModuleGroup>
                                            <ax:ModuleGroup runat="server" Title="Settings">
                                                <Items>
                                                    <ax:ModuleRoles ID="Lookup" runat="server" FieldLabel="Lookups" />
                                                    <ax:ModuleRoles ID="RoleManagement" runat="server" FieldLabel="Role Management" />
                                                    <ax:ModuleRoles ID="UserManagement" runat="server" FieldLabel="User Management" />
                                                </Items>
                                            </ax:ModuleGroup>
                                        </Items>
                                    </ax:RoleManager>
                                    <ext:Hidden ID="ID" runat="server" Value="<%# Model.RoleId.ToString() %>" AutoDataBind="true" />
                                </Items>
                            </ax:FormPanel>
                        </Items>
                    </ax:DetailPanel>
                    <ax:Panel ID="UserPanel" runat="server" Title="Users">
                        <ListPage>
                            <ax:GridPanel
                                ID="grdPnlRelatedUsers" 
                                runat="server" 
                                LoadOnActive="true"
                                Module="UserManagement"
                                EnableSearch="true"
                                FirstRowSelectOnLoad="true"
                                IDProperty="UserId"
                                DeleteUrl="/Settings/UserManagement/Delete">
                                <DetailPage Title="UserName" Url="/Settings/UserManagement/Detail" />
                                <Store>                       
                                    <ax:Store runat="server" Url="/Settings/UserManagement/GetUsersByRoleId">
                                        <BaseParams>
                                             <ext:Parameter Name="dir" Value="ASC" />
                                             <ext:Parameter Name="sort" Value="UserName" />
                                             <ext:Parameter Name="roleId" Value="#{ID}.getValue()" Mode="Raw" />
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
                    </ax:Panel>
                </Items>
            </ext:TabPanel>
        </Items>
    </ext:ViewPort>
</asp:Content>
