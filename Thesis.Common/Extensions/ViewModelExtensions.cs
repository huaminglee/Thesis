﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.ViewModels;

namespace Thesis.Common.Extensions
{
    public static class ViewModelExtensions
    {
        #region FilterBase

        public static void AddFilterBase(this GridPanelViewModel model, string propertyName, int value)
        {
            model.AddFilterBase(propertyName, value.ToString());
        }

        public static void AddFilterBase(this GridPanelViewModel model, string propertyName, string value)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            if (model.FilterBase == null)
                model.FilterBase = new List<FilterViewModel>();

            model.FilterBase.Add(new FilterViewModel() { 
                PropertyName = propertyName,
                FilterCondition = Enumerations.FilterCondition.Equals,
                WhereCondition = Enumerations.PredicateCondition.And,
                StartValue = value
            });
        }

        #endregion

        #region Filter

        public static void AddFilter(this GridPanelViewModel model, string propertyName, int value)
        {
            model.AddFilter(propertyName, value.ToString());
        }

        public static void AddFilter(this GridPanelViewModel model, string propertyName, string value)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            if (model.Filter == null)
                model.Filter = new List<FilterViewModel>();

            model.Filter.Add(new FilterViewModel()
            {
                PropertyName = propertyName,
                FilterCondition = Enumerations.FilterCondition.Equals,
                WhereCondition = Enumerations.PredicateCondition.And,
                StartValue = value
            });
        }

        #endregion
    }
}
