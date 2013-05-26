<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Activity.ActivityViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Activity</title>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ax:ViewPort ID="viewPortDetail" runat="server" IDMode="Static">
        <Items>
    
                    <ax:DetailPanel CheckDirty="true"
                        ID="pnlActivityDetail"
                        FormPanelID="ActivityForm"
                        IDProperty="ActivityID"
                        Module="Activity"
                        Url="/Activity/Activity/Detail"
                        DeleteUrl="/Activity/Activity/Delete"
                        SetDefaultUIValuesFn="SetDefaultUIValues"
                        AutoDataBind="true"                        
                        runat="server">
                        <Items>
                            <ax:FormPanel ID="ActivityForm" Url="/Activity/Activity/Save" runat="server">
                                <Items>
                                    <ax:HBox runat="server">
                                        <Items>
                                            <ax:FieldSet runat="server" Title="General">
                                                <Items>                                                
                                                    <ext:TextField ID="Number1" runat="server" FieldLabel="Number" Disabled="true" Text="<%# Model.Number %>" AutoDataBind="true" Width="165" />
                                                    <ext:TextField ID="LopcNumber" runat="server" FieldLabel="Project Number" Disabled="true" Text="" AutoDataBind="true" Width="165" />
                                                    <ext:TextField ID="TypeName" runat="server" FieldLabel="Type" Disabled="true" Text="<%# Model.TypeName %>" AutoDataBind="true" Width="165" />
                                                    <ax:ComboBox ID="AddressID" runat="server" FieldLabel="Address" AutoFocusOnAfterRender="true" 
                                                        ItemSelector="tr.list-item" DisplayField="Street" ValueField="AddressID">
                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.Address %>" Value="<%# Model.AddressID %>" />
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Crm/Address/GetAddressListItem">
                                                                <Reader>
                                                                    <ext:JsonReader IDProperty="AddressID" Root="data" TotalProperty="total">
                                                                        <Fields>
                                                                            <ext:RecordField Name="AddressID" />
                                                                            <ext:RecordField Name="Street" />
                                                                            <ext:RecordField Name="HouseNumber" />
                                                                            <ext:RecordField Name="City" />
                                                                            <ext:RecordField Name="Country" />
                                                                        </Fields>
                                                                    </ext:JsonReader>
                                                                </Reader>
                                                            </ax:Store>
                                                        </Store>
                                                        <Template runat="server">
                                                            <Html>
                                                                <tpl for=".">
                                                                    <tpl if="[xindex] == 1">
                                                                        <table class="multiColumns-list">
                                                                            <tr>
                                                                                <th>Street</th>
                                                                                <th>House Number</th>
                                                                                <th>City</th>
                                                                                <th>Country</th>
                                                                            </tr>
                                                                    </tpl>
                                                                    <tr class="list-item">
                                                                        <td style="padding:3px 0px;">{Street}</td>
                                                                        <td>{HouseNumber}</td>
                                                                        <td>{City}</td>
                                                                        <td>{Country}</td>
                                                                    </tr>
                                                                    <tpl if="[xcount-xindex]==0">
                                                                        </table>
                                                                    </tpl>
                                                                </tpl>
                                                            </Html>
                                                        </Template>
                                                    </ax:ComboBox>
                                                    <ext:TextField ID="Name" runat="server" FieldLabel="Name" Text="<%# Model.Name %>" AutoDataBind="true" Width="165" AllowBlank="false" />
                                                    <ext:TextArea ID="Description" runat="server" FieldLabel="Description" Text="<%# Model.Description %>" AutoDataBind="true" Width="165" />
                                                    <ext:RadioGroup FieldLabel="Completed" runat="server">
                                                        <Items>
                                                            <ext:Radio ID="CompletedNo"  Checked="<%# Model.IsCompleted != true %>" BoxLabel="No" runat="server" />
                                                            <ext:Radio ID="CompletedYes" Checked="<%# Model.IsCompleted == true %>" BoxLabel="Yes" runat="server">
                                                                <Listeners>
                                                                    <Check Handler="#{IsCompleted}.setValue((checked ? true : false));" />
                                                                </Listeners>
                                                            </ext:Radio>
                                                        </Items>
                                                    </ext:RadioGroup>
                                                    <ext:RadioGroup FieldLabel="Invoiced" runat="server">
                                                        <Items>
                                                            <ext:Radio ID="InvoicedNo" Checked="<%# Model.IsInvoiced != true %>" BoxLabel="No" runat="server"/>
                                                            <ext:Radio ID="InvoicedYes" Checked="<%# Model.IsInvoiced == true %>" BoxLabel="Yes" runat="server">
                                                                <Listeners>
                                                                    <Check Handler="#{IsInvoiced}.setValue((checked ? true : false));" />
                                                                </Listeners>
                                                            </ext:Radio>
                                                        </Items>
                                                    </ext:RadioGroup>
                                                    <ext:Hidden ID="IsInvoiced" Value="<%# Model.IsInvoiced %>" runat="server" />
                                                    <ext:Hidden ID="IsCompleted" Value="<%# Model.IsCompleted %>" runat="server" />
                                                </Items>
                                            </ax:FieldSet>
                                            <ax:RBox runat="server">
                                                <Items>
                                                    <ax:FieldSet runat="server" Title="Activity Details" Height="216">
                                                        <Items>
                                                            <ax:DateField ID="StartDate" EndDateField="EndDate" FieldLabel="Start Date" SelectedDateFromModel="<%# Model.StartDate %>" Width="128" runat="server" AutoDataBind="true" />
                                                            <ax:DateField ID="EndDate" StartDateField="StartDate" FieldLabel="End Date" SelectedDateFromModel="<%# Model.EndDate %>" Width="128" runat="server" AutoDataBind="true" />
                                                            <ax:ComboBox ID="OwnerID" runat="server" FieldLabel="Owner">
                                                                <SelectedItemModel AutoDataBind="true" Text="<%# Model.OwnerName %>" Value="<%# Model.OwnerID %>" />
                                                                <Store>
                                                                    <ax:Store runat="server" Url="/Settings/BaseTable/GetCompanies" />
                                                                </Store>                                                                       
                                                            </ax:ComboBox>
                                                            <ax:ComboBox ID="ExecuterID" runat="server" FieldLabel="Executer">
                                                                <SelectedItemModel AutoDataBind="true" Text="<%# Model.ExecuterName %>" Value="<%# Model.ExecuterID %>" />
                                                                <Store>
                                                                    <ax:Store runat="server" Url="/Settings/BaseTable/GetCompanies" />
                                                                </Store>
                                                            </ax:ComboBox>                                                    
                                                            <ax:FileUpload ID="DocumentID" runat="server" FieldLabel="Document" SelectedFileFromModel="<%# Model.DocumentID %>" AutoDataBind="true" />
                                                        </Items>
                                                    </ax:FieldSet>
                                                    <ax:FieldSet runat="server" Title="Reminder">
                                                        <Items>
                                                            <ax:DateField ID="RemainderDate" FieldLabel="Reminder" SelectedDateFromModel="<%# Model.RemainderDate %>" Width="128" runat="server" AutoDataBind="true" />
                                                        </Items>
                                                    </ax:FieldSet>
                                                </Items>
                                            </ax:RBox>
                                        </Items>                                    
                                    </ax:HBox>
                                    <ax:HBox runat="server">
                                        <Items>
                                            <ax:FieldSet runat="server" Title="Financial">
                                                <Items>
                                                    <ax:MoneyField ID="Value" FieldLabel="Value" runat="server" MoneyType="Euro" SelectedValueFromModel="<%# Model.Value %>" AutoDataBind="true" />
                                                    <ax:Hidden ID="Value_Value" runat="server" />
                                                    <ext:TextArea ID="InvoiceText" runat="server" FieldLabel="Invoice Text" Text="<%# Model.InvoiceText %>" AutoDataBind="true" Width="165" />
                                                    <ax:ComboBox ID="RelationID" runat="server" FieldLabel="Relation">
                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.Relation %>" Value="<%# Model.RelationID %>" />
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Settings/BaseTable/GetRelations" />
                                                        </Store>                                                                       
                                                    </ax:ComboBox>   

                                                   <ax:ComboBox ID="InvoiceAddressID" runat="server" FieldLabel="Invoice Address" 
                                                        ItemSelector="tr.list-item" DisplayField="Street" ValueField="AddressID">
                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.InvoiceAddress %>" Value="<%# Model.InvoiceAddressID %>" />
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Crm/Address/GetAddressListItem">
                                                                <Reader>
                                                                    <ext:JsonReader IDProperty="AddressID" Root="data" TotalProperty="total">
                                                                        <Fields>
                                                                            <ext:RecordField Name="AddressID" />
                                                                            <ext:RecordField Name="Street" />
                                                                            <ext:RecordField Name="HouseNumber" />
                                                                            <ext:RecordField Name="City" />
                                                                            <ext:RecordField Name="Country" />
                                                                        </Fields>
                                                                    </ext:JsonReader>
                                                                </Reader>
                                                            </ax:Store>
                                                        </Store>
                                                        <Template ID="Template1" runat="server">
                                                            <Html>
                                                                <tpl for=".">
                                                                    <tpl if="[xindex] == 1">
                                                                        <table class="multiColumns-list">
                                                                            <tr>
                                                                                <th>Street</th>
                                                                                <th>House Number</th>
                                                                                <th>City</th>
                                                                                <th>Country</th>
                                                                            </tr>
                                                                    </tpl>
                                                                    <tr class="list-item">
                                                                        <td style="padding:3px 0px;">{Street}</td>
                                                                        <td>{HouseNumber}</td>
                                                                        <td>{City}</td>
                                                                        <td>{Country}</td>
                                                                    </tr>
                                                                    <tpl if="[xcount-xindex]==0">
                                                                        </table>
                                                                    </tpl>
                                                                </tpl>
                                                            </Html>
                                                        </Template>
                                                    </ax:ComboBox> 
                                                </Items>
                                            </ax:FieldSet>
                                            <ax:FieldSet runat="server" Title="Activity Planning" Cls="right-box">
                                                <Items>
                                                    <ext:TextField ID="PlannedHours" runat="server" FieldLabel="Planned Hours" Text="<%# Model.PlannedHours %>" AutoDataBind="true" Width="165" />
                                                    <ax:ComboBox ID="ShiftID" runat="server" FieldLabel="Shift">
                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.ShiftNumber %>" Value="<%# Model.ShiftID %>" />
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Settings/BaseTable/GetShifts" />
                                                        </Store>                                                                       
                                                    </ax:ComboBox>  
                                                </Items>
                                            </ax:FieldSet>
                                        </Items>
                                    </ax:HBox>
                                    
                                    <ext:Hidden ID="TypeID" runat="server" Value="<%# Model.TypeID %>" AutoDataBind="true" />
                                    <ext:Hidden ID="ActivityID" runat="server" Value="<%# Model.ActivityID %>" AutoDataBind="true" />
                                </Items>
                            </ax:FormPanel>
                        </Items>
                    </ax:DetailPanel>
    
        </Items>
    </ax:ViewPort>
    <script type="text/javascript">
        function SetDefaultUIValues() {
            var isNew = ActivityID.value == '0';
            if (isNew) {
                TypeName.setValue('Normal');
                TypeID.setValue('32');
            }
        }
    </script>
</asp:Content>

