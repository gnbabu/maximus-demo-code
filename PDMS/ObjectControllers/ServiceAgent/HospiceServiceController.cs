using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Controllers.PDMS
{
    public class HospiceServiceController
    {
        public static DataSet GetInquireHospiceResponse(string hospiceTrackNo, string providerMedId)
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                ds.ReadXml(templateActualPath + @"/HospiceResponse.xml");
                //ds.ReadXml(@"C:\Latest_PDMS\PDMS\ObjectControllers\ServiceAgent\HospiceResponse.xml");
                if (Convert.ToString(ds.Tables["HospiceResponse"].Rows[0]["HospiceTrackNo"]) != hospiceTrackNo)
                {
                    ds.Clear();
                }
                
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SearchHospiceResponse(long hospiceTrackNo, string providerMedId, string reciefId, string providerNpi, int offset, int count)
        {
            try
            {
                DataSet ds = new DataSet();
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                ds.ReadXml(templateActualPath + @"/SearchResponses.xml");
                //ds.ReadXml(@"C:\Latest_PDMS\PDMS\ObjectControllers\ServiceAgent\SearchResponses.xml");
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
