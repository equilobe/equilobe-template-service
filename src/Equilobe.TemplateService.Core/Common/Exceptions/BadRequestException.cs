using System;
namespace Equilobe.TemplateService.Core.Common.Exceptions
{
    public class BadRequestException(string? message, Exception? innerException) : Exception(message, innerException)
    {
        public BadRequestException()
            : this(null, null) { }

        public BadRequestException(string message)
            : this(message, null) { }

        public static BadRequestException Create(
            string? message = null,
            Exception? innerException = null)
        {
            return new BadRequestException(message, innerException);
        }
    }
}

