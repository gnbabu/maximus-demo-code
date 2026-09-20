using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries.Helper
{
    public class MimeHelper
    {
        public static string GetMimeTypeForFileName(string f)
        {

            string ext = System.IO.Path.GetExtension(f);

            switch (ext)
            {
                case ".aac":
                    return ("audio/aac");

                case ".abw":
                    return ("application/x-abiword");

                case ".arc":
                    return ("application/x-freearc");

                case ".avi":
                    return ("video/x-msvideo");

                case ".azw":
                    return ("application/vnd.amazon.ebook");

                case ".bin":
                    return ("application/octet-stream");

                case ".bmp":
                    return ("image/bmp");

                case ".bz":
                    return ("application/x-bzip");

                case ".bz2":
                    return ("application/x-bzip2");

                case ".csv":
                    return ("text/csv");

                case ".doc":
                    return ("application/msword");

                case ".docx":
                    return ("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

                case ".eot":
                    return ("application/vnd.ms-fontobject");

                case ".epub":
                    return ("application/epub+zip");

                case ".gz":
                    return ("application/gzip");

                case ".gif":
                    return ("image/gif");

                case ".html":
                    return ("text/html");

                case ".ico":
                    return ("image/vnd.microsoft.icon");

                case ".ics":
                    return ("text/calendar");

                case ".jpeg":
                    return ("image/jpeg");

                case ".jpg":
                    return ("");

                case ".json":
                    return ("application/json");

                case ".jsonld":
                    return ("application/ld+json");

                case ".mp3":
                    return ("audio/mpeg");

                case ".mpeg":
                    return ("video/mpeg");

                case ".mpkg":
                    return ("application/vnd.apple.installer+xml");

                case ".odp":
                    return ("application/vnd.oasis.opendocument.presentation");

                case ".ods":
                    return ("application/vnd.oasis.opendocument.spreadsheet");

                case ".odt":
                    return ("application/vnd.oasis.opendocument.text");

                case ".oga":
                    return ("audio/ogg");

                case ".ogv":
                    return ("video/ogg");

                case ".ogx":
                    return ("application/ogg");

                case ".opus":
                    return ("audio/opus");

                case ".otf":
                    return ("font/otf");

                case ".png":
                    return ("image/png");

                case ".pdf":
                    return ("application/pdf");

                case ".php":
                    return ("application/x-httpd-php");

                case ".ppt":
                    return ("application/vnd.ms-powerpoint");

                case ".pptx":
                    return ("application/vnd.openxmlformats-officedocument.presentationml.presentation");

                case ".rar":
                    return ("application/vnd.rar");

                case ".rtf":
                    return ("application/rtf");

                case ".svg":
                    return ("image/svg+xml");

                case ".swf":
                    return ("application/x-shockwave-flash");

                case ".tar":
                    return ("application/x-tar");

                case ".tif":
                    return ("image/tiff");

                case ".tiff":
                    return ("");

                case ".ts":
                    return ("video/mp2t");

                case ".ttf":
                    return ("font/ttf");

                case ".txt":
                    return ("text/plain");

                case ".vsd":
                    return ("application/vnd.visio");

                case ".wav":
                    return ("audio/wav");

                case ".weba":
                    return ("audio/webm");

                case ".webm":
                    return ("video/webm");

                case ".webp":
                    return ("image/webp");

                case ".xls":
                    return ("application/vnd.ms-excel");

                case ".xlsx":
                    return ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                case ".zip":
                    return ("application/zip");

                case ".3gp":
                    return ("video/3gpp");

                case "":
                    return ("audio/3gpp if it doesn't contain video");

                case ".3g2":
                    return ("video/3gpp2");

                case ".7z":
                    return ("application/x-7z-compressed");

            }
            // default to plain text
            return ("text/plain");

        }
    }
}
