using StructureMap;
using System;
using System.Collections.Generic;

namespace MAXIMUS.Services.PDMS.Validation
{
    public interface IValidationFactory
    {
        ValidationResult Validate<T>(T obj);
    }

    public class ValidationFactory : IValidationFactory
    {
        public ValidationResult Validate<T>(T obj)
        {
            try
            {
                var validator = ObjectFactory.GetInstance<IValidator<T>>();
                return validator.Validate(obj);
            }
            catch (Exception ex)
            {
                var messages = new List<ValidationMessage> {new ValidationMessage {
                Message = string.Format("Error validating {0}", obj)}};

                messages.AddRange(FlattenError(ex));

                var result = new ValidationResult { Messages = messages };
                return result;
            }
        }

        private static IEnumerable<ValidationMessage> FlattenError(Exception exception)
        {
            var messages = new List<ValidationMessage>();
            var currentException = exception;

            do
            {
                messages.Add(new ValidationMessage { Message = exception.Message });
                currentException = currentException.InnerException;
            } while (currentException != null);

            return messages;
        }
    }
}