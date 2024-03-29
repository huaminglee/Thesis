﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<ax:AccordionLayout ID="AccordionLayout1" runat="server" Animate="true" CheckRoles="true">
    <Items>
        <ax:MenuPanel 
            ID="acCRM" 
            runat="server"  
            Title="CRM"
            Border="false" 
            SaveSelection="false" 
            Cls="white-menu">
            <Menu ID="mnCRM" runat="server">
                <Items>
                    <ax:MenuItem ID="mnAddressList" runat="server" Text="Addresses" Icon="BookAddresses" Module="Address">
                        <CustomConfig>
                            <ext:ConfigItem Name="url" Value="/CRM/Address" Mode="Value" />
                        </CustomConfig>
                    </ax:MenuItem>
                </Items>
                <Listeners>
                    <ItemClick Handler="Ax.AddTab({ title: menuItem.text, url: menuItem.url, icon: menuItem.iconCls, tabName: 'tpMain' });" />
                </Listeners>
            </Menu>
        </ax:MenuPanel>
        <ax:MenuPanel 
            ID="acActivity" 
            runat="server"  
            Title="Activities"
            Border="false" 
            SaveSelection="false" 
            Cls="white-menu">
            <Menu ID="mnActivity" runat="server">
                <Items>
                    <ax:MenuItem ID="mnActivityList" runat="server" Text="Activities" Icon="BookAddresses" Module="Activity">
                        <CustomConfig>
                            <ext:ConfigItem Name="url" Value="/Activity/Activity" Mode="Value" />
                        </CustomConfig>
                    </ax:MenuItem>                    
                </Items>
                <Listeners>
                    <ItemClick Handler="Ax.AddTab({ title: menuItem.text, url: menuItem.url, icon: menuItem.iconCls, tabName: 'tpMain' });" />
                </Listeners>
            </Menu>
        </ax:MenuPanel>
        <ax:MenuPanel 
            ID="acSettings" 
            runat="server"  
            Title="Settings"
            Border="false" 
            SaveSelection="false" 
            Cls="white-menu">
            <Menu ID="mnSettings" runat="server">
                <Items>
                    <ax:MenuItem ID="mnLookups" runat="server" Text="Lookups" Icon="Table">
                        <CustomConfig>
                            <ext:ConfigItem Name="url" Value="/Settings/Lookup" Mode="Value" />
                        </CustomConfig>
                    </ax:MenuItem> 
                    <ax:MenuItem ID="mnUserManagement" runat="server" Text="User Management" Icon="User" Module="UserManagement">
                        <CustomConfig>
                            <ext:ConfigItem Name="url" Value="/Settings/UserManagement" Mode="Value" />
                        </CustomConfig>
                    </ax:MenuItem>    
                    <ax:MenuItem ID="mnRoleManagement" runat="server" Text="Role Management" Icon="User" Module="RoleManagement">
                        <CustomConfig>
                            <ext:ConfigItem Name="url" Value="/Settings/RoleManagement" Mode="Value" />
                        </CustomConfig>
                    </ax:MenuItem>                   
                </Items>
                <Listeners>
                    <ItemClick Handler="Ax.AddTab({ title: menuItem.text, url: menuItem.url, icon: menuItem.iconCls, tabName: 'tpMain' });" />
                </Listeners>
            </Menu>
        </ax:MenuPanel>
         <ax:MenuPanel 
            ID="acFilterResults" 
            runat="server"  
            Title="Filter"
            Border="false" 
            SaveSelection="false" 
            Cls="white-menu"
            Hidden="true">
            <Menu ID="mnFilterResults" runat="server">
                <Items>
                    <ax:MenuItem ID="mnFilter" runat="server" Text="Filter" />
                </Items>
                <Listeners>
                    <ItemClick Handler="Ax.AddTab({ title: menuItem.text, url: menuItem.url, icon: menuItem.iconCls, tabName: 'tpMain' });" />
                </Listeners>
            </Menu>
        </ax:MenuPanel>
    </Items>
</ax:AccordionLayout>