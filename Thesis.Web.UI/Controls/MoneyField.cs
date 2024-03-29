﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Web.UI.Interfaces;
using System.Globalization;
using System.ComponentModel;
using System.Web.UI;

namespace Thesis.Web.UI.Controls
{
    public class MoneyField : Ext.Net.TextField, IBaseControl
    {
        public MoneyField()
        {
            SetDefaultValues = true;
        }

        protected override void OnClientInit(bool reinit)
        {
            LoadDefaultValues();            
            base.OnClientInit(reinit);
        }

        #region Properties

        public MoneyTypes MoneyType { get; set; }
        public double? SelectedValueFromModel { get; set; }
        //public double? MinValue { get; set; }
        //public double? MaxValue { get; set; }

   
        [Description("Custom money type")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Ext.Net.ViewStateMember]
        [Ext.Net.Meta]
        [Category("3. Store")]
        [NotifyParentProperty(true)]
        public CustomMoneyType CustomType { get; set; }

        #endregion

        #region Methods

        public string GetSymbol(MoneyTypes moneyType)
        {
            switch (moneyType)
            {
                case MoneyTypes.Euro:
                    return "€";
                case MoneyTypes.Dollar:
                    return "$";
                case MoneyTypes.Locale:
                    return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
                case MoneyTypes.Custom:
                    return CustomType.CurrencySymbol;
                default:
                    break;
            }

            return string.Empty;
        }

        public string GetSeperator(MoneyTypes moneyType)
        {
            switch (moneyType)
            {
                case MoneyTypes.Euro:
                    return ",";
                case MoneyTypes.Dollar:
                    return ".";
                case MoneyTypes.Locale:
                    return CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                case MoneyTypes.Custom:
                    return CustomType.CurrencyDecimalSeparator;
                default:
                    break;
            }

            return string.Empty;
        }

        public string GetChangeHandler(MoneyTypes moneyType)
        {
            switch (moneyType)
            {
                case MoneyTypes.Euro:
                    return string.Format("var val = Ax.EUMoney(newValue); if(val.indexOf('NaN') != -1) val = ''; var hiddenValueField = #{{{0}_Value}}; if(hiddenValueField) {{ if(val != '') {{ var hiddenVal = val; while (hiddenVal.indexOf('.') != -1) {{ hiddenVal = hiddenVal.replace(/\\./, '') }} hiddenValueField.setValue(hiddenVal.replace(/[{1}]/g, '').replace(/\\,/, '{2}').replace(' ', '')); }} else {{ hiddenValueField.setValue(''); }} }} item.setValue(val);", this.ID, GetSymbol(MoneyType), CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                case MoneyTypes.Dollar:
                    return string.Format("var val = Ax.UsMoney(newValue); if(val.indexOf('NaN') != -1) val = ''; var hiddenValueField = #{{{0}_Value}}; if(hiddenValueField) {{ if(val != '') {{ var hiddenVal = val; while (hiddenVal.indexOf(',') != -1) {{ hiddenVal = hiddenVal.replace(/\\,/, '') }} hiddenValueField.setValue(hiddenVal.replace(/[{1}]/g, '').replace(/\\./, '{2}').replace(' ', '')); }} else {{ hiddenValueField.setValue(''); }} }} item.setValue(val);", this.ID, GetSymbol(MoneyType), CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                case MoneyTypes.Locale:
                    string value = 1.ToString("C");
                    bool hasSpace = value.IndexOf(" ") != -1;
                    bool isLeft = value.StartsWith(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
                    return string.Format("while (newValue.indexOf('{3}') != -1) {{ newValue = newValue.replace(/\\{3}/, '') }} newValue = newValue.replace(/[{1}]/g, '').replace(/\\{2}/, '.'); var val = Ax.MoneyFormat({4}, newValue, '{3}', '{2}', '{1}', {5}); if(val.indexOf('NaN') != -1) val = ''; var hiddenValueField = #{{{0}_Value}}; if(hiddenValueField) {{ if(val != '') {{ var hiddenVal = val; while (hiddenVal.indexOf('{3}') != -1) {{ hiddenVal = hiddenVal.replace(/\\{3}/, '') }} hiddenValueField.setValue(hiddenVal.replace(/[{1}]/g, '').replace(' ', '')); }} else {{ hiddenValueField.setValue(''); }} }} item.setValue(val);", this.ID, GetSymbol(MoneyType), CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, hasSpace.ToString().ToLower(), isLeft.ToString().ToLower());
                case MoneyTypes.Custom:
                    return string.Format("while (newValue.indexOf('{3}') != -1) {{ newValue = newValue.replace(/\\{3}/, '') }} newValue = newValue.replace(/[{1}]/g, '').replace(/\\{2}/, '.'); var val = Ax.MoneyFormat({4}, newValue, '{3}', '{2}', '{1}', {5}); if(val.indexOf('NaN') != -1) val = ''; var hiddenValueField = #{{{0}_Value}}; if(hiddenValueField) {{ if(val != '') {{ var hiddenVal = val; while (hiddenVal.indexOf('{3}') != -1) {{ hiddenVal = hiddenVal.replace(/\\{3}/, '') }} hiddenValueField.setValue(hiddenVal.replace(/[{1}]/g, '').replace(/\\{2}/, '{6}').replace(' ', '')); }} else {{ hiddenValueField.setValue(''); }} }} item.setValue(val);", this.ID, GetSymbol(MoneyType), CustomType.CurrencyDecimalSeparator, CustomType.CurrencyGroupSeparator, CustomType.HasSpace.ToString().ToLower(), CustomType.IsLeft.ToString().ToLower(), CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                default:
                    break;
            }

            return string.Empty;
        }

        #endregion

        #region IBaseControl Members

        public bool SetDefaultValues { get; set; }

        public void LoadDefaultValues()
        {
            if (SetDefaultValues)
            {
                if (MoneyType == 0)
                    MoneyType = MoneyTypes.Locale;

                MaskRe = string.Format(@"/[0-9\{0}\{1}]/", GetSymbol(MoneyType), GetSeperator(MoneyType));
                StyleSpec = "text-align:right;";

                this.Listeners.Change.Handler = GetChangeHandler(MoneyType);

                if (SelectedValueFromModel.HasValue)
                {
                    string value = string.Empty;

                    switch (MoneyType)
                    {
                        case MoneyTypes.Euro:
                            value = SelectedValueFromModel.Value.ToString();
                            break;
                        case MoneyTypes.Dollar:
                            value = SelectedValueFromModel.Value.ToString();
                            break;
                        case MoneyTypes.Locale:
                            value = SelectedValueFromModel.Value.ToString("C");
                            break;
                        case MoneyTypes.Custom:
                            value = SelectedValueFromModel.Value.ToString();
                            break;
                        default:
                            break;
                    }

                    this.Listeners.AfterRender.Handler = "var newValue = '" + (MoneyType == MoneyTypes.Locale ? value : (GetSymbol(MoneyType) + value.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, GetSeperator(MoneyType)))) + "';" + GetChangeHandler(MoneyType) + this.Listeners.AfterRender.Handler;
                    this.Listeners.AfterRender.Delay = 100;
                }
            }
        }

        #endregion

        #region Helper Objects

        public enum MoneyTypes
        { 
            Euro = 1,
            Dollar = 2,
            Locale = 3,
            Custom = 4
        }

        public class CustomMoneyType
        {
            public string CurrencySymbol { get; set; }
            public string CurrencyDecimalSeparator { get; set; }
            public string CurrencyGroupSeparator { get; set; }
            public bool IsLeft { get; set; }
            public bool HasSpace { get; set; }
        }

        #endregion
    }
}
