using NetForemost.SharedKernel.Interfaces;
using System.Runtime.Serialization;

namespace NetForemost.Core.Exceptions;

public abstract class CodedException : Exception, ICodedException
{
    protected CodedException()
    {
    }

    protected CodedException(string? message) : base(message)
    {
    }

    protected CodedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    protected CodedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public abstract string Code { get; }
}