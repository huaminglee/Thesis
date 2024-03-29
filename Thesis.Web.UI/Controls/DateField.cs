﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class DateField : Ext.Net.DateField, IBaseControl
    {
        public DateField()
        {
            SetDefaultValues = true;
        }

        protected override void OnClientInit(bool reinit)
        {
            LoadDefaultValues();
            base.OnClientInit(reinit);
        }

        #region Properties

        public DateTime? SelectedDateFromModel { get; set; }
        public string EndDateField { get; set; }
        public string StartDateField { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if(SelectedDateFromModel.HasValue)
                    base.SelectedDate = SelectedDateFromModel.Value;

                if (!string.IsNullOrEmpty(EndDateField))
                {
                    Listeners.Change.Handler += "if(#{" + ID + "}.getValue() == '') { #{" + EndDateField + "}.setMinValue(null); }";
                    Listeners.Select.Handler += "#{" + EndDateField + "}.setMinValue(#{" + ID + "}.value);";

                    if (SelectedDateFromModel.HasValue)
                    {
                        Listeners.AfterRender.Handler += "#{" + EndDateField + "}.setMinValue('" + SelectedDateFromModel.Value.ToShortDateString() + "');";
                        Listeners.AfterRender.Delay = 150;                            
                    }
                }

                if (!string.IsNullOrEmpty(StartDateField))
                {
                    Listeners.Change.Handler += "if(#{" + ID + "}.getValue() == '') { #{" + StartDateField + "}.setMaxValue(null); }";
                    Listeners.Select.Handler += "#{" + StartDateField + "}.setMaxValue(#{" + ID + "}.value);";

                    if (SelectedDateFromModel.HasValue)
                    {
                        Listeners.AfterRender.Handler += "#{" + StartDateField + "}.setMaxValue('" + SelectedDateFromModel.Value.ToShortDateString() + "');";
                        Listeners.AfterRender.Delay = 150;
                    }
                }
            }
        }

        #endregion
    }
}
