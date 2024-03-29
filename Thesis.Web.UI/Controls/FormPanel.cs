﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class FormPanel : Ext.Net.FormPanel, IBaseControl
    {
        public FormPanel()
        {
            SetDefaultValues = true;
        }

        protected override void OnInit(EventArgs e)
        {
            LoadDefaultValues();
            base.OnInit(e);
        }

        #region Properties

        public bool DisableAutoScroll { get; set; }
        public bool EnableBorder { get; set; }
        public bool DisableDefaultLabelWidth { get; set; }
        public bool DisableBottomSpacer { get; set; }
        public int BottomSpacerHeight { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                AutoScroll = !DisableAutoScroll;
                Cls += "formpanel-item";
                Border = EnableBorder;
                if(!DisableDefaultLabelWidth)
                    LabelWidth = 130;

                if (!DisableBottomSpacer)
                {
                    if (BottomSpacerHeight <= 0)
                        BottomSpacerHeight = 20;

                    Items.Add(new Ext.Net.Panel { ID = string.Format("pnlSpacer{0}", this.ID), Border = false, Width = 1, Height = BottomSpacerHeight });
                }
            }
        }

        #endregion
    }
}
