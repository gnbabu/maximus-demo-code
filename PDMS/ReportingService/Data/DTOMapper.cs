using ReportingService.Services.Data.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReportingService.Services.Data
{
    /// <summary>
    /// DTOMapper is a Data Transfer Object Mapper. It is used to map the values of one object to another object.
    /// The source object can be any object that has properties that match the target object.
    /// The MapsToAttribute can be used to map a source property to a target property where the property names do not match.
    /// When the Source and Target properties are different data types, the source property value is converted to the target property type.
    /// </summary>
    public class DTOMapper
    {

        #region options

        public class Options
        {
            public bool SkipNulls = false;
            public bool SkipZeroInts = false;
            public string SrcPrefix = null;
            public string SrcSuffix = null;
            public string TargetPrefix = null;
            public string TargetSuffix = null;
            public int MaxDepth = 0;
            public bool CaseInsensitive = false;
            public Dictionary<string,object> Settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// Sets the value of properties in the target object with values from the source object where the propery names match.
        ///  
        /// </summary>
        /// <param name="sourceObj">source object</param>
        /// <param name="targetObj">object instance to bind vaues to</param>
        /// <param name="ignore">String array of target properties by name that are not to bound from the source i.e. ignore</param>
        ///<returns>Returns the TargetObject</returns>
        static public T Map<T>(object sourceObj, T targetObj, string[] ignore)
        {
            DoMap(sourceObj, targetObj, ignore, null);
            return targetObj;
        }

        /// <summary>
        /// Sets the value of properties in the target object with values from the source object where the propery names match.
        ///  
        /// </summary>
        /// <param name="sourceObj">Source object</param>
        /// <param name="targetObj">object instance to bind vaues to</param>
        /// <param name="options">DTOMapper.Options instance where options can be set</param>
        ///<returns>Returns the TargetObject</returns>
        static public T Map<T>(object sourceObj, T targetObj, Options options = null)
        {
            DoMap(sourceObj, targetObj, null, options);
            return targetObj;
        }

        /// <summary>
        /// Sets the value of properties in the target object with values from the source object where the propery names match.
        ///  and the source propery name is prefixed by 'Prefix'
        ///  and the source property name has a suffix of 'Suffix'
        /// </summary>
        /// <param name="sourceObj">source object</param>
        /// <param name="ignore">String array of target properties by name that are not to bound from the source i.e. ignore</param>
        /// <param name="options">DTOMapper.Options instance where options can be set</param>
        static public T Map<T>(object sourceObj, string[] ignore = null, Options options = null) where T : class, new()
        {
            var targetObj = new T();
            DoMap(sourceObj, targetObj, ignore, options);
            return targetObj;
        }

        static public T[] Map<T>(IEnumerable<object> sourceObj, string[] ignore = null, Options options = null) where T : class, new()
        {
            var output = new List<T>(sourceObj.Count());
            foreach (var item in sourceObj)
            {
                var targetObj = new T();
                DoMap(item, targetObj, ignore, options);
                output.Add(targetObj);
            }
            return output.ToArray();
        }

        /// <summary>
        /// Sets the value of properties in the target object with values from the source object where the propery names match.
        ///  and the source propery name is prefixed by 'Prefix'
        ///  and the source property name has a suffix of 'Suffix'
        /// </summary>
        /// <param name="sourceObj">source object</param>
        /// <param name="targetObj">object instance to bind vaues to</param>
        /// <param name="ignore">String array of target properties by name that are not to bound from the source i.e. ignore</param>
        /// <param name="options">DTOMapper.Options instance where options can be set</param>
        static public T Map<T>(object sourceObj, T targetObj, string[] ignore, Options options)
        {
            DoMap(sourceObj, targetObj, ignore, options);
            return targetObj;
        }

        protected static IEqualityComparer<string> StringComparerConfig(bool caseInsensitive)
        {
            return caseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        }

        static protected void DoMap(object sourceObj, object targetObj, string[] ignore, Options options, int depth = 1)
        {
            if (sourceObj == null || targetObj == null) { return; }
            if (options == null) options = new Options();
            var ignoreLength = ignore?.Length ?? 1;
            //index ignore fields
            Dictionary<string, byte> ignoreList = new Dictionary<string, byte>(ignoreLength, StringComparerConfig(options.CaseInsensitive));
            if (ignore != null && ignore.Length > 0)
            {
                foreach (string fieldName in ignore)
                    ignoreList.Add(fieldName, 0);
            }
            Type srcType = sourceObj.GetType();

            Type targetType = targetObj.GetType();

            MapMembers(srcType, sourceObj, targetType, targetObj, ignoreList, options, depth);
        }

        protected static void MapMembers(Type srcType, object sourceObj, Type targetType, object targetObj, Dictionary<string, byte> ignoreList, Options options, int depth = 1)
        {
            List<MemberInfo> targetMembers = new List<MemberInfo>(targetType.GetProperties());
            targetMembers.AddRange(targetType.GetFields());

            //load target properties into a dictionary by name
            var targetMbrList = GetTargetMaps(targetMembers.ToArray(), options);

            //load source properties
            List<MemberInfo> srcMembers = new List<MemberInfo>(srcType.GetProperties());
            srcMembers.AddRange(srcType.GetFields());

            //loop thru each source property and bind to target object if a match
            foreach (MemberInfo smi in srcMembers)
            {

                //if source member name is in ignore list skip to next property
                if (ignoreList.ContainsKey(smi.Name)) continue;

                var srcPropMaps = GetRankedSrcMaps(smi, options);
                foreach (var srcPropMap in srcPropMaps)
                {
                    //if expected property exists on target object bind the source value to it
                    if (targetMbrList.ContainsKey(srcPropMap))
                    {
                        object value = null;
                        MemberInfo tmi = targetMbrList[srcPropMap];
                        Type tmiType = MemberInfoType(tmi);

                        //if not a value type, string or guid or enumeration then prop is an object, recursively map the object.
                        if (!tmiType.IsValueType && tmiType != typeof(string) && tmiType != typeof(Guid))
                        {
                            if (tmiType.GetInterface(nameof(IEnumerable)) != null) continue;
                            if (options.MaxDepth == 0 || depth < options.MaxDepth)
                            {
                                var svalue = _getValue(smi, sourceObj);
                                var tvalue = _getValue(tmi, targetObj);
                                if (svalue != null)
                                    MapMembers(svalue.GetType(), svalue, tvalue.GetType(), tvalue, ignoreList, options, depth + 1);
                            }
                            else continue;
                        }


                        value = _getValue(smi, sourceObj);
                        if (options.SkipNulls && value == null) continue;
                        if (options.SkipZeroInts && value != null)
                        {
                            if (value is int && (int)value == 0) continue;
                            Type smiType = MemberInfoType(smi);
                            if (smiType.IsEnum && (
                                Enum.GetUnderlyingType(value.GetType()) == typeof(int) && (int)value == 0 ||
                                Enum.GetUnderlyingType(value.GetType()) == typeof(byte) && (byte)value == 0 ||
                                Enum.GetUnderlyingType(value.GetType()) == typeof(short) && (short)value == 0 ||
                                Enum.GetUnderlyingType(value.GetType()) == typeof(long) && (long)value == 0
                                ))
                                continue;
                        }
                        _setValue(smi, targetObj, tmi, value, options);
                    }
                }
            }
        }


        /// <summary>
        /// returns list of Property Map aliases ranked in order of most specific to least specific 
        /// </summary>
        /// <param name="smi"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected static List<string> GetRankedSrcMaps(MemberInfo smi, Options options)
        {
            var nameCnt = 0;
            //check for alternate mapping PropertyName
            var attr = smi.GetCustomAttribute(typeof(MapsToAttribute), false);
            if (attr != null) { nameCnt = ((MapsToAttribute)attr).PropertyNames.Length; }

            var propMaps = new List<string>(nameCnt + 1);
            //add alternate property maps
            if (attr != null)
                foreach (var pm in ((MapsToAttribute)attr).PropertyNames)
                    propMaps.Add(targetPropertyName(pm, options.SrcPrefix, options.SrcSuffix, options.TargetPrefix, options.TargetSuffix));

            propMaps.Add(targetPropertyName(smi.Name, options.SrcPrefix, options.SrcSuffix, options.TargetPrefix, options.TargetSuffix));
            //sort Property Name maps by the count of dots in the name decending.
            propMaps = propMaps.OrderBy(x => x.Where(y => y == '.').Count()).ToList();
            propMaps.Reverse();
            return propMaps;
        }

        protected static Type MemberInfoType(MemberInfo mi)
        {
            Type miType;
            if (mi is PropertyInfo)
                miType = ((PropertyInfo)mi).PropertyType;
            else
                miType = ((FieldInfo)mi).FieldType;
            return miType;
        }

        protected static Dictionary<string, MemberInfo> GetTargetMaps(MemberInfo[] targetPropArray, Options options)
        {
            Dictionary<string, MemberInfo> targetPropList =
                new Dictionary<string, MemberInfo>(
                    targetPropArray.Length * 2,
                    StringComparerConfig(options.CaseInsensitive)
                );

            foreach (MemberInfo p in targetPropArray)
            {
                if (!(p is PropertyInfo) && !(p is FieldInfo)) continue;

                var attr = p.GetCustomAttribute(typeof(MapsToAttribute), false) as MapsToAttribute;

                if (attr != null)
                {
                    foreach (var propertyName in attr.PropertyNames)
                    {
                        targetPropList[propertyName] = p;
                    }
                }

                targetPropList[p.Name] = p;
            }

            return targetPropList;
        }


        /// <summary>
        /// Produce expected target property name after source prefix and suffix has been stripped and target prefix and suffix is applied
        /// </summary>
        /// <param name="sProp">Source Property as PropertyInfo</param>
        /// <param name="sprefix">Source Prefix</param>
        /// <param name="ssuffix">Source Suffix</param>
        /// <param name="tprefix">Target Prefix</param>
        /// <param name="tsuffix">Target Suffix</param>
        /// <returns>Target Property Name</returns>
        private static string targetPropertyName(
            string sPropName,
            string sprefix,
            string ssuffix,
            string tprefix,
            string tsuffix)
        {
            string srcPropName = sPropName;

            if (!IsNullOrWhiteSpace(sprefix))
            {
                if (sPropName != null && sPropName.StartsWith(sprefix))
                {
                    srcPropName = srcPropName.Substring(sprefix.Length);
                }
                else
                {
                    return "";
                }
            }

            if (!IsNullOrWhiteSpace(ssuffix))
            {
                if (sPropName != null && sPropName.EndsWith(ssuffix))
                {
                    srcPropName = srcPropName.Remove(srcPropName.Length - ssuffix.Length);
                }
                else
                {
                    return "";
                }
            }

            if (!IsNullOrWhiteSpace(tprefix) && sPropName != null)
            {
                srcPropName = tprefix + srcPropName;
            }

            if (!IsNullOrWhiteSpace(tsuffix) && sPropName != null)
            {
                srcPropName = srcPropName + tsuffix;
            }

            return srcPropName;
        }

        static private object _getValue(MemberInfo mi, object obj)
        {
            return mi is PropertyInfo ? _getValue((PropertyInfo)mi, obj) : _getValue((FieldInfo)mi, obj);
        }

        static private object _getValue(PropertyInfo pi, object obj)
        {
            return pi.GetValue(obj, null);
        }
        static private object _getValue(FieldInfo pi, object obj)
        {
            return pi.GetValue(obj);
        }

        private static void _setValue(MemberInfo smi, object obj, MemberInfo tmi, object value, Options options)
        {
            Type tmiType = MemberInfoType(tmi);

            string propTypeName = tmiType.Name;

            if (tmiType.IsGenericType)
            {
                if (value == null)
                {
                    _setValue(tmi, obj, null);
                    return;
                }

                var underlyingType = Nullable.GetUnderlyingType(tmiType);
                propTypeName = underlyingType != null ? underlyingType.Name : tmiType.Name;
            }

            if (tmiType.IsClass && value == null)
            {
                _setValue(tmi, obj, null);
                return;
            }

            switch (propTypeName.ToLower())
            {
                case "string": _setValue(tmi, obj, Convert.ToString(value)); break;
                case "char": _setValue(tmi, obj, Convert.ToChar(value)); break;
                case "byte": _setValue(tmi, obj, Convert.ToByte(value)); break;
                case "int": _setValue(tmi, obj, Convert.ToInt32(value)); break;
                case "int16": _setValue(tmi, obj, Convert.ToInt16(value)); break;
                case "int32": _setValue(tmi, obj, Convert.ToInt32(value)); break;
                case "int64": _setValue(tmi, obj, Convert.ToInt64(value)); break;
                case "uint16": _setValue(tmi, obj, Convert.ToUInt16(value)); break;
                case "uint32": _setValue(tmi, obj, Convert.ToUInt32(value)); break;
                case "uint64": _setValue(tmi, obj, Convert.ToUInt64(value)); break;
                case "long": _setValue(tmi, obj, Convert.ToInt64(value)); break;
                case "decimal": _setValue(tmi, obj, Convert.ToDecimal(value)); break;
                case "single": _setValue(tmi, obj, Convert.ToSingle(value)); break;
                case "double": _setValue(tmi, obj, Convert.ToDouble(value)); break;
                case "boolean":
                case "bool":
                    _setValue(tmi, obj, Convert.ToBoolean(value));
                    break;
                case "datetime":
                case "date":
                    _setDateValue(smi, obj, tmi, value, options.Settings);
                    break;
                case "guid":
                    _setValue(tmi, obj, new Guid((string)value));
                    break;
                default:
                    if (tmiType.IsEnum && value is string)
                        value = Enum.Parse(tmiType, value.ToString());

                    _setValue(tmi, obj, value);
                    break;
            }
        }

        static void _setValue(MemberInfo mi, object obj, object value)
        {
            if (mi is PropertyInfo) ((PropertyInfo)mi).SetValue(obj, value, null);
            if (mi is FieldInfo) ((FieldInfo)mi).SetValue(obj, value);
        }

        private static void _setDateValue(
            MemberInfo smi,
            object obj,
            MemberInfo tmi,
            object value,
            Dictionary<string, object> Settings)
        {
            if (smi == null || tmi == null || obj == null) return;

            Type tmiType = MemberInfoType(tmi);
            Type smiType = MemberInfoType(smi);

            var underlyingType = Nullable.GetUnderlyingType(tmiType);

            if (tmiType.IsGenericType && (underlyingType == null || !underlyingType.Name.StartsWith("Date")))
                return;

            if (!tmiType.IsGenericType && !tmiType.Name.StartsWith("Date"))
                return;

            var srcTZAttr = (TimeZoneAttribute)smi.GetCustomAttribute(typeof(TimeZoneAttribute), false);
            var tgtTZAttr = (TimeZoneAttribute)tmi.GetCustomAttribute(typeof(TimeZoneAttribute), false);

            if (srcTZAttr != null && tgtTZAttr != null)
            {
                var srcTz = _getTimeZoneId(srcTZAttr.Id, Settings);
                var tgtTz = _getTimeZoneId(tgtTZAttr.Id, Settings);

                var srcDate = Convert.ToDateTime(value);

                if (srcDate.Kind != DateTimeKind.Unspecified)
                    srcDate = DateTime.SpecifyKind(srcDate, DateTimeKind.Unspecified);

                var tgtDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(srcDate, srcTz, tgtTz);

                _setValue(tmi, obj, tgtDate);
            }
            else
            {
                _setValue(tmi, obj, Convert.ToDateTime(value));
            }
        }

        static private Dictionary<string, TimeZoneInfo> _systemTZcache = null;
        static private TimeZoneInfo FindSysTimeZoneById(string id)
        {
            if (_systemTZcache == null) _systemTZcache = TimeZoneInfo.GetSystemTimeZones().ToDictionary(x => x.Id);
            if (_systemTZcache.ContainsKey(id)) return _systemTZcache[id]; else return null;
        }

        static private string _getTimeZoneId(string tzId, Dictionary<string, object> Settings)
        {
            if (string.IsNullOrWhiteSpace(tzId)) return TimeZoneInfo.Local.Id;
            string timeZone;
            timeZone = FindSysTimeZoneById(tzId)?.Id;
            if (timeZone == null) timeZone = Settings.ContainsKey(tzId) ? Settings[tzId] as string : TimeZoneInfo.Local.Id;
            return timeZone;
        }

        private static bool IsNullOrWhiteSpace(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
