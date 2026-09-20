using System;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseJob : IDisposable
    {

        // static directories
        public static class JobExceptionText
        {
            public const string JobIdNotImplemented = "Job Id does not exist for this class";
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ThreadId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadId"></param>
        public BaseJob(Guid threadId)
        {
            ThreadId = threadId;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ExecuteJob()
        {
            // no action
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        public virtual void ExecuteJob(Guid jobId)
        {
            // no action
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
               // no action
        }
    }
}
