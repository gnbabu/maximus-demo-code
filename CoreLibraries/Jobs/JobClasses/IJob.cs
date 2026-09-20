using System;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    /// Summary description for IJob
    /// </summary>
    public interface IJob
    {
        /// <summary>
        ///     If only a single method will exist for a class use this method
        /// </summary>
        void ExecuteJob();

        /// <summary>
        ///     If a class contains multiple possible methods which can be executed
        ///         pass a JobId to indicate which method to execute
        /// </summary>
        /// <param name="jobId">The Guid of the job to execute</param>
        void ExecuteJob(Guid jobId);
    }
}