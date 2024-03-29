﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        #region Converter Helper

        /// <summary>
        /// Default set to 0
        /// </summary>
        public static void ConvertIntValueWithDefault(string text, out int value)
        {
            if (string.IsNullOrEmpty(text) || !int.TryParse(text, out value))
                value = 0;
        }

        /// <summary>
        /// Default set to null
        /// </summary>
        public static void ConvertIntValueWithDefault(string text, out int? value)
        {            
            int val;

            if (string.IsNullOrEmpty(text))
                value = null;
            else if (int.TryParse(text, out val))
                value = val;
            else
                value = null;
        }

        /// <summary>
        /// Default set to 0
        /// </summary>
        public static int ConvertIntValueWithDefault(object text)
        {
            if (text == null)
                return 0;
            else
                return ConvertIntValueWithDefault(text.ToString());
        }

        /// <summary>
        /// Default set to 0
        /// </summary>
        public static int ConvertIntValueWithDefault(string text)
        {
            int value;
            if (string.IsNullOrEmpty(text) || !int.TryParse(text, out value))
                value = 0;

            return value;
        }

        /// <summary>
        /// Default return null
        /// </summary>
        public static int? ConvertNullableIntValueWithDefault(object obj)
        {
            int val;
            if (obj == null || !int.TryParse(obj.ToString(), out val))
                return null;
            return val;
        }

        /// <summary>
        /// Default return null
        /// </summary>
        public static int? ConvertNullableIntValueWithDefault(string text)
        {
            int val;
            if (string.IsNullOrEmpty(text) || !int.TryParse(text, out val))
                return null;
            return val;
        }

        /// <summary>
        /// Default set to 0
        /// </summary>
        public static decimal ConvertDecimalValueWithDefault(string text)
        {
            decimal value;
            if (string.IsNullOrEmpty(text) || !decimal.TryParse(text, out value))
                value = 0;

            return value;
        }

        /// <summary>
        /// Default set to 0
        /// </summary>
        public static float ConvertFloatValueWithDefault(string text)
        {
            float value;
            if (string.IsNullOrEmpty(text) || !float.TryParse(text, out value))
                value = 0;

            return value;
        }

        /// <summary>
        /// Default null
        /// </summary>
        public static DateTime? ConvertDateTimeValueWithDefault(string text)
        {
            DateTime value;
            if (string.IsNullOrEmpty(text) || !DateTime.TryParse(text, out value))
                return null;

            return value;
        }

        /// <summary>
        /// default false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertBoolean(string value)
        {
            bool val;
            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out val))
                return false;

            return val;
        }

        /// <summary>
        /// default string.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertStringWithDefault(object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public static Guid? ConvertToGuid(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            Guid value;
            if (Guid.TryParse(key, out value)) return value;
            return null;
        }

        #endregion

        #region TryParse

        /// <summary>
        /// use int.TryParse method but if s is null, return false
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, ref int result)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return int.TryParse(s, out result);
        }

        /// <summary>
        /// use decimal.TryParse method but if s is null, return false
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, ref decimal result)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return decimal.TryParse(s, out result);
        }

        #endregion

        #region IsCorrectType

        public static bool IsInteger(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) return false;
            int result = -1;
            return TryParse(obj.ToString(), ref result);
        }

        public static bool IsDecimal(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) return false;
            decimal result = -1;
            return TryParse(obj.ToString(), ref result);
        }

        #endregion
    }
}
