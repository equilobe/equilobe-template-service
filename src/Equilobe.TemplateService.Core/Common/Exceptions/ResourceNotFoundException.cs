using System;
namespace Equilobe.TemplateService.Core.Common.Exceptions
{
    public class ResourceNotFoundException(
        string? message,
        Exception? innerException,
        string? resourceDescription) : Exception(message, innerException)
    {
        public string? ResourceDescription { get; } = resourceDescription;

        public ResourceNotFoundException()
            : this(null, null, null) { }

        public ResourceNotFoundException(string message)
            : this(message, null, null) { }

        public ResourceNotFoundException(string message, Exception innerException)
            : this(message, innerException, null) { }

        public static ResourceNotFoundException Create(
            string? resourceDescription = null,
            string? message = null,
            Exception? innerException = null)
        {
            return new ResourceNotFoundException(message, innerException, resourceDescription);
        }
    }
}

