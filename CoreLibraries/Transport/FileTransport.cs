//using System;
//using System.IO;
//using System.Reflection;

//// if deployed at a later date set reference to .\ThirdParty\zip-v1.9-Reduced\Release\Ionic.Zip.Reduced.dll
//// using Ionic.Zip;

//// Database objects generated for this class:
////  TABLEs:    
////  SPs:        
////  VIEWs:  
////  FUNCTIONs:  

//namespace MAXIMUS.Core.Libraries
//{
//    /// <summary>
//    ///     Author(s):      jfetters
//    ///     Date:           2012.02.15
//    ///     Name:           FileTransport
//    ///     Description:    This class encapsulates some of the details of sending files via various methods such as sFTP, SCP, etc.
//    /// </summary>
//    public class FileTransport
//    {

//        #region "Constructors"

//        /// <summary>
//        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
//        /// </summary>
//        public FileTransport()
//        {
//            // generate a new thread id GUID
//            this.ThreadId = Guid.NewGuid();
//        }

//        /// <summary>
//        ///     The parameterized constructor which provides the Logging threadId GUID.
//        /// </summary>
//        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
//        public FileTransport(Guid threadId)
//        {
//            this.ThreadId = threadId;
//        }

//        #endregion
//        #region "Logging Objects"
//        private Guid threadId;
//        private Guid ThreadId
//        {
//            get
//            {
//                return this.threadId;
//            }
//            set
//            {
//                this.threadId = value;
//            }
//        }

//        #endregion


        
//    }
//}
