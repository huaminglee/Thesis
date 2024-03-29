﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/List.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Assembly="Axiom.Web.UI" Namespace="Thesis.Web.UI.Controls" TagPrefix="ax" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <ax:ViewPort ID="viewPortList" runat="server" IDMode="Static">
        <ListPage>

                    <ax:GridPanel SelectedFilterColumnIndex="8"
                        ID="grdPnlActivities"
                        runat="server" 
                        Module="Activity"
                        EnableSearch="true"
                        FirstRowSelectOnLoad="true"
                        IDProperty="ActivityID"
                        DeleteUrl="/Activity/Activity/Delete">
                        <DetailPage Title="Name" Url="/Activity/Activity/Detail" />
                        <Store>
                            <ax:Store runat="server" Url="/Activity/Activity/GetByFilter">
                                <BaseParams>
                                    <ext:Parameter Name="sort" Value="Name" />
                                </BaseParams>
                            </ax:Store>
                        </Store>
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
    </ax:ViewPort>
    <script type="text/javascript">
        function FormatNumber(value) {
            var zero = "000000";
            return zero.substring(0, zero.length - value.toString().length) + value.toString();
        }
        function FormatEUMoney(value) {
            if (Ax.IsNullOrEmpty(value)) return value;
            return Ax.EUMoney(value.toString().replace('.', ','));
        }
    </script>
</asp:Content>

