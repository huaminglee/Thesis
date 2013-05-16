using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class ComboBox : Ext.Net.ComboBox, IBaseControl, IStoreBinder
    {
        public ComboBox()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnPreRender(e);
        }

        protected override void OnClientInit(bool reinit)
        {
            SetModelValue();
            base.OnClientInit(reinit);
        }

        #region Properties

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Ext.Net.ListItem SelectedItemModel { get; set; }
        public bool DisableTypeAhead { get; set; }
        public bool AutoFocusOnAfterRender { get; set; }
        public string ParentID { get; set; }
        public string ChildID { get; set; }

        #endregion

        #region Methods

        private void SetModelValue()
        {
            if (SelectedItemModel != null && !string.IsNullOrEmpty(SelectedItemModel.Text))
            {
                Listeners.Render.Delay = 100;
                Listeners.Render.Handler = "Ax.SetComboValue(item, { text: '" + SelectedItemModel.Text + "', value: '" + (SelectedItemModel.Value ?? string.Empty) + "' });";
            }
        }

        #endregion

        #region IStoreBinder Members

        public void BindStore(Store store)
        {
            store.ID = string.Format("store{0}", this.ID);

            if (store.Reader.Count == 0)
            {
                var valueField = ValueField.ToLower() == "value" ? "Value" : ValueField;

                store.Reader.Add(new Ext.Net.JsonReader
                                {
                                    IDProperty = valueField,
                                    Root = "data",
                                    TotalProperty = "total"
                                }
                            );    
        
                store.AddField(new Ext.Net.RecordField { Name = valueField });
                store.AddField(new Ext.Net.RecordField { Name = DisplayField.ToLower() == "text" ? "Text" : DisplayField });
            }

            store.BaseParams.Add(new Ext.Net.Parameter {
                    Name = "DisplayField",
                    Value = this.DisplayField,
                    Mode = Ext.Net.ParameterMode.Value
                });

            store.BaseParams.Add(new Ext.Net.Parameter {
                    Name = "filter",
                    Value = "#{" + ID + "}.getText()",
                    Mode = Ext.Net.ParameterMode.Raw
                });
        }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (this.Store.Count > 0)
                {
                    if (string.IsNullOrEmpty(DisplayField) || DisplayField.ToLower() == "text")
                        DisplayField = "Text";

                    if (string.IsNullOrEmpty(ValueField) || ValueField.ToLower() == "value")
                        ValueField = "Value";
                }

                TypeAhead = !DisableTypeAhead;

                if (PageSize <= 0)
                    PageSize = 10;

                if(string.IsNullOrEmpty(ItemSelector))
                    ItemSelector = "div.search-item";

                MinChars = 1;

                if(ListWidth.IsEmpty)
                    ListWidth = 300;

                if (string.IsNullOrEmpty(Template.Html))
                {
                    StringBuilder itemHtml = new StringBuilder();
                    itemHtml.AppendLine("<tpl for=\".\">");
                    itemHtml.AppendLine("<div class=\"search-item\">");
                    itemHtml.AppendLine("<h3>{" + DisplayField + "}</h3>");
                    itemHtml.AppendLine("</div>");
                    itemHtml.AppendLine("</tpl>");

                    Template.Html = itemHtml.ToString();
                }

                TriggerIcon = Ext.Net.TriggerIcon.Search;
                Triggers.Add(new Ext.Net.FieldTrigger { HideTrigger = true, Icon = Ext.Net.TriggerIcon.Clear });

                bool hasChildID = !string.IsNullOrEmpty(ChildID);
                bool hasParentID = !string.IsNullOrEmpty(ParentID);

                string[] childIds = null;
                if (hasChildID) childIds = ChildID.Split(',');

                string[] parentIds = null;
                if (hasParentID) parentIds = ParentID.Split(',');

                Listeners.BeforeQuery.Handler = "if(Ext.isEmpty(this.getRawValue())) { this.lastQuery=null; this.setValue(''); this.triggers[0].hide(); } else { this.triggers[0].show(); }" + Listeners.BeforeQuery.Handler;
                Listeners.TriggerClick.Handler = "if(index == 0) { item.focus().clearValue(); trigger.hide(); }" + Listeners.TriggerClick.Handler;

                if (hasChildID)
                {
                    string _childId = string.Empty;
                    for (int i = 0; i < childIds.Length; i++)
                    {
                        _childId = childIds[i].Trim();
                        Listeners.BeforeQuery.Handler += " #{" + _childId + "}.lastQuery=null; #{" + _childId + "}.setValue(''); #{" + _childId + "}.triggers[0].hide();";
                        Listeners.TriggerClick.Handler += " if(index == 0) { #{" + _childId + "}.lastQuery=null; #{" + _childId + "}.setValue(''); #{" + _childId + "}.triggers[0].hide(); }";
                    }
                }

                if (hasParentID)
                {
                    for (int i = 0; i < parentIds.Length; i++)
                        Listeners.BeforeQuery.Handler += " return #{" + parentIds[i].Trim() + "}.getValue().toString().length > 0";
                }

                Listeners.Select.Handler = "item.triggers[0].show();" + Listeners.Select.Handler;
                Listeners.Blur.Handler = "if(Ext.isEmpty(item.getText())) { item.setValue(''); item.triggers[0].hide(); } else { item.triggers[0].show(); }" + Listeners.Blur.Handler;

                if (AutoFocusOnAfterRender)
                {
                    Listeners.AfterRender.Handler = "#{" + this.ID + "}.focus();" + Listeners.AfterRender.Handler;
                    Listeners.AfterRender.Delay = 100;
                }
            }
        }

        #endregion
    }
}
