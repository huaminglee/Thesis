<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Detail.Master" Inherits="System.Web.Mvc.ViewPage<Crm.AddressViewModel>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Addresses</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">  
    <ext:ViewPort ID="viewPortDetail" runat="server" Layout="fit" IDMode="Static">
        <Items>
            <ext:TabPanel ID="AddressPanel" runat="server" Border="false" LayoutOnTabChange="true">
                <Items>
                    <ax:DetailPanel 
                        ID="pnlAddressDetail" 
                        Title="Detail" 
                        Module="Address"
                        FormPanelID="AddressForm"
                        IDProperty="ID"
                        Url="/Crm/Address/Detail"
                        DeleteUrl="/Crm/Address/Delete"
                        SetDefaultUIValuesFn="SetDefaultUIValues"
                        CheckDirty="true"                        
                        runat="server">
                        <Items>
                            <ax:FormPanel ID="AddressForm" Url="/Crm/Address/Save" runat="server">
                                <Items>
                                    <ax:HBox runat="server">
                                        <Items>
                                            <ax:LBox runat="server">
                                                <Items>
                                                    <ax:FieldSet runat="server" Title="General">
                                                        <Items>
                                                            <ax:CompositeField FieldLabel="Address Type" runat="server">
                                                                <Items>
                                                                    <ax:ComboBox ID="AddressType" runat="server" AutoFocusOnAfterRender="true">
                                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.AddressTypeName %>" Value="<%# Model.AddressType %>" />
                                                                        <Store>
                                                                            <ax:Store runat="server" Url="/Settings/Lookup/GetLookups">
                                                                                <BaseParams>
                                                                                    <ext:Parameter Name="LookupTypeId" Value="<%# (int)Settings.LookupType.AddressType %>" AutoDataBind="true" />
                                                                                </BaseParams>
                                                                            </ax:Store>
                                                                        </Store>
                                                                    </ax:ComboBox>
                                                                    <ax:Checkbox ID="IsActive" runat="server" IndicatorText="Active" AutoDataBind="true" Checked="<%# Model.IsActive == true %>" />
                                                                    <ax:CHidden Name="IsActive" runat="server" />
                                                                </Items>
                                                            </ax:CompositeField>
                                                            <ax:CompositeField FieldLabel="Address" runat="server">
                                                                <Items>
                                                                    <ext:TextField ID="Street" AutoDataBind="true" Text="<%# Model.Street %>" runat="server">
                                                                        <Listeners>
                                                                            <Blur Handler="codeAddress();" />
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                    <ext:TextField ID="HouseNumber" AutoDataBind="true" Text="<%# Model.HouseNumber %>" MaxLength="255" Width="50" runat="server">
                                                                        <Listeners>
                                                                            <Blur Handler="codeAddress();" />
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                    <ext:TextField ID="Addition" AutoDataBind="true" Text="<%# Model.Addition %>" Width="50" runat="server">
                                                                        <Listeners>
                                                                            <Blur Handler="codeAddress();" />
                                                                        </Listeners>
                                                                    </ext:TextField>
                                                                </Items>
                                                            </ax:CompositeField>
                                                            <ax:CompositeField FieldLabel="Postal Code / City" runat="server">
                                                                <Items>
                                                                    <ext:TextField ID="Postalcode" AllowBlank="false" AutoDataBind="true" Text="<%# Model.PostalCode %>" runat="server" Width="80" MaxLength="255" />
                                                                    <ax:ComboBox ID="CityID" runat="server" Width="153" ParentID="CountryID">
                                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.CityName %>" Value="<%# Model.CityID %>" />
                                                                        <Store>
                                                                            <ax:Store runat="server" Url="/Settings/BaseTable/GetCities">                                                                                        
                                                                                <BaseParams>
                                                                                    <ext:Parameter Name="countryID" Value="#{CountryID}.getValue()" Mode="Raw"/>
                                                                                </BaseParams>
                                                                            </ax:Store>
                                                                        </Store>
                                                                        <Listeners>
                                                                            <Blur Handler="codeAddress();" />
                                                                        </Listeners>
                                                                    </ax:ComboBox>
                                                                </Items>
                                                            </ax:CompositeField>
                                                            <ax:ComboBox ID="CountryID" runat="server" FieldLabel="Country" ChildID="CityID">
                                                                <SelectedItemModel AutoDataBind="true" Text="<%# Model.CountryName %>" Value="<%# Model.CountryID %>" />
                                                                <Store>
                                                                    <ax:Store runat="server" Url="/Settings/BaseTable/GetCountries" />
                                                                </Store>  
                                                                <Listeners>
                                                                    <Blur Handler="codeAddress();" />
                                                                </Listeners>                                                                     
                                                            </ax:ComboBox>
                                                            <ext:TextField AllowBlank="false" ID="Phone" Text="<%# Model.Phone %>" AutoDataBind="true" FieldLabel="Phone" runat="server" MaxLength="255" />
                                                            <ext:TextField ID="Fax" Text="<%# Model.Fax %>" AutoDataBind="true" FieldLabel="Fax" runat="server" MaxLength="255" />
                                                        </Items>
                                                    </ax:FieldSet>
                                                    <ax:FieldSet runat="server" Title="Details">
                                                        <Items>
                                                            <ext:TextArea ID="DetailDescription" AllowBlank="false" Text="<%# Model.DetailDescription %>" AutoDataBind="true" FieldLabel="Description" Width="170" runat="server" MaxLength="255" />
                                                            <ext:TextField ID="KeyPersonName" Text="<%# Model.KeyPersonName %>" AutoDataBind="true" FieldLabel="Key Person Name" runat="server" MaxLength="255" />
                                                            <ext:TextField ID="KeyPersonPhone" Text="<%# Model.KeyPersonPhone %>" AutoDataBind="true" FieldLabel="Key Person Phone" runat="server" MaxLength="255" />
                                                        </Items>
                                                    </ax:FieldSet>
                                                </Items>
                                            </ax:LBox>
                                             <ax:FieldSet ID="FieldSet1" runat="server" Cls="right-box" Title="Map" Width="450" Height="353">
                                                <Items>
                                                    <ext:Panel ID="map_canvas" runat="server" Border="false" Width="428" Height="322" StyleSpec="width:428px; height:322px;">
                                                    </ext:Panel>
                                                </Items>
                                            </ax:FieldSet>
                                        </Items>
                                    </ax:HBox>
                                    <ax:HBox runat="server">
                                        <Items>
                                            <ax:FieldSet runat="server" Height="430" Title="Work Address Details">
                                                <Items>
                                                    <ax:ComboBox ID="Electriciteit" runat="server" FieldLabel="Electricity">
                                                        <SelectedItemModel AutoDataBind="true" Text="<%# Model.ElectriciteitName %>" Value="<%# Model.Electriciteit %>" />
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Settings/BaseTable/GetElectriciteites" />
                                                        </Store>
                                                    </ax:ComboBox>
                                                    <ext:TextArea ID="ExtraLetterText" Text="<%# Model.ExtraLetterText %>" AutoDataBind="true" FieldLabel="ExtraLetterText" runat="server" Width="200" Height="100" MaxLength="255" />
                                                    <ext:TextArea ID="Location" Text="<%# Model.Location %>" AutoDataBind="true" FieldLabel="Location" runat="server" Width="200" Height="100" MaxLength="255" />
                                                    <ext:TextArea ID="Information" Text="<%# Model.Information %>" AutoDataBind="true" FieldLabel="Information" runat="server" Width="200" Height="100" MaxLength="255" />
                                                </Items>
                                            </ax:FieldSet>
                                            <ax:RBox runat="server">
                                                <Items>
                                                    <ax:FieldSet runat="server" Title="Work Address Financials">
                                                        <Items>
                                                            <ext:TextField Disabled="true" FieldLabel="Planned Man Hours" ID="PlannedManHours" Text="<%# Model.PlannedManHours %>" runat="server" AutoDataBind="true" />
                                                            <ext:TextField Disabled="true" FieldLabel="Total Surface M2" ID="TotalSurfaceM2" Text="<%# Model.TotalSurfaceM2 %>" runat="server" AutoDataBind="true" />
                                                            <ext:TextField Disabled="true" FieldLabel="Contract Value" ID="ContractValue" Text="<%# Model.ContractValue %>" runat="server" AutoDataBind="true" />
                                                            <ext:TextField Disabled="true" FieldLabel="Invoiced Work Address" ID="InvoicedWorkAddress" Text="<%# Model.InvoicedWorkAddress %>" runat="server" AutoDataBind="true" />
                                                            <ext:TextField Disabled="true" FieldLabel="To Be Invoiced" ID="ToBeInvoiced" Text="<%# Model.ToBeInvoiced %>" runat="server" AutoDataBind="true" />
                                                            <ax:DateField ID="LastInvoiceDate" FieldLabel="Last Invoice Date" SelectedDateFromModel="<%# Model.LastInvoiceDate %>" Width="128" runat="server" AutoDataBind="true" />
                                                        </Items>
                                                    </ax:FieldSet>                                                            
                                                    <ax:FieldSet runat="server" Title="Roofs">
                                                        <Items>
                                                            <ax:GridPanel Height="200"
                                                                ID="grdPnlRoofs" 
                                                                runat="server" 
                                                                AutoExpandColumn="SurfaceAreaM2"
                                                                AutoLoadStore="false"
                                                                IDProperty="RoofID"
                                                                DeleteUrl="/Crm/Roof/Delete">
                                                                <Store>
                                                                    <ax:Store runat="server" Url="/Crm/Roof/GetByFilter" AutoLoad="false">
                                                                        <BaseParams>
                                                                            <ext:Parameter Name="addressID" Value="#{ID}.getValue()" Mode="Raw" />
                                                                        </BaseParams>
                                                                    </ax:Store>
                                                                </Store>
                                                                <ColumnModel ID="cmRoofs" runat="server">
                                                                    <Columns> 
                                                                        <ext:Column ColumnID="Name" DataIndex="Name" Header="Name" />
                                                                        <ext:Column ColumnID="SurfaceAreaM2" DataIndex="SurfaceAreaM2" Header="Surface m2" />
                                                                    </Columns>
                                                                </ColumnModel>
                                                            </ax:GridPanel>
                                                        </Items>
                                                    </ax:FieldSet>
                                                </Items>
                                            </ax:RBox>
                                        </Items>
                                    </ax:HBox>
                                    <ax:FieldSet runat="server" Title="Related Relations">
                                        <ListPage>
                                            <ax:GridPanel Height="200" 
                                                ID="grdPnlRelatedRelations" 
                                                runat="server" 
                                                AutoExpandColumn="RelationName"
                                                IDProperty="RelationInAddressesID"
                                                DeleteUrl="/Crm/RelationInAddress/Delete">
                                                <DetailPage WindowID="RelationDetailWindow" Title="RelationName" Url="/Crm/RelationInAddress/Detail">
                                                    <BaseParams>
                                                        <ext:Parameter Name="addressID" Value="#{ID}.getValue()" Mode="Raw" />
                                                    </BaseParams>
                                                </DetailPage>
                                                <Store>
                                                    <ax:Store runat="server" Url="/Crm/RelationInAddress/GetByFilter">
                                                        <BaseParams>
                                                            <ext:Parameter Name="addressID" Value="#{ID}.getValue()" Mode="Raw" />
                                                        </BaseParams>
                                                    </ax:Store>
                                                </Store>
                                                <ColumnModel ID="cmRelatedRelations" runat="server">
                                                    <Columns> 
                                                        <ext:Column ColumnID="RelationName" DataIndex="RelationName" Header="Relation" />
                                                    </Columns>
                                                </ColumnModel>
                                            </ax:GridPanel>
                                        </ListPage>
                                    </ax:FieldSet>
                                    <ext:Hidden ID="ID" runat="server" Value="<%# Model.AddressID %>" AutoDataBind="true" />
                                </Items>
                            </ax:FormPanel>
                            
                        </Items>
                        <Listeners>
                            <AfterRender Handler="loadScript();" />
                        </Listeners>
                    </ax:DetailPanel>
                    <ax:Panel ID="ActivityPanel" runat="server" Title="Activities">
                        <ListPage>
                            <ax:GridPanel SelectedFilterColumnIndex="8"
                                ID="grdPnlRelatedActivities"
                                runat="server" 
                                LoadOnActive="true"
                                EnableSearch="true"
                                FirstRowSelectOnLoad="true"
                                AutoExpandColumn="Name"
                                IDProperty="ActivityID"
                                DeleteUrl="/Activity/Activity/Delete">
                                <DetailPage Title="Name" Url="/Activity/Activity/Detail" />
                                <Store>
                                    <ax:Store runat="server" Url="/Activity/Activity/GetByFilter" />
                                </Store>
                                <FilterParams>
                                    <ext:Parameter Name="AddressID" Value="#{ID}.getValue()" Mode="Raw" />
                                </FilterParams>
                                <ColumnModel runat="server">
                                    <Columns> 
                                        <ax:Column ColumnID="ActivityID" DataIndex="ActivityID" DataType="Int" Header="Number">
                                            <Renderer Fn="FormatNumber" />
                                        </ax:Column>
                                        <ext:Column ColumnID="TypeName" DataIndex="TypeName" Header="Type" />
                                        <ext:Column ColumnID="Name" DataIndex="Name" Header="Name" />
                                        <ax:Column ColumnID="Value" DataIndex="Value" DataType="Float" Header="Value" Align="Right">
                                            <Renderer Fn="FormatEUMoney" />
                                        </ax:Column>
                                        <ext:DateColumn ColumnID="StartDate" DataIndex="StartDate" Header="Start Date" />
                                        <ext:DateColumn ColumnID="EndDate" DataIndex="EndDate" Header="End Date" />
                                        <ext:Column ColumnID="OwnerName" DataIndex="OwnerName" Header="Owner" />
                                        <ext:Column ColumnID="ExecuterName" DataIndex="ExecuterName" Header="Executer" />                                                                                
                                        <ext:CheckColumn ColumnID="IsCompleted" DataIndex="IsCompleted" Header="Completed" />
                                        <ext:CheckColumn ColumnID="IsInvoiced" DataIndex="IsInvoiced" Header="Invoiced" />                                                
                                    </Columns>
                                </ColumnModel>
                            </ax:GridPanel>
                        </ListPage>
                    </ax:Panel>                    
                </Items>
            </ext:TabPanel>
            <ext:Window ID="RelationDetailWindow" runat="server" Width="500" Height="400" Hidden="true" Closable="false" Modal="true" IsDestroyable="false">
                <AutoLoad ShowMask="true" Mode="IFrame" />
                <Listeners>
                    <Hide Handler="#{grdPnlRelatedRelations}.store.reload();" />
                </Listeners>
            </ext:Window>
        </Items>
    </ext:ViewPort>
    <script type="text/javascript">
        function SetDefaultUIValues() {
            var isNew = ID.value == '0';
            
            var btnAddRelation = Ext.getCmp('btnAddgrdPnlRelatedRelations');
            if (btnAddRelation) btnAddRelation.setDisabled(isNew);

            var btnAddRoof = Ext.getCmp('btnAddgrdPnlRoofs');
            if (btnAddRoof) btnAddRoof.setDisabled(isNew);
        }

        function FormatNumber(value) {
            var zero = "000000";
            return zero.substring(0, zero.length - value.toString().length) + value.toString();
        }

        function FormatEUMoney(value) {
            if (Ax.IsNullOrEmpty(value)) return value;
            return Ax.EUMoney(value.toString().replace('.', ','));
        }
    </script>
    <script type="text/javascript">
        var geocoder;
        var map;
        var markersArray = [];

        function initialize() {
            geocoder = new google.maps.Geocoder();
            var myOptions = {
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
            codeAddress();
        }

        function codeAddress() {
            var address = '';
            if (!Ax.IsNullOrEmpty(Street.getValue())) address += Street.getValue() + ', ';
            if (!Ax.IsNullOrEmpty(HouseNumber.getValue())) address += HouseNumber.getValue() + ', ';
            if (!Ax.IsNullOrEmpty(Addition.getValue())) address += Addition.getValue() + ', ';
            if (!Ax.IsNullOrEmpty(Postalcode.getValue())) address += Postalcode.getValue() + ', ';
            if (!Ax.IsNullOrEmpty(CityID.getRawValue())) address += CityID.getRawValue() + ', ';
            if (!Ax.IsNullOrEmpty(CountryID.getRawValue())) address += CountryID.getRawValue() + ', ';

            if (address == '') {
                map.setCenter(new google.maps.LatLng(52.373056, 4.892222));
            }
            else {
                address = address.substring(0, address.length - 2);

                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        clearOverlays();
                        map.setCenter(results[0].geometry.location);
                        var marker = new google.maps.Marker({
                            map: map,
                            position: results[0].geometry.location
                        });
                        map.setZoom(16);
                        markersArray.push(marker);
                    } else {
                        //alert("Geocode was not successful for the following reason: " + status);
                    }
                });
            }
        }

        function clearOverlays() {
            if (markersArray) {
                for (i in markersArray) {
                    if (markersArray[i].setMap)
                        markersArray[i].setMap(null);
                }
                markersArray.length = 0;
            }
        }

        function loadScript() {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://maps.googleapis.com/maps/api/js?sensor=false&callback=initialize&language=<%: System.Threading.Thread.CurrentThread.CurrentCulture.Name %>";
            document.body.appendChild(script);
        }        
    </script>
</asp:Content>
