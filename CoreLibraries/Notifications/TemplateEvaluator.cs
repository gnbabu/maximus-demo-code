using System;
using System.Collections.Generic;

using System.IO;
using System.Text.RegularExpressions;


namespace MAXIMUS.Core.Libraries
{
    public sealed class TemplateEvaluator
    {
        #region private fields

        // Moved regular expression pattern to a read only variable to see if this helps to mitigate the Sonarqube security hotspots medium issue
        private static readonly string pattern = @"\[{2}\s*(?<field>((?<parts>\w+)\.)*(?<parts>\w+))(?([,:])(?'format'[,:][^\]]+))\s*\]{2}";

        private static Regex _rxFields = new Regex(
            pattern,
            RegexOptions.IgnoreCase |
            RegexOptions.ExplicitCapture |
            RegexOptions.Compiled);

        private string _source = string.Empty;
        private TemplateWriter _writer = DefaultWriter;
        private List<string> _fieldNames = null;

        #endregion

        #region "Constructors"
        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public TemplateEvaluator()
        {
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
        }

        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public TemplateEvaluator(string source, Guid threadId)
        {
            this.ThreadId = threadId;
            _source = source;
            Parse();
        }


        #region "Constructors"



        #endregion

        #region "Logging Objects"


        private Guid threadId;
        private Guid ThreadId
        {
            get
            {
                return this.threadId;
            }
            set
            {
                this.threadId = value;
            }
        }

        #endregion

        /// <summary>
        /// The template source
        /// </summary>
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                Parse();
            }
        }

        /// <summary>
        /// Loads a template from a file.
        /// </summary>
        /// <param name="fileName">The full path to the file containing the template source.</param>
        public void Load(string fileName)
        {
            //using (StreamReader reader = File.OpenText(fileName))
            try
            {

                using (StreamReader reader = new StreamReader(fileName))
                {
                    Load(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Load(TextReader reader)
        {
            try
            {
                _source = reader.ReadToEnd();
                Parse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<string> FieldNames
        {
            get
            {
                return _fieldNames as IList<string>;
            }
        }

        public string Eval(TemplateFieldAccessor accessor)
        {

            StringWriter sw = new StringWriter();
            try
            {

                _writer(sw, accessor);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sw.ToString();
        }

        public void Eval(TextWriter writer, TemplateFieldAccessor accessor)
        {
            try
            {
                _writer(writer, accessor);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region private helpers

        /// <summary>
        /// Parses the template using a regular expression and converts it
        /// to a series of delegate calls that render the template. After this
        /// method is executed the member variable _writer will reference the
        /// root level writer that renders the template.
        /// </summary>
        private void Parse()
        {
            try
            {
                MatchCollection sections = _rxFields.Matches(_source);
                Dictionary<string, TemplateWriter> writerLookup = new Dictionary<string, TemplateWriter>();
                List<TemplateWriter> writerList = new List<TemplateWriter>();
                _fieldNames = new List<string>(sections.Count);

                int curIndex = 0;
                foreach (Match m in sections)
                {
                    _fieldNames.Add(m.Groups["field"].Value);

                    if (m.Index > curIndex)
                    {
                        // create a literal from curIndex to m.Index
                        string literal = _source.Substring(curIndex, m.Index - curIndex);
                        writerList.Add(LiteralWriter(literal));
                    }

                    string field = m.Value;
                    TemplateWriter writer;
                    if (!writerLookup.TryGetValue(field, out writer))
                    {
                        CaptureCollection parts = m.Groups["parts"].Captures;
                        string format = m.Groups["format"].Value.Trim();
                        if (string.IsNullOrEmpty(format))
                            writer = FieldWriter(parts);
                        else
                        {
                            format = "{0" + format + "}";
                            writer = FieldWriter(parts, format);
                        }
                        writerLookup.Add(field, writer);
                    }
                    writerList.Add(writer);

                    curIndex = m.Index + m.Length;
                }

                if (curIndex < _source.Length)
                {
                    // generate a literal for the remaining text
                    string literal = _source.Substring(curIndex);
                    writerList.Add(LiteralWriter(literal));
                }

                // Create an aggregate writer that calls all the
                // writers that make up the template
                TemplateWriter[] writers = writerList.ToArray();
                _writer = delegate(TextWriter textWriter, TemplateFieldAccessor root)
                {
                    for (int i = 0; i < writers.Length; i++)
                        writers[i](textWriter, root);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Creates a delegate that writes a literal string to a TextWriter 
        /// </summary>
        /// <param name="literal">The literal string to write.</param>
        /// <returns>A new writer delegate.</returns>
        private static TemplateWriter LiteralWriter(string literal)
        {
            return delegate(TextWriter writer, TemplateFieldAccessor root)
            {
                try
                {
                    writer.Write(literal);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            };
        }

        /// <summary>
        /// Creates a delegate that writes the value of a template field to a TextWriter
        /// </summary>
        /// <param name="parts">The parsed parts of the field name.</param>
        /// <param name="format">The format to be used when converting the field value to a string.</param>
        /// <returns>A new writer delegate.</returns>
        private static TemplateWriter FieldWriter(CaptureCollection parts, string format)
        {

            return delegate(TextWriter writer, TemplateFieldAccessor root)
            {
                try
                {
                    writer.Write(string.Format(format, GetFieldValue(root, parts)));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }

        private static TemplateWriter FieldWriter(CaptureCollection parts)
        {
            return delegate(TextWriter writer, TemplateFieldAccessor root)
            {
                try
                {
                    object fieldValue = GetFieldValue(root, parts);
                    if (fieldValue != null)
                        writer.Write(fieldValue.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }

        private static object GetFieldValue(TemplateFieldAccessor root, CaptureCollection parts)
        {
            object value;
            TemplateFieldAccessor accessor = root;
            try
            {
                for (int i = 0; i < parts.Count - 1; i++)
                {
                    Capture part = parts[i];
                    value = accessor(part.Value);

                    // if the accessor didnt return an accessor then
                    // wrap the value in an object accessor
                    if (!(value is TemplateFieldAccessor))
                        value = TemplateFieldAccessors.PropertyAccessor(value);

                    accessor = (TemplateFieldAccessor)value;
                }

                value = accessor(parts[parts.Count - 1].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        /// <summary>
        /// Represents a function that renders a compiled template.
        /// </summary>
        /// <param name="writer">The writer to render the template to.</param>
        /// <param name="root">A delegate used to retrieve field values.</param>
        private delegate void TemplateWriter(TextWriter writer, TemplateFieldAccessor root);

        private static void DefaultWriter(TextWriter writer, TemplateFieldAccessor accessor)
        {
        }

        #endregion
    }
}
