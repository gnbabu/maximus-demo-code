using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries.QuartzLibrary
{
    /// <summary>
    ///     Author(s):      Ahackl
    ///     Date:           2017.06.12
    ///     Name:           QuartzJob
    ///     Description:    SFTP WIth Key Job
    /// </summary>
    [DisallowConcurrentExecution]
    public class SFTPSetWithKeyJob : QuartzJob
    {
        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Job Type Id
        /// </summary>
        public override int JobTypesID
        {
            get { return 2; }
        }

        /// <summary>
        ///     Author(s):      Ahackl
        ///     Date:           2017.06.12
        ///     Name:           QuartzJob
        ///     Description:    Main Execution Method
        /// </summary>
        public override void Execute(Quartz.IJobExecutionContext context)
        {
            // TODO: Implement
            //string filePath = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FilePathAppSettingsKey"));
            //string fileName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FileNameAppSettingsKey"));
            //string hostName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("HostNameAppSettingsKey"));
            //string remotePath = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("RemotePathAppSettingsKey"));
            //string userName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("UserNameAppSettingsKey"));
            //string password = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("PasswordAppSettingsKey"));
            //string hostKey = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("HostKeyAppSettingsKey"));
            //int portNumber = Convert.ToInt32(AppSettings.Get((string)context.JobDetail.JobDataMap.Get("PortNumberAppSettingKey")));
            //string privateKey = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("PrivateKeyAppSettingKey"));

            //// create a file transport (sFTP send) object
            //FileTransport fileHelper = new FileTransport(Guid.NewGuid());

            //// send the document
            //fileHelper.SetFileViaSFTP(filePath, fileName, hostName, remotePath, userName, password, hostKey, portNumber, privateKey);
        }
    }
}
