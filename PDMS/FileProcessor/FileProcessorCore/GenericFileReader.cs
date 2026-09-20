using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace FileProcessorCore
{
	public class GenericFileReader : IDisposable
	{
		#region Private Members

		private StreamReader _streamReader;

		private TextFieldParser _textFielParser;

		#endregion

		#region Properties

		public string FileName { get; set; }

		public FileFormat Format { get; set; }

		public bool EndOfStream
		{
			get
			{
				switch (Format)
				{
					case FileFormat.FixedLength:
						return _streamReader.EndOfStream;
					case FileFormat.CSV:
						return _textFielParser.EndOfData;
					default:
						throw new ArgumentException(string.Format("Unsupported FileFormat: {0}", Format));
				}
			}
		}

		#endregion

		#region Constructors
		
		public GenericFileReader(string filename, FileFormat format)
		{
			FileName = filename;
			Format = format;

			switch (format)
			{
				case FileFormat.FixedLength:
					_streamReader = new StreamReader(filename);
					break;
				case FileFormat.CSV:
					_textFielParser = new TextFieldParser(filename);
					/// TODO: Allow to be configurable?
					_textFielParser.SetDelimiters(",");
					break;
				default:
					throw new ArgumentException(string.Format("Unsupported FileFormat: {0}", format));
			}
		}

		#endregion

		#region Public Methods
		
		public void Dispose()
		{
			if (_streamReader != null)
			{
				_streamReader.Dispose();
			}

			if (_textFielParser != null)
			{
				_textFielParser.Dispose();
			}
		}

		public string ReadLine()
		{
			switch (Format)
			{
				case FileFormat.FixedLength:
					return _streamReader.ReadLine();
				case FileFormat.CSV:
					return _textFielParser.ReadLine();
				default:
					throw new ArgumentException(string.Format("Unsupported FileFormat: {0}", Format));
			}
		}

		public string[] ReadFields()
		{
			switch (Format)
			{
				case FileFormat.FixedLength:
					throw new Exception("ReadLineArray not supported for fixed length files.");
				case FileFormat.CSV:
					return _textFielParser.ReadFields();
				default:
					throw new ArgumentException(string.Format("Unsupported FileFormat: {0}", Format));
			}
		}

		#endregion
	}
}
