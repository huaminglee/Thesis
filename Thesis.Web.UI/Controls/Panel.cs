using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.Web.UI;

namespace Thesis.Web.UI.Controls
{
    public class Panel : Ext.Net.Panel, IBaseControl
    {
        public Panel()
        {
            SetDefaultValues = 
                SetDefaultSearchPanelSize = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool SetDefaultSearchPanelSize { get; set; }

        public bool EnableBorder { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Ext.Net.ItemsCollection<GridPanel> ListPage { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                Border = EnableBorder;

                if (string.IsNullOrEmpty(Layout))
                    Layout = "fit";

                if (ListPage != null && ListPage.Count == 1)
                {
                    GridPanel grid = ListPage[0] as GridPanel;
                    if (grid != null)
                    {
                        if (grid.LoadOnActive)
                        {
                            string gridStoreID = string.Format("store{0}", grid.ID);
                            this.Listeners.Activate.Handler += "if(!#{" + gridStoreID + "}.loaded) #{" + gridStoreID + "}.reload();";
                        }

                        base.Items.Add(Thesis.Web.UI.Helpers.UIHelper.GridComponent(grid, SetDefaultSearchPanelSize));
                    }
                }
            }
        }

        #endregion
    }
}
