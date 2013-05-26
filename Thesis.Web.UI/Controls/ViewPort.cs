using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.Web.UI;

namespace Thesis.Web.UI.Controls
{
    public class ViewPort : Ext.Net.Viewport, IBaseControl
    {
        public ViewPort()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Ext.Net.ItemsCollection<GridPanel> ListPage { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (string.IsNullOrEmpty(Layout))
                    Layout = "fit";

                if (ListPage != null && ListPage.Count == 1)
                {
                    GridPanel grid = ListPage[0] as GridPanel;
                    if (grid != null)
                    {
                        if (grid.EnableSearch)
                        {
                            Ext.Net.Panel panel = new Ext.Net.Panel()
                            {
                                ID = string.Format("panel_{0}", grid.ID),
                                Border = false
                            };

                            panel.Items.Add(Thesis.Web.UI.Helpers.UIHelper.GridComponent(grid));

                            base.Items.Add(panel);
                        }
                        else
                            base.Items.Add(grid);
                    }
                }
            }
        }

        #endregion
    }
}
