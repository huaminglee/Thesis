using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;

namespace Thesis.Web.UI.Controls
{
    public class NumberField : Ext.Net.NumberField, IBaseControl
    {
        public NumberField()
        {
            SetDefaultValues = true;
        }

        protected override void OnClientInit(bool reinit)
        {
            LoadDefaultValues();
            base.OnClientInit(reinit);
        }

        #region Properties
        
        public string EndNumberField { get; set; }
        public string StartNumberField { get; set; }
        public double? SelectedNumberFromModel { get; set; }
        public ValueTypes ValueType { get; set; }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                this.DecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

                if (SelectedNumberFromModel.HasValue)
                    this.Number = SelectedNumberFromModel.Value;

                if (this.ValueType != 0)
                {
                    switch (this.ValueType)
                    {
                        case ValueTypes.Byte:
                            if (this.MinValue == double.MinValue || this.MinValue < byte.MinValue) this.MinValue = byte.MinValue;
                            if (this.MaxValue == double.MaxValue || this.MaxValue > byte.MaxValue) this.MaxValue = byte.MaxValue;
                            this.AllowNegative = this.AllowDecimals = false;
                            break;
                        case ValueTypes.Int:
                            if (this.MinValue == double.MinValue || this.MinValue < int.MinValue) this.MinValue = int.MinValue;
                            if (this.MaxValue == double.MaxValue || this.MaxValue > int.MaxValue) this.MaxValue = int.MaxValue;
                            this.AllowDecimals = false;
                            break;
                        case ValueTypes.Decimal:
                            if (this.MinValue == double.MinValue || this.MinValue < (double)decimal.MinValue) this.MinValue = (double)decimal.MinValue;
                            if (this.MaxValue == double.MaxValue || this.MaxValue > (double)decimal.MaxValue) this.MaxValue = (double)decimal.MaxValue;
                            break;
                        case ValueTypes.Float:
                            if (this.MinValue == double.MinValue || this.MinValue < float.MinValue) this.MinValue = float.MinValue;
                            if (this.MaxValue == double.MaxValue || this.MaxValue > float.MaxValue) this.MaxValue = float.MaxValue;
                            break;
                        case ValueTypes.Double:
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(EndNumberField))
                {
                    Listeners.Change.Handler += "if(#{" + ID + "}.getValue() == '') { #{" + EndNumberField + "}.setMinValue(null); } else { #{" + EndNumberField + "}.setMinValue(#{" + ID + "}.value); } ";

                    if (SelectedNumberFromModel.HasValue)
                    {
                        Listeners.AfterRender.Handler += "#{" + EndNumberField + "}.setMinValue('" + SelectedNumberFromModel.Value.ToString(System.Globalization.CultureInfo.CurrentCulture) + "');";
                        Listeners.AfterRender.Delay = 150;
                    }
                }

                if (!string.IsNullOrEmpty(StartNumberField))
                {
                    Listeners.Change.Handler += "if(#{" + ID + "}.getValue() == '') { #{" + StartNumberField + "}.setMaxValue(null); } else { #{" + StartNumberField + "}.setMaxValue(#{" + ID + "}.value); }";

                    if (SelectedNumberFromModel.HasValue)
                    {
                        Listeners.AfterRender.Handler += "#{" + StartNumberField + "}.setMaxValue('" + SelectedNumberFromModel.Value.ToString(System.Globalization.CultureInfo.CurrentCulture) + "');";
                        Listeners.AfterRender.Delay = 150;
                    }
                }
            }
        }

        #endregion

        #region Helper Objects

        public enum ValueTypes
        { 
            Byte = 1,
            Int = 2,
            Decimal = 3,
            Float = 4,
            Double = 5
        }

        #endregion
    }
}
