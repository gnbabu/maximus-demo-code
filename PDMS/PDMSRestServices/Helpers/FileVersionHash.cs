using Microsoft.AspNetCore.Hosting;
using ReportingService.Common.Text;
using System.IO;
using System.Web;

namespace MainBoldReportsAPI.Web.Helper
{
    /// <summary>
    /// Generates a simple hash from the file last modified date/time and size for the purpose of cache busting.
    /// </summary>
    public class FileVersionHash
    {
        readonly IWebHostEnvironment _env;

        public FileVersionHash(IWebHostEnvironment env)
        {
            _env = env;
        }

        //Calculate murmurHash for file based on last modified date and time and size
        public string CalcHash(string path)
        {
            var file = new FileInfo(path);
            var lastModified = file.LastWriteTimeUtc;
            var size = file.Length;
            var hash = MurmurHash.Hash($"{lastModified}{size}");
            return hash ?? "";
        }

        /// <summary>
        /// Calculate version hash for a file resource relative to Web Root
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public string VersionedResource(string resourcePath)
        {
            var filePathAndName = resourcePath.StartsWith("/") ? resourcePath.Substring(1) : resourcePath;
            filePathAndName = filePathAndName.StartsWith("\\") ? filePathAndName.Substring(1) : filePathAndName;
            filePathAndName = filePathAndName.Replace("/", "\\");
            var path = System.IO.Path.Combine(_env.WebRootPath, filePathAndName);
            var hash = HttpUtility.UrlEncode(CalcHash(path));
            return $"{resourcePath}?v={hash}";
        }
    }
}
