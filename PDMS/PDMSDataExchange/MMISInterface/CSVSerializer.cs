using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    public class CsvIgnoreAttribute : Attribute { }

    /// <summary>
    /// Class for serializing and deserialing an object to CSV.
    /// </summary>
    public class CSVSerializer<T> where T : class, new()
    {
        public string Separator { get; set; }
        public string EnumerableSeparator { get; set; }
        public string Replacement { get; set; }
        public string NewLine { get; set; }
        protected List<PropertyInfo> _properties;

        /// <summary>
        /// The CSVSerializer constructor, get a list of all proeperties of the type <T>.
        /// </summary>
        public CSVSerializer()
        {
            var type = typeof(T);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance
                 | BindingFlags.GetProperty | BindingFlags.SetProperty);

            _properties = (from a in properties
                           where a.GetCustomAttributes(typeof(CsvIgnoreAttribute), false).Count() == 0
                           orderby a.Name
                           select a).ToList();
        }

        /// <summary>
        /// Write the header information, this will most likely be a property list on the first line 
        /// which will be used as columns.
        /// </summary>
        /// <param name="sb"> The stringbuilder to write the header information to</param>
        protected virtual void SerializeHeader(StringBuilder sb)
        {   
        }

        /// <summary>
        /// Read the header information, this will most likely read a property list from the first line 
        /// which will be used as columns.
        /// </summary>
        /// <param name="sr"> The string reader to write the header information to</param>
        protected virtual void DeserializeHeader(StreamReader sr)
        {
        }

        /// <summary>
        /// Returns the property at given index.
        /// </summary>
        /// <param name="index"> The index from which to read the property.</param>
        /// <returns> Returns the property info.</returns>
        protected virtual PropertyInfo GetProperty(int index)
        {
            return _properties[index];
        }

        /// <summary>
        /// Write properties from the given object list to the stream separated by Separator
        /// </summary>
        /// <param name="stream"> The stringbuilder to write.</param>
        /// <param name="data"> The data to be written.</param>
        public void Serialize(Stream stream, IList<T> data)
        {
            var sb = new StringBuilder();
            var values = new List<string>();

            // Write any header information
            this.SerializeHeader(sb);

            foreach (var item in data)
            {
                values.Clear();

                // Get all properties on the object
                var properties = item.GetType().GetProperties()
                    .Where(x => x.CanRead)
                    .Where(x => x.GetCustomAttributes(typeof(CsvIgnoreAttribute), false).Count() == 0)
                    .OrderBy(x => x.Name)
                    .ToDictionary(x => x.Name, x => x.GetValue(item, null));

                // Get names for all IEnumerable properties (excluding string)
                var enumerablePropertiesNames = properties
                    .Where(x => !(x.Value is string) && x.Value is IEnumerable<dynamic>)
                    .Select(x => x.Key)
                    .ToList();

                // Concat all IEnumerable properties into a <separator> separated string
                foreach (var key in enumerablePropertiesNames)
                {
                    var valueType = properties[key].GetType();
                    var valueElemType = valueType.IsGenericType
                                            ? valueType.GetGenericArguments()[0]
                                            : valueType.GetElementType();
                    if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                    {
                        var enumerable = properties[key] as IEnumerable<dynamic>;
                        properties[key] = string.Join(EnumerableSeparator, enumerable.Cast<object>());
                    }
                }

                sb.AppendLine(string.Join(Separator, properties
                .Select(x => x.Value == null ?
                        "" :
                        x.Value is DateTime ? ((DateTime)x.Value).ToString("s", CultureInfo.InvariantCulture) : x.Value.ToString().Replace(Separator, Replacement))));
            }
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(sb.ToString().Trim());
            }
        }

        /// <summary>
        /// Read data from the stream and construct a list of objects.
        /// </summary>
        /// <param name="stream"> The stringbuilder to write.</param>
        /// <param name="data"> The data to be written.</param>
        /// <returns> Return list of objects read from the stream.</returns>
        public IList<T> Deserialize(Stream stream)
        {
            string[] rows = new string[_properties.Count];

            try
            {
                using (var sr = new StreamReader(stream))
                {
                    this.DeserializeHeader(sr);
                    rows = sr.ReadToEnd().Split(new string[] { NewLine }, StringSplitOptions.None);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The CSV File is Invalid. See Inner Exception for more inoformation.", ex);
            }

            var data = new List<T>();
            for (int row = 0; row < rows.Length; row++)
            {
                var line = rows[row];
                if (string.IsNullOrWhiteSpace(line))
                {
                   throw new Exception(string.Format(@"Error: Empty line at line number: {0}", row));
                }

                var parts = line.Split(Separator.ToCharArray());

                var datum = new T();
                for (int i = 0; i < parts.Length; i++)
                {
                    var value = parts[i];

                    // TODO Construct enumerable properties

                    value = value.Replace(Replacement, Separator.ToString());

                    var property = this.GetProperty(i);

                    var converter = TypeDescriptor.GetConverter(property.PropertyType);

                    property.SetValue(datum, value == "" ? null : converter.ConvertFrom(value), null);
                }
                data.Add(datum);
            }
            return data;
        }
    }

    //// Move it to the test framework whenver we get one.
    //public class SomeClass
    //{
    //    public string Category { get; set; }
    //    public int NumberOfItems { get; set; }
    //    public DateTime BirthDate { get; set; }
    //    public string[] Children { get; set; }
    //    [CsvIgnoreAttribute]
    //    public string IgnoreMe { get; set; }
    //}

    //public class TestClass
    //{
    //    public void TestCSVSerializer()
    //    {
    //        CSVSerializer<SomeClass> serializer = new CSVSerializer<SomeClass>()
    //        {
    //            Separator = "|",
    //            NewLine = Environment.NewLine,
    //            EnumerableSeparator = ",",
    //            Replacement = "_"
    //        };
    //        SomeClass test = new SomeClass()
    //        {
    //            Category = "Shoes",
    //            NumberOfItems = 20,
    //            BirthDate = DateTime.Now,
    //            IgnoreMe = "sensitive info don't send to anyone"
    //        };
    //        IList<SomeClass> testList = new List<SomeClass> { test };
    //        Stream stream = new FileStream("c:\\\\Test\\a.csv", FileMode.Create);
    //        serializer.Serialize(stream, testList);

    //        Stream readStream = new FileStream("c:\\\\Test\\a.csv", FileMode.Open);
    //        serializer.Deserialize(readStream);
    //        Console.ReadKey();
    //    }
    //}
}