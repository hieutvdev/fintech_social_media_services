using System.Runtime.Serialization;
using BuildingBlocks.CQRS.Common;

namespace BuildingBlocks.Exceptions;

public class BaseException : Exception
{
   public BaseException()
   {
      
   }

   public BaseException(string message) : base(message)
   {
      
   }

   public BaseException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args))
   {
      
   }

   protected BaseException(SerializationInfo info, StreamingContext context) : base(info, context)
   {
      
   }

   public BaseException(string message, Exception innerException) : base(message, innerException)
   {
      
   }
   
}