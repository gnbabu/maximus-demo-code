using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    class CSVSerializerWithHeader<T> : CSVSerializer<T> where T : class, new()
    {
        private string[] columns;

        /// <summary>
        /// Write the header information, this will most likely be a property list on the first line 
        /// which will be used as columns.
        /// </summary>
        /// <param name="sb"> The stringbuilder to write the header information to</param>
        protected new void SerializeHeader(StringBuilder sb)
        {
            var columns = _properties.Select(a => a.Name).ToArray();
            var header = string.Join(Separator.ToString(), columns);
            sb.AppendLine(header);
        }

        /// <summary>
        /// Read the header information, this will most likely read a property list from the first line 
        /// which will be used as columns.
        /// </summary>
        /// <param name="sr"> The string reader to write the header information to</param>
        protected new void DeserializeHeader(StreamReader sr)
        {
            columns = sr.ReadLine().Split(Separator.ToCharArray());
        }

        /// <summary>
        /// Returns the property at given index.
        /// </summary>
        /// <param name="index"> The index from which to read the property.</param>
        /// <returns> Returns the property info.</returns>
        protected new PropertyInfo GetProperty(int index)
        {
            return _properties.First(a => a.Name == columns[index]); ;
        }
    }
}
