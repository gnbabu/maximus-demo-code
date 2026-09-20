using System;
using System.Collections.Generic;

namespace ReportingService.Services.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class MapsToAttribute : Attribute
    {

        public MapsToAttribute(string PropertyName)
        {
            _propertyNames.Add(PropertyName);
        }

        public MapsToAttribute(string[] PropertyNames)
        {
            _propertyNames.AddRange(PropertyNames);
        }

        private List<string> _propertyNames = new List<string>(3);

        public string[] PropertyNames
        {
            get { return _propertyNames.ToArray(); }
        }

    }
}
