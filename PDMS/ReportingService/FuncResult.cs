using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService
{
    /// <summary>
    /// Function Result class that can be used to return a result from a function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FuncResult
    {
        protected List<string> _errors = new List<string>(2);
        public bool Success { get; protected set; }

        public string[] Errors { get { return _errors.ToArray(); } }

        public FuncResult(bool success)
        {
            Success = success;
        }
        public FuncResult(bool success, string errorMsg) : this(success)
        {
            _errors.Add(errorMsg);
        }
        public FuncResult(bool success, string[] errors) : this(success)
        {
            _errors.AddRange(errors);
        }
        public FuncResult AddError(string msg)
        {
            _errors.Add(msg);
            Success = false;
            return this;
        }
        public FuncResult AddErrors(string[] msgs)
        {
            _errors.AddRange(msgs);
            Success = false;
            return this;
        }
    }

    /// <summary>
    /// Function Result class that can be used to return a result from a function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FuncResult<T> : FuncResult
    {
        public T Value { get; set; }

        public FuncResult():base(false) { }
        public FuncResult(T value) : base(true)
        {
            Value = value;
        }
        public FuncResult(bool success, string errorMsg) : base(success, errorMsg) { }
        public FuncResult(bool success, string[] errors) : base(success, errors) { }
        public FuncResult(bool success, T value, string errorMsg) : base(success, errorMsg) {
            Value = value;
        }

        /// <summary>
        /// Sets a successful result with 'Value' = value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FuncResult<T> SetResult(T value)
        {
            Success = true;
            Value = value;
            return this;
        }

        public FuncResult<T> SetStatus(bool status)
        {
            Success = status;
            return this;
        }

        public new FuncResult<T> AddError(string msg)
        {
            base.AddError(msg);
            return this;
        }
        public new FuncResult<T> AddErrors(string[] msgs)
        {
            base.AddErrors(msgs);
            return this;
        }
    }
}
