using System;

namespace MAXIMUS.Presentation.PDMS
{
    /// <summary>
    /// Check is a class that is concerned with validating inputs. This class
    /// should be moved into a new library at some point, but for now, this 
    /// is a demo.
    /// </summary>
    public class Check
    {
        public static void IsNotNull(object objectToValidate, string message)
        {
            if (objectToValidate == null)
                throw new ArgumentNullException(message);
        }
    }
}
