using System;
using System.ComponentModel.DataAnnotations;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        RequiredAttribute _innerAttribute = new RequiredAttribute();
        public string _error { get; set; }
        public string _dependentProperty { get; set; }
        public object _targetValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue, string errorMessage)
        {
            this._dependentProperty = dependentProperty;
            this._targetValue = targetValue;
            this._error = errorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string errorMessage = String.IsNullOrWhiteSpace(this._error) ?validationContext.DisplayName + " Is required." : this._error;
                        return new ValidationResult(ErrorMessage = errorMessage);
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }
    }

}
