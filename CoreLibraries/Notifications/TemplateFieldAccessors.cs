using System;
using System.Collections.Generic;

using System.Data;
using System.Reflection;

namespace MAXIMUS.Core.Libraries
{


    public delegate object TemplateFieldAccessor(string fieldName);

    /// <summary>
    /// Provides common accessor patterns
    /// </summary>
    public static class TemplateFieldAccessors
    {

        public static TemplateFieldAccessor DictionaryAccessor(IDictionary<string, object> dictionary)
        {
            return DictionaryAccessor(dictionary, false);
        }

        public static TemplateFieldAccessor DictionaryAccessor(IDictionary<string, object> dictionary, bool throwIfNotFound)
        {
            return delegate(string fieldName)
            {
                object value;
                try
                {

                    if (!dictionary.TryGetValue(fieldName, out value) && throwIfNotFound)
                        throw new Exception(string.Format("{0} not found in dictionary.", fieldName));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return value;
            };
        }

        public static TemplateFieldAccessor IDataRecordAccessor<T>(T record) where T : IDataRecord
        {
            return delegate(string fieldName)
            {
                try
                {
                    int index = record.GetOrdinal(fieldName);
                    if (!record.IsDBNull(index))
                        return record.GetValue(index);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return null;

            };
        }

        public static TemplateFieldAccessor DataRowAccessor(DataRow row)
        {
            return delegate(string fieldName)
            {
                try
                {
                    if (!row.IsNull(fieldName))
                        return row[fieldName];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return null;
            };
        }

        public static TemplateFieldAccessor PropertyAccessor(object instance)
        {
            Type type = instance.GetType();
            return delegate(string fieldName)
            {
                PropertyInfo info = type.GetProperty(fieldName);
                try
                {

                    if (info == null)
                        throw new InvalidOperationException(string.Format("Object {0} does not have property '{1}'", type, fieldName));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return info.GetValue(instance, null);
            };
        }
    }
}
