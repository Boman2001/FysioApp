
using System;
using System.Runtime.Serialization;

namespace Core.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#nullable enable

        public ValidationException(string? message) : base(message)
        {
        }

        public ValidationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
#nullable disable
}