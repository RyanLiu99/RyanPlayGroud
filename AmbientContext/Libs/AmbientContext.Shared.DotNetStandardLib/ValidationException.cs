using System;

namespace AmbientContext.Shared.DotNetStandardLib
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base (message)
        {
                
        }
    }
}
