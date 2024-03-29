﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Settings.UserManagementViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Detail</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ext:ViewPort ID="viewPortDetail" runat="server" Layout="fit" IDMode="Static">
        <Items>
            <ext:TabPanel ID="UserPanelPanel" runat="server" Border="false" LayoutOnTabChange="true">
                <Items>
                    <ax:DetailPanel 
                        ID="pnlUsersDetail" 
                        Title="Detail" 
                        FormPanelID="UserForm"
                        Module="UserManagement"
                        CheckDirty="true"
                        IDProperty="ID"
                        IdType="Guid"
                        Url="/Settings/UserManagement/Detail"
                        DeleteUrl="/Settings/UserManagement/Delete"
                        SetDefaultUIValuesFn="SetDefaultUIValues"
                        AfterSuccessHandler="SuccessHandler(form, action);"
                        BeforeSaveHandler="if(!BeforeSave()) return;"
                        runat="server">
                        <Items>
                            <ax:FormPanel ID="UserForm" Url="/Settings/UserManagement/Save" runat="server">
                                <Items>
                                    <ax:FieldSet runat="server" Title="User Information">
                                        <Items>
                                            <ext:TextField ID="UserName" Text="<%# Model.UserName %>" AllowBlank="false" AutoDataBind="true" FieldLabel="User Name" runat="server" MaxLength="50" AutoFocus="<%# Model.UserId == System.Guid.Empty ? true : false %>" />
                                            <ext:TextField ID="Email" Text="<%# Model.Email %>" AutoDataBind="true" FieldLabel="Email" runat="server" MaxLength="255" Vtype="email" AutoFocus="<%# Model.UserId == System.Guid.Empty ? false : true %>" />
                                        </Items>
                                    </ax:FieldSet>
                                    <ax:FieldSet ID="PasswordQuestionFieldSet" runat="server" Title="Password Question / Answer">
                                        <Items>
                                            <ext:TextField ID="PasswordQuestionPassword" MinLength="6" InputType="Password" AutoDataBind="true" FieldLabel="Password" runat="server" MaxLength="50" />
                                            <ext:TextField ID="PasswordQuestion" Text="<%# Model.PasswordQuestion %>" EnableKeyEvents="true" AutoDataBind="true" FieldLabel="Password Question" runat="server" MaxLength="255">
                                                <Listeners>
                                                    <KeyUp Handler="CheckPasswordQuestion(item);" />
                                                </Listeners>
                                            </ext:TextField>
                                            <ext:TextField ID="PasswordAnswer" AutoDataBind="true" FieldLabel="Password Answer" runat="server" MaxLength="128" />
                                        </Items>
                                    </ax:FieldSet>
                                    <ax:FieldSet ID="PasswordFieldSet" runat="server" Title="Password">
                                        <Items>
                                            <ext:TextField ID="OldPassword" MinLength="6" InputType="Password" AutoDataBind="true" FieldLabel="Old Password" runat="server" MaxLength="50" />                                                  
                                            <ext:TextField ID="NewPassword" MinLength="6" InputType="Password" AutoDataBind="true" FieldLabel="New Password" runat="server" MaxLength="50" />
                                            <ext:TextField ID="ConfirmPassword" MinLength="6" InputType="Password" AutoDataBind="true" FieldLabel="Confirm Password" runat="server" MaxLength="50" />
                                        </Items>
                                    </ax:FieldSet>
                                    <ax:FieldSet runat="server" Title="Role Management">
                                        <Items>
                                            <ax:CompositeField FieldLabel="Role" runat="server">
                                                <Items>
                                                    <ax:ComboBox ID="RoleId" runat="server">
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Settings/RoleManagement/GetRoles" />
                                                        </Store>                                                                       
                                                    </ax:ComboBox>
                                                    <ext:Button ID="btnAddRole" runat="server" Icon="Add" Text="Add">
                                                        <Listeners>
                                                            <Click Handler="
                                                                if(Ax.CheckRequired(#{RoleId}) && Ax.CheckDuplicate(#{grdPnlRelatedRoles}, 'RoleName', 'RoleName', #{RoleId}, 'Text')) {
                                                                    #{grdPnlRelatedRoles}.store.insertRecord(0, { RoleId: #{RoleId}.value, RoleName: #{RoleId}.getRawValue() });
                                                                    
                                                                    Ax.AddValueFromComboBox(#{RoleId}, 'Text', #{RoleNames});
                                                                    Ax.ClearComboBoxWithTrigger(#{RoleId});
                                                                }
                                                                " />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ax:CompositeField>
                                            <ax:GridPanel
                                                ID="grdPnlRelatedRoles" 
                                                Height="200"
                                                runat="server"   
                                                PageSize="200"                                                                                             
                                                AutoExpandColumn="RoleName"
                                                DisableAdd="true"
                                                DisableDelete="true"
                                                IDProperty="RoleId">
                                                <DetailPage Title="RoleName" Url="/Settings/RoleManagement/Detail" />
                                                <TopBar>
                                                    <ext:Toolbar runat="server">
                                                        <Items>
                                                            <ext:Button runat="server" ID="btnDeleteRoles" Text="Delete" Icon="Delete">
                                                                <Listeners>
                                                                    <Click Handler="Ax.DeleteValuesFromGridPanel(#{grdPnlRelatedRoles}, 'RoleName', #{RoleNames}, this);" />
                                                                </Listeners>
                                                            </ext:Button>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </TopBar>                                                
                                                <Store>                       
                                                    <ax:Store runat="server" Url="/Settings/RoleManagement/GetRolesByUserId">
                                                        <Listeners>
                                                            <Load Handler="Ax.LoadValuesFromRecords(records, 'RoleName', #{RoleNames});" />
                                                        </Listeners>
                                                        <BaseParams>
                                                            <ext:Parameter Name="dir" Value="ASC" />
                                                            <ext:Parameter Name="sort" Value="RoleName" />
                                                            <ext:Parameter Name="userId" Value="#{ID}.getValue()" Mode="Raw" />
                                                        </BaseParams>
                                                    </ax:Store>
                                                </Store>
                                                <ColumnModel runat="server">
                                                    <Columns>                                
                                                        <ext:Column ColumnID="RoleName" DataIndex="RoleName" Header="Role Name" />
                                                    </Columns>
                                                </ColumnModel>
                                            </ax:GridPanel>
                                        </Items>
                                    </ax:FieldSet>
                                    <ext:Hidden ID="RoleNames" runat="server" />
                                    <ext:Hidden ID="ID" runat="server" Value="<%# Model.UserId.ToString() %>" AutoDataBind="true" />
                                    <ext:Hidden ID="UserId" runat="server" Value="<%# Model.UserId.ToString() %>" AutoDataBind="true" />
                                </Items>
                            </ax:FormPanel>
                        </Items>
                    </ax:DetailPanel>
                </Items>
            </ext:TabPanel>
        </Items>
    </ext:ViewPort>
<script type="text/javascript">
    function SetDefaultUIValues() {
        var isNew = ID.value == '<%= System.Guid.Empty %>';
        UserName.setDisabled(!isNew);
        PasswordQuestionPassword[isNew ? 'hide' : 'show']();
        OldPassword[isNew ? 'hide' : 'show']();
        NewPassword.allowBlank = !isNew;
        ConfirmPassword.allowBlank = !isNew;
        PasswordFieldSet.setTitle(isNew ? 'Password' : 'Change Password');
        PasswordQuestionFieldSet.setTitle(isNew ? 'Password Question/Answer' : 'Change Password Question/Answer');
    }
    function SuccessHandler(form, action) {
        if (action.result.extraParams.newID) {
            UserId.setValue(action.result.extraParams.newID);
            OldPassword.reset();
            NewPassword.reset();
            ConfirmPassword.reset();
        }
        grdPnlRelatedRoles.store.commitChanges();
    }
    function BeforeSave() {
        if (grdPnlRelatedRoles.store.data.items.length == 0) {
            pnlUsersDetail.getEl().unmask();
            Ax.ShowNotification('Error', 'You must be selected from at least a role', 'icon-exclamation');
            RoleId.focus();
            return false;
        }
        return true;
    }
    function CheckPasswordQuestion(item) {
        var val = item.getValue();
        var isChange = val != '' && val != '<%= Model.PasswordQuestion %>';

        if (ID.value != '<%= System.Guid.Empty %>')
            PasswordQuestionPassword.allowBlank = !isChange;
        PasswordAnswer.allowBlank = !isChange;
    }
</script>
</asp:Content>
