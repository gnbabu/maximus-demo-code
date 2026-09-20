using System.Collections.Generic;
using System.Linq;

namespace MAXIMUS.Services.PDMS.Validation
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T obj);
        ValidationResult Validate(T obj, bool suppressWarnings);
    }

    public sealed class ValidationMessage
    {
        public string Message { get; internal set; }
        public bool Warning { get; internal set; }

        internal ValidationMessage()
        {
        }
    }

    public sealed class ValidationResult
    {
        private bool _dirty = true;
        private bool _valid;

        public bool Valid
        {
            get
            {
                if (_dirty)
                {
                    _valid = Messages.FirstOrDefault(msg => msg.Warning == false) == null;
                    _dirty = false;
                }

                return _valid;
            }
        }

        public ValidationResult()
        {
            Messages = new List<ValidationMessage>();
        }

        public IList<ValidationMessage> Messages { get; internal set; }

        /// <summary>
        /// Adds the error. 
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void AddError(string errorMessage)
        {
            _dirty = true;
            Messages.Add(new ValidationMessage { Message = errorMessage });
        }

        /// <summary>
        /// Adds the warning. 
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        public void AddWarning(string warningMessage)
        {
            //No need to mark collection dirty since warnings never generate validation changes
            Messages.Add(new ValidationMessage { Message = warningMessage, Warning = true });
        }
    }


}
