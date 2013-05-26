using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class ModuleGroup : Ext.Net.FieldSet, IBaseControl
    {
        public ModuleGroup()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (Width.IsEmpty)
                    Width = 450;

                if (string.IsNullOrEmpty(Layout))
                    Layout = "form";

                Collapsible = true;
                AnimCollapse = true;
                TitleCollapse = true;

                CheckboxToggle = true;
                HideCollapseTool = true;

                this.Listeners.AfterRender.Handler = "item.checkbox.dom.checked = false;";
                this.Listeners.BeforeCollapse.Handler = @"var isChecked = item.checkbox.dom.checked;
                                                                    var modules = item.items.items;
                                                                    var moduleLength = modules.length; 
                                                                    if(moduleLength == 0) return false; 
                                                                    for(var i = 0; i < moduleLength; i++) {
                                                                        if(modules[i].xtype == 'checkboxgroup') {
                                                                            var moduleProcess = modules[i].items.items;
                                                                            for(var j = 0; j < moduleProcess.length; j++) {
                                                                                if(moduleProcess[j].xtype == 'checkbox') {
                                                                                    moduleProcess[j].setValue(isChecked);
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    return false;";

                Ext.Net.CompositeField cf = new Ext.Net.CompositeField();
                cf.StyleSpec = "margin-bottom:10px;";
                cf.Items.AddRange(new List<Ext.Net.Label>() { 
                    new Ext.Net.Label(){ Text = "View" },
                    new Ext.Net.Label(){ Text = "Add", StyleSpec="padding-left:40px;" },
                    new Ext.Net.Label(){ Text = "Update", StyleSpec="padding-left:36px;" },
                    new Ext.Net.Label(){ Text = "Delete", StyleSpec="padding-left:30px;" }
                });

                this.Items.Insert(0, cf);
            }
        }

        #endregion
    }
}
