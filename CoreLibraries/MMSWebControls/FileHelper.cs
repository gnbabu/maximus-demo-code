using System;
using System.Configuration;
using System.IO;

namespace MMSWebControls
{
	public static class FileHelper
	{
		private static int? _maxFilenameLength;
		private const short DEFAULT_MAX_FILENAME_LENGTH = 120;

		public static int MaxFilenameLength
		{
			get
			{
				if (!_maxFilenameLength.HasValue)
				{
					string tempMaxFilenameLength = ConfigurationManager.AppSettings["MaxFilenameLength"];
					int temp;

					if (string.IsNullOrEmpty(tempMaxFilenameLength) || !int.TryParse(tempMaxFilenameLength, out temp))
					{
						_maxFilenameLength = DEFAULT_MAX_FILENAME_LENGTH;
					}
					else
					{
						_maxFilenameLength = temp;
					}
				}

				return _maxFilenameLength.Value;
			}
		}

		public static string RenameFileWithGUID(string filename)
		{
			Guid guid = Guid.NewGuid();

			return string.Format("{0}.{1}{2}", TrimFilenameIfNecessary(Path.GetFileNameWithoutExtension(filename)), guid, Path.GetExtension(filename));
		}

		private static string TrimFilenameIfNecessary(string fileName)
		{
			if (fileName.Length <= MaxFilenameLength)
			{
				return fileName;
			}
			else
			{
				return fileName.Substring(0, MaxFilenameLength);
			}
		}
	}
}
