<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <ax:ViewPort ID="viewPortDetail" runat="server" IDMode="Static">
        <Items>
            <ext:Panel runat="server" Border="false">
                <Items>
                    <ext:BorderLayout runat="server">
                        <West Split="false" MarginsSummary="5 0 5 5">
                            <ext:TreePanel 
                                ID="TreePanelLookups" 
                                runat="server" 
                                Width="260"
                                Split="true"
                                AutoScroll="true"
                                Collapsible="false">
                                <TopBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarTextItem runat="server" Text="Filter:" />
                                            <ext:ToolbarSpacer />
                                            <ext:TriggerField ID="FilterTrigger" Width="150" runat="server" EnableKeyEvents="true">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" />
                                                </Triggers>
                            
                                                <Listeners>
                                                    <KeyUp Fn="filterTree" Buffer="250" />
                                                    <TriggerClick Handler="clearFilter();" />
                                                </Listeners>
                                            </ext:TriggerField>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Root>
                                    <ext:TreeNode Text="Root" Expanded="true">
                                        <Nodes>
                                            <ext:TreeNode Text="CRM" Qtip="Crm lookups">
                                                <Nodes>
                                                    <ext:TreeNode NodeID="1" Text="Address Types" Qtip="Address Types" Leaf="true" AutoDataBind="false" />
                                                    <ext:TreeNode NodeID="2" Text="Employee Types" Qtip="Employee Types" Leaf="true" AutoDataBind="false" />
                                                </Nodes>
                                            </ext:TreeNode>
                                        
                                            <ext:TreeNode Text="Activities" Qtip="Activities">
                                                <Nodes>
                                                    <ext:TreeNode NodeID="3" Text="Activity Types" Qtip="Activity Types" Leaf="true" AutoDataBind="false" />
                                                </Nodes>
                                            </ext:TreeNode>
                                        </Nodes>
                                    </ext:TreeNode>
                                </Root>
                                <Listeners>
                                    <Click Handler="if(node.leaf == true) { #{LookupTypeId}.setValue(node.id); #{grdPnlLookups}.store.reload(); }" />
                                </Listeners>
                            </ext:TreePanel>
                        </West>
                        <Center MarginsSummary="5 5 5 0">
                           <ext:Panel runat="server" Border="false">
                                <Items>
                                    <ext:BorderLayout runat="server">
                                        <Center>
                                            <ax:Panel runat="server" SetDefaultSearchPanelSize="false">
                                                <ListPage>
                                                    <ax:GridPanel AutoLoadStore="false" ToolbarOrder="3,0,1,2"
                                                        ID="grdPnlLookups"
                                                        runat="server"
                                                        FirstRowSelectOnLoad="true"
                                                        SingleSelect="true"
                                                        EnableSearch="true"
                                                        IDProperty="LookupId"
                                                        DeleteUrl="/Settings/Lookup/Delete">
                                                        <TopBar>
                                                            <ext:Toolbar runat="server">
                                                                <Items>
                                                                    <ext:Button ID="btnAddgrdPnlLookups" Disabled="true" runat="server" Text="Add" Icon="Add">
                                                                        <Listeners>
                                                                            <Click Handler="
                                                                                var southPanel = #{SouthPanel};
                                                                                if(southPanel.disabled) southPanel.enable();
                                                                                southPanel.autoLoad.url = '/Settings/Lookup/Detail' + '?lookupTypeId=' + #{LookupTypeId}.getValue();
                                                                                southPanel.setTitle('New');
                                                                                southPanel.expand(true);
                                                                                southPanel.doLayout();
                                                                                southPanel.reload();
                                                                            " />
                                                                        </Listeners>
                                                                    </ext:Button>
                                                                </Items>
                                                            </ext:Toolbar>
                                                        </TopBar>
                                                        <Store>
                                                            <ax:Store runat="server" Url="/Settings/Lookup/GetByFilter">
                                                                <BaseParams>
                                                                    <ext:Parameter Name="lookupTypeId" Value="#{LookupTypeId}.getValue()" Mode="Raw" />
                                                                </BaseParams>
                                                                <Listeners>
                                                                    <Load Handler="
                                                                        #{btnAddgrdPnlLookups}.enable();
                                                                        var grd = #{grdPnlLookups};
                                                                        if(grd.selModel.selections.length == 1) {
                                                                            var southPanel = #{SouthPanel};
                                                                            if(southPanel.disabled) southPanel.enable();
                                                                            var row = grd.selModel.selections.items[0];
                                                                            southPanel.autoLoad.url = '/Settings/Lookup/Detail/' + row.id + '?lookupTypeId=' + #{LookupTypeId}.getValue();
                                                                            southPanel.setTitle(Ax.FormatTitle(row.data.Text, 24));
                                                                            southPanel.expand(true);
                                                                            southPanel.doLayout();
                                                                            southPanel.reload();
                                                                        }

                                                                        if(grd.store.data.items.length == 0) {
                                                                            var southPanel = #{SouthPanel};
                                                                            southPanel.collapse();
                                                                            southPanel.disable();
                                                                        }
                                                                    " Buffer="1000" />
                                                                </Listeners>
                                                            </ax:Store>
                                                        </Store>
                                                        <ColumnModel runat="server">
                                                            <Columns> 
                                                                <ext:Column ColumnID="Text" DataIndex="Text" Header="Text" Width="200" />                                            
                                                            </Columns>
                                                        </ColumnModel>
                                                        <Listeners>
                                                            <RowClick Handler="
                                                                var southPanel = #{SouthPanel};
                                                                if(southPanel.disabled) southPanel.enable();
                                                                var row = item.store.data.items[rowIndex];
                                                                southPanel.autoLoad.url = '/Settings/Lookup/Detail/' + row.id + '?lookupTypeId=' + #{LookupTypeId}.getValue();
                                                                southPanel.setTitle(Ax.FormatTitle(row.data.Text, 24));
                                                                southPanel.expand(true);
                                                                southPanel.doLayout();
                                                                southPanel.reload();
                                                            " />                                                            
                                                        </Listeners>
                                                    </ax:GridPanel>
                                                </ListPage>                                            
                                            </ax:Panel>
                                        </Center>
                                        <South>
                                            <ext:Panel 
                                                ID="SouthPanel" 
                                                runat="server" 
                                                Split="true" 
                                                Title="Detail"
                                                Border="true"
                                                Disabled="true"
                                                Collapsible="true"
                                                Collapsed="true"
                                                Height="240">                                                
                                                <AutoLoad 
                                                    ShowMask="true"
                                                    MaskMsg="Loading..."
                                                    Mode="IFrame">
                                                    <Params>
                                                        <ext:Parameter Name="gridPanelID" Value="grdPnlLookups" Mode="Value" />
                                                    </Params>
                                                </AutoLoad>
                                                <Listeners>
                                                    <BeforeExpand Handler="return this.title == 'New' || #{grdPnlLookups}.selModel.selections.length > 0; " />
                                                </Listeners>
                                            </ext:Panel>
                                        </South>
                                    </ext:BorderLayout>
                                </Items>
                            </ext:Panel>
                        </Center>       
                    </ext:BorderLayout>
                </Items>
            </ext:Panel>    
        </Items>
    </ax:ViewPort>
    <ext:Hidden ID="LookupTypeId" Text="0" runat="server" />
     <script type="text/javascript">
         var filterTree = function (el, e) {
             var tree = TreePanelLookups,
                text = this.getRawValue();

             tree.clearFilter();

             if (Ext.isEmpty(text, false)) {
                 return;
             }

             if (e.getKey() === Ext.EventObject.ESC) {
                 clearFilter();
             } else {
                 var re = new RegExp(".*" + text + ".*", "i");

                 tree.filterBy(function (node) {
                     return re.test(node.text);
                 });
             }
         };

         var clearFilter = function () {
             var field = FilterTrigger,
                tree = TreePanelLookups;

             field.setValue("");
             tree.clearFilter();
             tree.getRootNode().collapseChildNodes(true);
             tree.getRootNode().ensureVisible();
         };
    </script>
</asp:Content>
