using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.Web.UI;

namespace Thesis.Web.UI.Controls
{
    public class FieldSet : Ext.Net.FieldSet, IBaseControl
    {
        public FieldSet()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnPreRender(e);
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
                if(Width.IsEmpty)
                    Width = 450;

                if(string.IsNullOrEmpty(Layout))
                    Layout = "form";
                
                Collapsible = true;
                AnimCollapse = true;
                TitleCollapse = true;
                this.BodyStyle = "padding-top:5px;";

                if (ListPage != null && ListPage.Count == 1)
                {
                    GridPanel grid = ListPage[0] as GridPanel;
                    if (grid != null)
                        base.Items.Add(Thesis.Web.UI.Helpers.UIHelper.GridComponent(grid));
                }
            }
        }

        #endregion
    }
}
