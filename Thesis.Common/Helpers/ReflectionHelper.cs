using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        public static object GetValue(object obj, string propertyName)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property == null)
                return null;

            return property.GetValue(obj, null);
        }

        public static void SetValue(object obj, string propertyName, object value)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property == null)
                return;

            property.SetValue(obj, value, null);
        }
    }
}
