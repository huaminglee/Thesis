﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class SubmitButton : Ext.Net.Button, IBaseControl
    {
        public SubmitButton()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool DisableIcon { get; set; }
        public string FormPanelID { get; set; }
        public bool SetNew { get; set; }
        public bool DisableSuccessHandler { get; set; }
        public bool DisableFailureHandler { get; set; }
        public string SuccessHandler { get; set; }
        public string FailureHandler { get; set; }
        public string SuccessHandlerFn { get; set; }
        public string FailureHandlerFn { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if(!DisableIcon && Icon == Ext.Net.Icon.None)
                    Icon = Ext.Net.Icon.Disk;

                string success = string.Empty;
                if (!DisableSuccessHandler)
                {
                    if (!string.IsNullOrEmpty(SuccessHandlerFn))
                        success = SuccessHandlerFn;
                    else if (!string.IsNullOrEmpty(SuccessHandler))
                        success = "function (form, action) { " + SuccessHandler + " }";

                    if (success.Length > 0)
                        success = string.Format(", success: {0}", success);
                }

                string failure = string.Empty;
                if (!DisableFailureHandler)
                {
                    if (!string.IsNullOrEmpty(FailureHandlerFn))
                        failure = FailureHandlerFn;
                    else if (!string.IsNullOrEmpty(FailureHandler))
                        failure = "function (form, action) { " + FailureHandler + " }";

                    if (failure.Length > 0)
                        failure = string.Format(", failure: {0}", failure);
                }

                Listeners.Click.Handler = "if(Ax.IsValidForm(#{" + FormPanelID + "})) #{" + FormPanelID + "}.form.submit({ params: { setNew:" + SetNew.ToString().ToLower() + " }" + success + failure + " });";
                Listeners.Click.Delay = 100;
            }
        }

        #endregion
    }
}
