using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace PB.SpiralOfTest.Infrastructure.Headers
{
    [DataContract]
    public class Header<T>
        where T : class
    {
        public static T GetIncoming()
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return null;
            }
            return context.IncomingMessageHeaders.GetHeader<T>(typeof(T).Name, typeof(T).Namespace);
        }

        public static T GetOutgoing()
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return null;
            }
            return context.OutgoingMessageHeaders.GetHeader<T>(typeof(T).Name, typeof(T).Namespace);
        }

        public static void AddIncoming(T value)
        { }

        public static void AddOutgoing(T value)
        { }

        public static void ReplaceIncoming(T value)
        { }

        public static void ReplaceOutgoing(T value)
        { }

        public static void CopyHeaderTo(MessageHeaders destination)
        { }

        public static void CopyHeaderFrom(MessageHeaders source)
        { }

    }
}
