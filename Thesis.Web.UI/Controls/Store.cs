﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class Store : Ext.Net.Store
    {
        public Store()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool SetDefaultValues { get; set; }
        public string Url { get; set; }

        #endregion

        #region Methods

        private void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                AutoLoad = false;

                Proxy.Add(new Ext.Net.HttpProxy { Url = this.Url });

                IStoreBinder storeBinder = base.ParentComponent as IStoreBinder;
                if (storeBinder != null)
                    storeBinder.BindStore(this);
            }
        }

        #endregion
    }
}
