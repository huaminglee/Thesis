using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Controls;

namespace Thesis.Web.UI.Helpers
{
    internal static class UIHelper
    {
        public static Ext.Net.RecordFieldType GetFieldType(Ext.Net.ColumnBase column)
        {
            string columnType = column.GetType().Name;

            switch (columnType)
            {
                case "DateColumn":
                    return Ext.Net.RecordFieldType.Date;
                case "NumberColumn":
                    return Ext.Net.RecordFieldType.Float;
                case "CheckColumn":
                    return Ext.Net.RecordFieldType.Boolean;
                default:
                    return Ext.Net.RecordFieldType.Auto;
            }
        }

        public static Ext.Net.Component GridComponent(GridPanel grid)
        {
            return GridComponent(grid, false);
        }

        public static Ext.Net.Component GridComponent(GridPanel grid, bool isParentPanel)
        {
            if (grid.EnableSearch)
            {
                return Thesis.Web.UI.Helpers.UIHelper.SearchPanel(grid, isParentPanel);
            }
            else
                return grid;
        }

        public static Ext.Net.BorderLayout SearchPanel(GridPanel grid, bool isParentPanel)
        {
            string gridID = grid.ID;

            #region Main Panel

            Ext.Net.Panel mainPanel = new Ext.Net.Panel()
            {
                ID = string.Format("panel_search_{0}", gridID),
                Border = false,
                Collapsed = true,
                Collapsible = true,
                CollapseMode = grid.SearchCollapseMode,
                Height = 140,
                Title = "Filter"
            };

            if (grid.ToggleSearch)
            {
                mainPanel.Listeners.Collapse.Handler = string.Format("var btnFilterToggle = #{{btnToogleSearch_{0}}}; if(btnFilterToggle && btnFilterToggle.pressed) btnFilterToggle.toggle();", gridID);
                mainPanel.Listeners.Expand.Handler = string.Format("var btnFilterToggle = #{{btnToogleSearch_{0}}}; if(btnFilterToggle && !btnFilterToggle.pressed) btnFilterToggle.toggle();", gridID);
            }

            #region Container Search

            Ext.Net.Panel searchPanel = new Ext.Net.Panel()
            {
                ID = string.Format("container_search_{0}", gridID),
                Border = false,
                Layout = "hbox",
                StyleSpec = "padding: 10px;"
            };

            #region SelectBox Columns

            Ext.Net.SelectBox selectboxColumns = new Ext.Net.SelectBox()
            {
                ID = string.Format("SearchBox_{0}", gridID),
                EmptyText = "Select a column",
                AllowBlank = false
            };

            var columns = grid.ColumnModel.Columns;
            Column axiomColumn = null;
            ColumnBaseType columnType = ColumnBaseType.String;

            foreach (var column in columns)
            {
                if (column is Column)
                {
                    axiomColumn = column as Column;
                    if (axiomColumn.DataType == 0) axiomColumn.DataType = ColumnBaseType.String;
                    selectboxColumns.Items.Add(new Ext.Net.ListItem { Text = axiomColumn.Header, Value = string.Format("{0}_{1}", axiomColumn.DataIndex, axiomColumn.DataType.ToString().ToLower(new System.Globalization.CultureInfo("en-US"))) });
                }
                else
                {
                    Ext.Net.RecordFieldType fieldType = GetFieldType(column);
                    switch (fieldType)
                    {
                        case Ext.Net.RecordFieldType.Auto:
                            columnType = ColumnBaseType.String;
                            break;
                        case Ext.Net.RecordFieldType.Boolean:
                            columnType = ColumnBaseType.Bool;
                            break;
                        case Ext.Net.RecordFieldType.Date:
                            columnType = ColumnBaseType.Date;
                            break;
                        case Ext.Net.RecordFieldType.Float:
                            columnType = ColumnBaseType.Float;
                            break;
                        case Ext.Net.RecordFieldType.Int:
                            columnType = ColumnBaseType.Int;
                            break;
                        case Ext.Net.RecordFieldType.String:
                            columnType = ColumnBaseType.String;
                            break;
                        default:
                            columnType = ColumnBaseType.String;
                            break;
                    }

                    selectboxColumns.Items.Add(new Ext.Net.ListItem { Text = column.Header, Value = string.Format("{0}_{1}", column.DataIndex, columnType.ToString().ToLower(new System.Globalization.CultureInfo("en-US"))) });
                }
            }

            selectboxColumns.Listeners.Select.Handler = string.Format(@"if(#{{compositeField_whereCondition_{0}}}.hidden)
                                                                        #{{compositeField_whereCondition_{0}}}.show();

                                                                        #{{container_search_{0}}}.items.each(function (b) {{ if (b.xtype == 'container') b.hide(); }});
                                                                        var type = this.value.split('_')[1];
                                                                        if(type == 'int' || type == 'float') {{ 
                                                                            var isFloat = type == 'float';
                                                                            Ax.DisableDecimals(#{{numberfield_start_numeric_{0}}}, isFloat);
                                                                            Ax.DisableDecimals(#{{numberfield_end_numeric_{0}}}, isFloat);
                                                                            type = 'numeric'; 
                                                                        }}
                                                                        Ext.getCmp('container_' + type + '_{0}').show();", gridID);

            if (isParentPanel)
                selectboxColumns.Listeners.Select.Handler += string.Format("if(type == 'numeric') {{ #{{numberfield_start_numeric_{0}}}.reset(); }}", gridID);

            selectboxColumns.SelectedIndex = grid.SelectedFilterColumnIndex;

            searchPanel.Items.Add(selectboxColumns);

            #endregion

            #region First Column Type

            string firstColumnType = selectboxColumns.Items.Count > 0 ? selectboxColumns.Items[selectboxColumns.SelectedIndex].Value.Split('_')[1] : string.Empty;
            bool hasFirstColumn = firstColumnType != string.Empty;

            #endregion

            #region Container String

            Ext.Net.Container container_string = new Ext.Net.Container()
            {
                ID = string.Format("container_string_{0}", gridID),
                StyleSpec = "left:165px;"
            };

            if (!(hasFirstColumn && firstColumnType == "string"))
            {
                container_string.Listeners.AfterRender.Handler = "item.el.setStyle('margin-left', Ext.isIE ? '14px' : '6px'); item.hide();";
            }
            else
            {
                container_string.StyleSpec += "margin-left:6px;";
                container_string.Listeners.AfterRender.Handler = "item.show();";
            }

            container_string.Listeners.BeforeShow.Handler = string.Format("#{{selectbox_string_filterCondition_{0}}}.selectFirst();", gridID);
            container_string.Listeners.BeforeShow.Delay = 100;

            container_string.Listeners.Hide.Handler = string.Format("#{{selectbox_string_filterCondition_{0}}}.reset(); #{{textField_string_{0}}}.reset();", gridID);

            CompositeField compositeField_string = new CompositeField() {
                ID = string.Format("compositeField_string_{0}", gridID),
                Width = 500
            };

            if (hasFirstColumn && firstColumnType == "string")
            {
                compositeField_string.Listeners.AfterRender.Handler = "item.el.first().setStyle('height', '23px');";
                compositeField_string.Listeners.AfterRender.Delay = 150;
            }

            Ext.Net.SelectBox selectBox_string = new Ext.Net.SelectBox()
            {
                ID = string.Format("selectbox_string_filterCondition_{0}", gridID),
                AllowBlank = false,
                Width = 107
            };

            selectBox_string.Items.AddRange(new List<Ext.Net.ListItem>() { 
                new Ext.Net.ListItem() { Text = "Starts With", Value="StartsWith" },
                new Ext.Net.ListItem() { Text = "Ends With", Value="EndsWith" },
                new Ext.Net.ListItem() { Text = "Contains", Value="Contains" },
                new Ext.Net.ListItem() { Text = "Equals", Value="Equals" },
                new Ext.Net.ListItem() { Text = "Not Equal", Value="NotEqual" }
            });

            selectBox_string.SelectedIndex = 0;

            Ext.Net.TextField textfield_string = new Ext.Net.TextField()
            {
                ID = string.Format("textField_string_{0}", gridID),
                StyleSpec = "margin-left:8px;",
                AllowBlank = false,
                MinLength = 1,
                EmptyText = "Type a filter"
            };

            compositeField_string.Items.Add(selectBox_string);
            compositeField_string.Items.Add(textfield_string);

            container_string.Items.Add(compositeField_string);

            searchPanel.Items.Add(container_string);

            #endregion

            #region Container DateTime

            Ext.Net.Container container_date = new Ext.Net.Container() {
                ID = string.Format("container_date_{0}", gridID),
                StyleSpec = "left:165px;"
            };

            if (!(hasFirstColumn && firstColumnType == "date"))
            {
                container_date.Listeners.AfterRender.Handler = "item.el.setStyle('margin-left', Ext.isIE ? '14px' : '6px'); item.hide();";
            }
            else
            {
                container_date.StyleSpec += "margin-left:6px;";
                container_date.Listeners.AfterRender.Handler = "item.show();";
            }

            container_date.Listeners.BeforeShow.Handler = string.Format("#{{selectbox_date_filterCondition_{0}}}.selectFirst();", gridID);
            container_date.Listeners.BeforeShow.Delay = 100;

            container_date.Listeners.Hide.Handler = string.Format(@"#{{selectbox_date_filterCondition_{0}}}.reset();
                                                                    #{{datefield_start_filterCondition_{0}}}.reset();
                                                                    #{{datefield_start_filterCondition_{0}}}.setMaxValue(null); 
                                                                    #{{datefield_end_filterCondition_{0}}}.reset();
                                                                    #{{datefield_end_filterCondition_{0}}}.allowBlank = true;
                                                                    #{{panel_between_date_{0}}}.hide();", gridID);

            CompositeField compositeField_date = new CompositeField() {
                ID = string.Format("compositeField_date_{0}", gridID),
                Width = 500
            };

            if (hasFirstColumn && firstColumnType == "date")
            {
                compositeField_date.Listeners.AfterRender.Handler = "item.el.first().setStyle('height', '23px');";
                compositeField_date.Listeners.AfterRender.Delay = 200;
            }

            Ext.Net.SelectBox selectBox_date = new Ext.Net.SelectBox() {
                ID = string.Format("selectbox_date_filterCondition_{0}", gridID),
                AllowBlank = false,
                Width=107
            };

            selectBox_date.Items.AddRange(new List<Ext.Net.ListItem>() { 
                new Ext.Net.ListItem() { Text = "Equals", Value = "Equals" },
                new Ext.Net.ListItem() { Text = "Not Equal", Value="NotEqual" },
                new Ext.Net.ListItem() { Text = "Greater Than", Value = "Greater" },
                new Ext.Net.ListItem() { Text = "Less Than", Value = "Less" },
                new Ext.Net.ListItem() { Text = "Between", Value = "Between" }
            });

            selectBox_date.SelectedIndex = 0;

            selectBox_date.Listeners.Select.Handler = string.Format(@"var selectedValue = this.getValue();
                                                                      var isBetween = selectedValue == 'Between';
                                                                      panel_between_date_{0}[isBetween ? 'show' : 'hide']();
                                                                      #{{datefield_end_filterCondition_{0}}}.allowBlank = !isBetween;
                                                                      if(isBetween) 
                                                                          #{{datefield_end_filterCondition_{0}}}.reset();
                                                                      else
                                                                          #{{datefield_start_filterCondition_{0}}}.setMaxValue(null);", gridID);

            DateField dateField_start = new DateField() {
                ID = string.Format("datefield_start_filterCondition_{0}", gridID),
                EndDateField = string.Format("datefield_end_filterCondition_{0}", gridID),
                AllowBlank = false,
                EmptyText = "Select a start date",
                Width = 128,
                StyleSpec = "margin-left:8px;"
            };

            Ext.Net.Panel panel_between_date = new Ext.Net.Panel() {
                ID = string.Format("panel_between_date_{0}", gridID),
                Border = false,
                Hidden = true,
                StyleSpec = "padding-left:13px;"
            };

            CompositeField compositeField_between_date = new CompositeField() {
                ID = string.Format("compositeField_between_date_{0}", gridID),
                Width = 200,
            };

            Ext.Net.DisplayField displayField_date_and = new Ext.Net.DisplayField() {
                ID = string.Format("displayfield_date_and_{0}", gridID),
                Text = "and",
                StyleSpec = "margin-top:4px;"
            };

            DateField dateField_end = new DateField() {
                ID = string.Format("datefield_end_filterCondition_{0}", gridID),
                StartDateField = string.Format("datefield_start_filterCondition_{0}", gridID),
                EmptyText = "Select a end date",
                Width = 128,
                StyleSpec = "margin-left:6px;"
            };

            compositeField_between_date.Items.Add(displayField_date_and);
            compositeField_between_date.Items.Add(dateField_end);

            panel_between_date.Items.Add(compositeField_between_date);


            compositeField_date.Items.Add(selectBox_date);
            compositeField_date.Items.Add(dateField_start);
            compositeField_date.Items.Add(panel_between_date);

            container_date.Items.Add(compositeField_date);

            searchPanel.Items.Add(container_date);

            #endregion

            #region Container Numeric

            Ext.Net.Container container_numeric = new Ext.Net.Container() {
                ID = string.Format("container_numeric_{0}", gridID),
                StyleSpec = "left:165px;"
            };

            if (!(hasFirstColumn && (firstColumnType == "float" || firstColumnType == "int")))
            {
                container_numeric.Listeners.AfterRender.Handler = "item.el.setStyle('margin-left', Ext.isIE ? '14px' : '6px'); item.hide();";
            }
            else
            {
                container_numeric.StyleSpec += "margin-left:6px;";
                container_numeric.Listeners.AfterRender.Handler = "item.show();";
            }

            container_numeric.Listeners.BeforeShow.Handler = string.Format("#{{selectbox_numeric_filterCondition_{0}}}.selectFirst();", gridID);
            container_numeric.Listeners.BeforeShow.Delay = 100;

            container_numeric.Listeners.Hide.Handler = string.Format(@"#{{selectbox_numeric_filterCondition_{0}}}.reset();
                                                                       #{{numberfield_start_numeric_{0}}}.reset();
                                                                       #{{numberfield_start_numeric_{0}}}.setMaxValue(null);
                                                                       #{{numberfield_end_numeric_{0}}}.reset();
                                                                       #{{numberfield_end_numeric_{0}}}.allowBlank = true;
                                                                       #{{panel_between_numeric_{0}}}.hide();", gridID);

            CompositeField compositeField_numeric = new CompositeField() {
                ID = string.Format("compositeField_numeric_{0}", gridID),
                Width = 500
            };

            if (hasFirstColumn && (firstColumnType == "float" || firstColumnType == "int"))
            {
                compositeField_numeric.Listeners.AfterRender.Handler = "item.el.first().setStyle('height', '23px');";
                compositeField_numeric.Listeners.AfterRender.Delay = 150;
            }

            Ext.Net.SelectBox selectBox_numeric = new Ext.Net.SelectBox() {
                ID = string.Format("selectbox_numeric_filterCondition_{0}", gridID),
                AllowBlank = false,
                Width = 107
            };

            selectBox_numeric.Items.AddRange(new List<Ext.Net.ListItem>() { 
                new Ext.Net.ListItem() { Text = "Equals", Value = "Equals" },
                new Ext.Net.ListItem() { Text = "Not Equal", Value="NotEqual" },
                new Ext.Net.ListItem() { Text = "Greater Than", Value = "Greater" },
                new Ext.Net.ListItem() { Text = "Less Than", Value = "Less" },
                new Ext.Net.ListItem() { Text = "Between", Value = "Between" }
            });

            selectBox_numeric.SelectedIndex = 0;

            selectBox_numeric.Listeners.Select.Handler = string.Format(@"var selectedValue = this.getValue();
                                                                         var isBetween = selectedValue == 'Between';
                                                                         panel_between_numeric_{0}[isBetween ? 'show' : 'hide']();
                                                                         #{{numberfield_end_numeric_{0}}}.allowBlank = !isBetween;
                                                                         if(isBetween) 
                                                                             #{{numberfield_end_numeric_{0}}}.reset();
                                                                         else {{
                                                                             #{{numberfield_start_numeric_{0}}}.setMaxValue(null);
                                                                             if(#{{numberfield_start_numeric_{0}}}.getValue() != '') #{{numberfield_start_numeric_{0}}}.validate();
                                                                         }}", gridID);

            NumberField numberField_start = new NumberField() {
                ID = string.Format("numberfield_start_numeric_{0}", gridID),
                EndNumberField = string.Format("numberfield_end_numeric_{0}", gridID),
                AllowBlank = false,
                EmptyText = "Type a filter",
                StyleSpec = "margin-left:8px;text-align:right;"
            };

            Ext.Net.Panel panel_between_numeric = new Ext.Net.Panel() {
                ID = string.Format("panel_between_numeric_{0}", gridID),
                Border = false,
                Hidden = true,
                StyleSpec = "padding-left: 13px;"
            };

            CompositeField compositeField_between_numeric = new CompositeField() {
                ID = string.Format("compositeField_between_numeric_{0}", gridID),
                Width = 200
            };

            Ext.Net.DisplayField displayField_numeric_and = new Ext.Net.DisplayField() {
                ID = string.Format("displayfield_numeric_and_{0}", gridID),
                Text = "and",
                StyleSpec = "margin-top:4px;"
            };

            NumberField numberField_end = new NumberField() {
                ID = string.Format("numberfield_end_numeric_{0}", gridID),
                StartNumberField = string.Format("numberfield_start_numeric_{0}", gridID),
                EmptyText = "Type a filter",
                StyleSpec = "margin-left:6px;text-align:right;"
            };

            compositeField_between_numeric.Items.Add(displayField_numeric_and);
            compositeField_between_numeric.Items.Add(numberField_end);

            panel_between_numeric.Items.Add(compositeField_between_numeric);

            compositeField_numeric.Items.Add(selectBox_numeric);
            compositeField_numeric.Items.Add(numberField_start);
            compositeField_numeric.Items.Add(panel_between_numeric);

            container_numeric.Items.Add(compositeField_numeric);

            searchPanel.Items.Add(container_numeric);

            #endregion

            #region Container Bool

            Ext.Net.Container container_bool = new Ext.Net.Container() { 
                ID = string.Format("container_bool_{0}", gridID),
                StyleSpec = "left:165px;"
            };

            if (!(hasFirstColumn && firstColumnType == "bool"))
            {
                container_bool.Listeners.AfterRender.Handler = "item.el.setStyle('margin-left', Ext.isIE ? '14px' : '6px'); item.hide();";
            }
            else
            {
                container_bool.StyleSpec += "margin-left:6px;";
                container_bool.Listeners.AfterRender.Handler = "item.show();";
            }

            container_bool.Listeners.BeforeShow.Handler = string.Format("#{{selectbox_bool_filterCondition_{0}}}.selectFirst();", gridID);
            container_bool.Listeners.BeforeShow.Delay = 100;

            container_bool.Listeners.Hide.Handler = string.Format("#{{selectbox_bool_filterCondition_{0}}}.reset();", gridID);

            CompositeField compositeField_bool = new CompositeField() {
                ID = string.Format("compositeField_bool_{0}", gridID),
                Width = 500
            };

            if (hasFirstColumn && firstColumnType == "bool")
            {
                compositeField_bool.Listeners.AfterRender.Handler = "item.el.first().setStyle('height', '23px');";
                compositeField_bool.Listeners.AfterRender.Delay = 150;
            }

            Ext.Net.SelectBox selectBox_bool = new Ext.Net.SelectBox() {
                ID = string.Format("selectbox_bool_filterCondition_{0}", gridID),
                AllowBlank = false,
                Width=107
            };

            selectBox_bool.Items.AddRange(new List<Ext.Net.ListItem>() { 
                new Ext.Net.ListItem { Text = "True", Value = "true" },
                new Ext.Net.ListItem { Text = "False", Value = "false" }
            });

            selectBox_bool.SelectedIndex = 0;

            compositeField_bool.Items.Add(selectBox_bool);

            container_bool.Items.Add(compositeField_bool);

            searchPanel.Items.Add(container_bool);

            #endregion

            #endregion

            #region CompositeField Where Condition

            CompositeField compositeField_whereCondition = new CompositeField()
            {
                ID = string.Format("compositeField_whereCondition_{0}", gridID),
                Hidden = !hasFirstColumn,
                Width = 500,
                StyleSpec = "padding-left:10px;"
            };

            Ext.Net.SelectBox selectBox_whereCondition = new Ext.Net.SelectBox() {
                ID = string.Format("selectbox_whereCondition_{0}", gridID),
                Width = 100,
                AllowBlank = false
            };

            selectBox_whereCondition.Items.AddRange(new List<Ext.Net.ListItem>() { 
                new Ext.Net.ListItem() { Text = "And", Value = "And" },
                new Ext.Net.ListItem() { Text = "Or", Value = "Or" }
            });

            selectBox_whereCondition.SelectedIndex = 0;

            Ext.Net.Button button_search = new Ext.Net.Button() {
                ID = string.Format("button_search_{0}", gridID),
                Text = "Add Filter",
                StyleSpec = "left:100px; margin-left:10px;"
            };

            button_search.Listeners.Click.Handler = string.Format(@"var type = #{{SearchBox_{0}}}.value.split('_')[1];
                                                            if(type == 'int' || type == 'float') {{ type = 'numeric'; }}

                                                            var cf = Ext.getCmp('compositeField_' + type + '_{0}');
                                                            if(!Ax.IsValidComponent(cf))
                                                                return;
                                                                
                                                            var cp = #{{panel_SearchCondition_{0}}};
                                                            var filterItemLength = cp.items.length;

                                                            var filterId = Ax.UUID();

                                                            var selectWhere = #{{selectbox_whereCondition_{0}}};
                                                            var filterText = filterItemLength > 0 ? selectWhere.getRawValue() + ' ' : '';
                                                            filterText += #{{SearchBox_{0}}}.getRawValue();
                                                            filterText += ' ';

                                                            var filterItem = '';

                                                            if(type == 'string') {{
                                                                filterText += #{{selectbox_string_filterCondition_{0}}}.getRawValue();
                                                                filterText += ' ';
                                                                filterText += Ax.FormatFilterText(#{{textField_string_{0}}}.getValue());

                                                                filterItem += Ax.FilterItem(filterId, 
                                                                            #{{SearchBox_{0}}}.value.split('_')[0],
                                                                            selectWhere.getValue(),
                                                                            #{{selectbox_string_filterCondition_{0}}}.getValue(),
                                                                            #{{textField_string_{0}}}.getValue(),
                                                                            '');

                                                            }}
                                                            else if(type == 'date') {{
                                                                filterText += #{{selectbox_date_filterCondition_{0}}}.getRawValue();
                                                                filterText += ' ';
                                                                filterText += #{{datefield_start_filterCondition_{0}}}.getRawValue();

                                                                var isBetween = #{{selectbox_date_filterCondition_{0}}}.getValue() == 'Between';

                                                                if(isBetween) {{
                                                                    filterText += ' and ';
                                                                    filterText += #{{datefield_end_filterCondition_{0}}}.getRawValue();
                                                                }}

                                                                filterItem += Ax.FilterItem(filterId, 
                                                                            #{{SearchBox_{0}}}.value.split('_')[0],
                                                                            selectWhere.getValue(),
                                                                            #{{selectbox_date_filterCondition_{0}}}.getValue(),
                                                                            #{{datefield_start_filterCondition_{0}}}.getRawValue(),
                                                                            isBetween ? #{{datefield_end_filterCondition_{0}}}.getRawValue() : '');
                                                            }}
                                                            else if(type == 'numeric') {{
                                                                filterText += #{{selectbox_numeric_filterCondition_{0}}}.getRawValue();
                                                                filterText += ' ';
                                                                filterText += #{{numberfield_start_numeric_{0}}}.getRawValue();

                                                                var isBetween = #{{selectbox_numeric_filterCondition_{0}}}.getValue() == 'Between';

                                                                if(isBetween) {{
                                                                    filterText += ' and ';
                                                                    filterText += #{{numberfield_end_numeric_{0}}}.getRawValue();
                                                                }}

                                                                filterItem += Ax.FilterItem(filterId, 
                                                                            #{{SearchBox_{0}}}.value.split('_')[0],
                                                                            selectWhere.getValue(),
                                                                            #{{selectbox_numeric_filterCondition_{0}}}.getValue(),
                                                                            #{{numberfield_start_numeric_{0}}}.getValue(),
                                                                            isBetween ? #{{numberfield_end_numeric_{0}}}.getValue() : '');
                                                            }}
                                                            else if(type == 'bool') {{
                                                                filterText += 'Equals ';
                                                                filterText += #{{selectbox_bool_filterCondition_{0}}}.getRawValue();

                                                                filterItem += Ax.FilterItem(filterId, 
                                                                            #{{SearchBox_{0}}}.value.split('_')[0],
                                                                            selectWhere.getValue(),
                                                                            'Equals',
                                                                            #{{selectbox_bool_filterCondition_{0}}}.getValue(),
                                                                            '');
                                                            }}

                                                            var filterItems = #{{hidden_SearchCondition_{0}}}.value;
                                                            if(filterItems) filterItems += ',' + filterItem;
                                                            else filterItems = filterItem;
                                                            #{{hidden_SearchCondition_{0}}}.setValue(filterItems);

                                                            var pnlFilterItem = new Ext.Panel({{
                                                                                    id:'pnlFilterItem_' + filterId,
                                                                                    xtype:'panel',
                                                                                    border:true,
                                                                                    style:'float:left;' + (filterItemLength > 0 ? 'padding-left:10px;' : ''),
                                                                                    items:[
                                                                                        new Ext.form.Label({{
                                                                                            id:'label_' + filterId,
                                                                                            xtype:'label',
                                                                                            text:filterText,
                                                                                            style: 'font:11px/15px arial, tahoma, helvetica, sans-serif; background-color : #F4F4F4; padding-left:10px; padding-top:8px; padding-bottom:8px;'
                                                                                            }}),
                                                                                        new Ext.net.Image({{
                                                                                            id:'deleteIcon_' + filterId,
                                                                                            listeners:
                                                                                                {{
                                                                                                click:{{
                                                                                                        fn:function(item,e){{
                                                                                                            var obj = Ext.getCmp('pnlFilterItem_' + filterId);
                                                                                                            var isFirstItem = cp.items.indexOf(obj) == 0;
                                                                                                            cp.items.remove(obj);
                                                                                                            obj.destroy();
                                                                                                              
                                                                                                            if(isFirstItem && cp.items.length > 0) {{
                                                                                                            var pnlFltr = cp.items.first();
                                                                                                            pnlFltr.el.dom.style.paddingLeft = '';
                                                                                                            var lblFilter = pnlFltr.items.first();
                                                                                                            lblFilter.setText(lblFilter.text.substring(lblFilter.text.indexOf(' ') + 1));
                                                                                                            }}
                                                                                                              
                                                                                                            cp.doLayout(true, true);

                                                                                                            var filterObjs = eval('[' + #{{hidden_SearchCondition_{0}}}.value + ']');
                                                                                                            var newFilterObj = '';
                                                                                                            var filterObj;
                                                                                                            var isFirst = true;
                                                                                                            for(var i = 0; i < filterObjs.length; i++) {{
                                                                                                            filterObj = filterObjs[i];
                                                                                                            if(filterObj.Id == filterId) 
                                                                                                                continue;
                                                                                                            newFilterObj += ((isFirst ? '' : ',') + Ax.FilterObj(filterObj));
                                                                                                            if(isFirst) isFirst = false;
                                                                                                            }}
                                                                                                            #{{hidden_SearchCondition_{0}}}.setValue(newFilterObj);
                                                                                                              
                                                                                                            #{{{0}}}.getPagingToolbar().changePage(1);
                                                                                                        }},
                                                                                                        delay:20
                                                                                                        }}
                                                                                                }},
                                                                                            imageUrl:'/Content/icon-cross.png',
                                                                                            style: 'cursor:pointer; vertical-align:middle; padding-left: 5px; background-color : #F4F4F4; padding-right:8px; padding-top:6px; padding-bottom:6px;'
                                                                                        }})
                                                                                    ]
                                                                                }});
                                                              
                                                            cp.items.add(pnlFilterItem);
                                                            cp.doLayout(true, true);

                                                            if(!Ext.isIE) {{ pnlFilterItem.el.dom.parentNode.style['height'] = '30px'; }}
                                                            #{{{0}}}.getPagingToolbar().changePage(1);", gridID);

            compositeField_whereCondition.Items.Add(selectBox_whereCondition);
            compositeField_whereCondition.Items.Add(button_search);

            #endregion

            Ext.Net.Panel panel_SearchCondition = new Ext.Net.Panel()
            {
                ID = string.Format("panel_SearchCondition_{0}", gridID),
                Layout = "hbox",
                Border = false,
                StyleSpec = "padding:10px;"
            };

            Ext.Net.Label label_CrossIcon = new Ext.Net.Label()
            {
                ID = string.Format("label_loadCrossIcon_{0}", gridID),
                Icon = Ext.Net.Icon.Cross,
                Hidden = true
            };

            Ext.Net.Hidden hidden_SearchCondition = new Ext.Net.Hidden()
            {
                ID = string.Format("hidden_SearchCondition_{0}", gridID)
            };

            mainPanel.Items.Add(searchPanel);
            mainPanel.Items.Add(compositeField_whereCondition);
            mainPanel.Items.Add(panel_SearchCondition);
            mainPanel.Items.Add(label_CrossIcon);
            mainPanel.Items.Add(hidden_SearchCondition);

            Ext.Net.KeyBinding searchKey = new Ext.Net.KeyBinding();
            searchKey.StopEvent = true;
            searchKey.Keys.Add(new Ext.Net.Key { Code = Ext.Net.KeyCode.ENTER });
            searchKey.Listeners.Event.Handler = string.Format("if(#{{SearchBox_{0}}}.getValue().length > 0) {{ var btnAddFilter = #{{button_search_{0}}}; btnAddFilter.fireEvent('click', btnAddFilter); }}", gridID);

            mainPanel.KeyMap.Add(searchKey);

            #endregion

            Ext.Net.BorderLayout borderLayout = new Ext.Net.BorderLayout()
            {
                ID = string.Format("borderLayout_search_{0}", gridID)
            };

            borderLayout.North.Items.Add(mainPanel);
            borderLayout.Center.Items.Add(grid);

            return borderLayout;
        }
    }    
}
