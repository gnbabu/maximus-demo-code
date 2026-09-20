using System;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSWebControls
{
    [ToolboxData("<{0}:EncryptedFileUpload runat=server></{0}:EncryptedFileUpload>")]
    public class EncryptedFileUpload : FileUpload
    {
		public bool EncryptionEnabled
		{
			get
			{
				if (ViewState["EncryptionEnabled"] == null)
					ViewState["EncryptionEnabled"] = Convert.ToBoolean(WebConfigurationManager.AppSettings["FileUploadEncryptionEnabled"]);

				return (bool)ViewState["EncryptionEnabled"];
			}
			set
			{
				ViewState["EncryptionEnabled"] = value;
			}
		}


		public byte[] EncryptedFileBytes
		{
			get
			{
				if (FileBytes == null)
				{
					return null;
				}

				if (EncryptionEnabled)
				{
					Encryption encryption = new Encryption();
					return encryption.EncryptRijndael(FileBytes);
				}
				else
				{
					return FileBytes;
				}
			}
		}
    }
}
