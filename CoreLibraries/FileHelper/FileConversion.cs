using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     This class encapsulates some of the details of document generation such as determining the save location, 
    ///         and lines of code necessary to generate other document formats including pdf.
    /// </summary>
    public class FileConversion
    {

        // set private variables
        private string shortFileName = Guid.NewGuid().ToString() + ".pdf";

        // set the constant for one inch to use throughout
        // NOTE: 72pts = 1 inch
        const double oneInch = 72;

        /// <summary>
        ///     This method generates a pdf document containing 1 or more pages based on the text from 
        ///     the pageText input parameter. The file name will be an auto generated GUID. Returns 
        ///     the name of the file without the path.
        /// </summary>
        /// <param name="filePath">The location where the file will be placed</param>
        /// <param name="pageText">An array of pages containing the text to insert into the file</param>
        /// <returns>The name of the file without the path.</returns>
        public string GeneratePDF(string filePath, string[] pagesText, string fontName, double fontSize)
        {

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // loop over all pages
            foreach (string pageText in pagesText)
            {
                // Create an empty page
                PdfPage page = document.AddPage();

                // Get an XGraphics object for drawing
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // set the document font size and font type
                XFont font = new XFont(fontName, fontSize);  // 3rd parameter =  XFontStyle.BoldItalic

                // create a new rectangle to use for the text (set 1 inch margins)
                XRect rect = new XRect(oneInch, oneInch, page.Width - (2 * oneInch), page.Height - (2 * oneInch));

                // set the rectangle surface to transparent
                gfx.DrawRectangle(XBrushes.Transparent, rect);

                // create a new text formatter which will write to the rectangle
                XTextFormatter tf = new XTextFormatter(gfx);

                // draw the text
                tf.DrawString(pageText, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            // save the document
            string fileName = filePath + shortFileName;
            document.Save(fileName);

            // return the complete filename
            return shortFileName;
        }

        /// <summary>
        ///     This method generates a pdf document containing 1 or more pages based on the text from 
        ///     the pageText input parameter. The file name will be an auto generated GUID. Returns 
        ///     the name of the file without the path.
        /// </summary>
        /// <param name="filePath">The location where the file will be placed</param>
        /// <param name="pageText">An array of pages containing the text to insert into the file</param>
        /// <returns>The name of the file without the path.</returns>
        public string GeneratePDF(string filePath, string[] pagesText)
        {
            return GeneratePDF(filePath, pagesText, GetDefaultFont(), GetDefaultFontSize());
        }

        /// <summary>
        ///     This method generates a pdf document containing 1 or more pages based on the text from 
        ///     the pageText input parameter. The file name will be an auto generated GUID. Returns 
        ///     the name of the file without the path.
        /// </summary>
        /// <param name="filePath">The location where the file will be placed</param>
        /// <param name="pageText">An array of pages containing the text to insert into the file</param>
        /// <param name="pageHeader">The location of the page header graphic</param>
        /// <returns>The name of the file without the path.</returns>
        public string GeneratePDF(string filePath, string[] pagesText, string pageHeader, string fontName, double fontSize)
        {

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // loop over all pages
            foreach (string pageText in pagesText)
            {
                // Create an empty page
                PdfPage page = document.AddPage();

                // Get an XGraphics object for drawing
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // set the document font size and font type
                XFont font = new XFont(fontName, fontSize);  // 3rd parameter =  XFontStyle.BoldItalic

                //////////////////////////////////////
                // create the page header rectangle //
                //////////////////////////////////////

                // if the page header exists
                if (File.Exists(pageHeader))
                {

                    // create the pdf XImage from the image location
                    XImage headerImage = XImage.FromFile(pageHeader);

                    // draw the header image slightly up and to the left on the pdf
                    gfx.DrawImage(headerImage, (oneInch / 2), (oneInch / 2));
                }

                //////////////////////////////////////
                // create the page content          //
                //////////////////////////////////////

                // create a new rectangle to use for the text (set 1 inch margins)
                // adding approximately an inch for the header
                XRect rect = new XRect(oneInch, (2 * oneInch), page.Width - (2 * oneInch), page.Height - (3 * oneInch));

                // set the rectangle surface to transparent
                gfx.DrawRectangle(XBrushes.Transparent, rect);

                // create a new text formatter which will write to the rectangle
                XTextFormatter tf = new XTextFormatter(gfx);

                // draw the text
                tf.DrawString(pageText, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            // save the document
            string fileName = filePath + shortFileName;
            document.Save(fileName);

            // return the complete filename
            return shortFileName;
        }

        /// <summary>
        ///     This method generates a pdf document containing 1 or more pages based on the text from 
        ///     the pageText input parameter. The file name will be an auto generated GUID. Returns 
        ///     the name of the file without the path.
        /// </summary>
        /// <param name="filePath">The location where the file will be placed</param>
        /// <param name="pageText">An array of pages containing the text to insert into the file</param>
        /// <param name="pageHeader">The location of the page header graphic</param>
        /// <returns>The name of the file without the path.</returns>
        public string GeneratePDF(string filePath, string[] pagesText, string pageHeader)
        {
            return GeneratePDF(filePath, pagesText, pageHeader, GetDefaultFont(), GetDefaultFontSize());
        }

        /// <summary>
        ///     Attempts to retrieve the default font from the web app.config and if not found defaults font
        /// </summary>
        /// <returns>The default font name</returns>
        private string GetDefaultFont()
        {
            return AppSettings.Get("pdfFont", "Verdana");
        }

        /// <summary>
        ///     Attempts to retrieve the default font size from the web app.config and if not found defaults font size
        /// </summary>
        /// <returns>The default font size</returns>
        private double GetDefaultFontSize()
        {

            // set the default font size
            double pdfFontSize = 10;

            // if the font size has been set in the application settings file
            if (AppSettings.Get("pdfFontSize") != null)
            {

                // verify the value is a number
                double Num;
                bool isNumber = double.TryParse(AppSettings.Get("pdfFontSize"), out Num);
                if (isNumber)
                {
                    // set to the value in file
                    pdfFontSize = Convert.ToDouble(AppSettings.Get("pdfFontSize"));
                }
            }

            return pdfFontSize;

        }
     }
}
