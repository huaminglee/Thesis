using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class MenuPanel : Ext.Net.MenuPanel, IBaseControl
    {
        public MenuPanel()
        {
            SetDefaultValues = true;
        }

        //protected override void OnInit(EventArgs e)
        //{
        //    LoadDefaultValues();
        //    base.OnInit(e);
        //}

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (this.Menu == null)
                    this.Visible = false;
                else
                {
                    bool hasVisibleItem = false;
                    foreach (var item in this.Menu.Items)
                    {
                        if (item.Visible)
                        {
                            hasVisibleItem = true;
                            break;
                        }
                    }
                    this.Visible = hasVisibleItem;                    
                }
            }
        }

        #endregion
    }
}
