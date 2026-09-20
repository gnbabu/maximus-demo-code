//using Quartz;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MAXIMUS.Core.Libraries.QuartzJobs
//{

//    /// <summary>
//    ///     Author(s):      Ahackl
//    ///     Date:           2017.06.12
//    ///     Name:           QuartzJob
//    ///     Description:    SFTP Set
//    /// </summary>
//    [DisallowConcurrentExecution]
//    public class SFTPSetJob : QuartzJob
//    {
//        /// <summary>
//        ///     Author(s):      Ahackl
//        ///     Date:           2017.06.12
//        ///     Name:           QuartzJob
//        ///     Description:    Job Type Id
//        /// </summary>
//        public override int JobTypesID
//        {
//            get { return 6; }
//        }

//        /// <summary>
//        ///     Author(s):      Ahackl
//        ///     Date:           2017.06.12
//        ///     Name:           QuartzJob
//        ///     Description:    Main Execution Method
//        /// </summary>
//        public override void Execute(Quartz.IJobExecutionContext context)
//        {
//            string filePath = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FilePathAppSettingsKey"));
//            string fileName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("FileNameAppSettingsKey"));
//            string hostName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("HostNameAppSettingsKey"));
//            string remotePath = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("RemotePathAppSettingsKey"));
//            string userName = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("UserNameAppSettingsKey"));
//            string password = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("PasswordAppSettingsKey"));
//            string wildCard = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("WildCardAppSettingsKey"));
//            string hostKey = AppSettings.Get((string)context.JobDetail.JobDataMap.Get("HostKeyAppSettingsKey"));

//            // create a file transport (sFTP send) object
//            FileTransport fileTransport = new FileTransport(Guid.NewGuid());

//            // send the document
//            fileName = fileName.Replace(wildCard, "*");
//            string fullName = filePath + fileName;
//            fileTransport.SetFileViaSFTP(filePath, fileName, hostName, remotePath, userName
//                , password, hostKey, true);
//        }
//    }
//}
