using System;
using System.Runtime.Serialization;

namespace DotNetCore.API.Common.Exceptions
{
    public class DotNetCoreAPICommonException : Exception
    {
        public DotNetCoreAPICommonException()
        {
        }

        public DotNetCoreAPICommonException(string message) : base(message)
        {
        }

        public DotNetCoreAPICommonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DotNetCoreAPICommonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
