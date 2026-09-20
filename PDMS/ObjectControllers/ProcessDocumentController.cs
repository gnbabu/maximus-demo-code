using System;
using System.Data;

namespace MAXIMUS.Controllers.PDMS
{
    public static class ProcessDocumentController
    {
        public static bool GenerateDiddContract(int regID, out string fileName)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.GenerateDIDDContract(regID, out fileName);
        }

        public static bool GenerateICFIIDContract(int regID, out string fileName)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.GenerateICF_IDDContract(regID, out fileName);
        }

        public static bool GenerateApplication(int regID, string userName, bool saveDocument, out string fileName)
        {
           Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.GenerateApplication(regID, userName, saveDocument, out fileName);
        }

        public static string CreatePDFFromHTML(string htmlPath, string fileName, bool IsRetrieveReports = false)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.ConvertHTMLToPDF(htmlPath, fileName, IsRetrieveReports);
        }

        public static string CreatePDFFromHTMLRAmettiance(string htmlPath, string fileName)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.ConvertHTMLToPDFRemittance(htmlPath, fileName);
        }

        public static bool Generate1099WithPayerInfo(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName, string payerName, string payerAddress1, string payerAddress2, string payerCity, string payerState, string payerZip)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.Generate1099(regID, taxId, fedWithhold, payments, year, userName, appPath, out pdfFileName, payerName, payerAddress1, payerAddress2, payerCity, payerState, payerZip);
        }

        public static bool Generate1099(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName)
        {
            Guid logThreadId = Guid.NewGuid();

            MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments doc = new MAXIMUS.ProcessDocuments.PDMS.ProcessDocuments(logThreadId);

            return doc.Generate1099(regID, taxId, fedWithhold, payments, year, userName, appPath, out pdfFileName);
        }
    }
}
