using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thesis.Common.Helpers
{
    public partial class Ax
    {
        private const string _stringRequiredErrorMessage = "Value cannot be null or empty.";

        public static void ValidateRequiredStringValue(string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException(_stringRequiredErrorMessage, parameterName);
            }
        }
    }
}
