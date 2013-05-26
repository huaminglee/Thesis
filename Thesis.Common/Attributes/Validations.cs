using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel;
using System.Data.SqlTypes;

namespace Thesis.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty
        {
            get;
            private set;
        }

        public string OriginalProperty
        {
            get;
            private set;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";

        //private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;
        private int _minCharacters { get; set; }

        public ValidatePasswordLengthAttribute(int minCharacters) : base(_defaultErrorMessage)
        {
            this._minCharacters = minCharacters;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            if (String.IsNullOrEmpty(valueAsString))
            {
                return true;
            }

            return (valueAsString.Length >= _minCharacters);
        }
    }

    public sealed class ValidateRequiredAttribute : RequiredAttribute
    {
        public ValidateRequiredAttribute()
        {
            base.ErrorMessage = "This field is required";
        }
    }

    public sealed class ValidateStringLengthAttribute : StringLengthAttribute
    {
        int maximumLength;

        public ValidateStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            this.maximumLength = maximumLength;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("This field must be a string with a maximum length of {0}",
                this.maximumLength);
        }
    }

    public sealed class ValidateDateTimeAttribute : RangeAttribute
    {
        public ValidateDateTimeAttribute()
            : base(typeof(DateTime), SqlDateTime.MinValue.ToString(), SqlDateTime.MaxValue.ToString())
        {
            base.ErrorMessage = "This field must be bigger than " + SqlDateTime.MinValue.ToString();
        }
    }

    public sealed class ValidateIDAttribute : RangeAttribute
    {
        public ValidateIDAttribute() : base(1, int.MaxValue)
        {
            base.ErrorMessage = "This field is required";
        }
    }
}
