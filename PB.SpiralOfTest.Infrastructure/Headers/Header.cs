using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System;

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

        public static void Add(T value, MessageHeaders destination)
        {
            var messageHeader = MessageHeader.CreateHeader(typeof(T).Name, typeof(T).Namespace, value);
            destination.Add(messageHeader);
        }

        public static void AddIncoming(T value)
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return;
            }
            Add(value, context.IncomingMessageHeaders);
        }

        public static void AddOutgoing(T value)
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return;
            }
            Add(value, context.OutgoingMessageHeaders);
        }

        public static void ReplaceIncoming(T value)
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return;
            }
            var index = context.IncomingMessageHeaders.FindHeader(typeof(T).Name, typeof(T).Namespace);
            if (index > -1)
                context.IncomingMessageHeaders.RemoveAt(index);
            Add(value, context.IncomingMessageHeaders);
        }

        public static void ReplaceOutgoing(T value)
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return;
            }
            var index = context.OutgoingMessageHeaders.FindHeader(typeof(T).Name, typeof(T).Namespace);
            if (index > -1)
                context.OutgoingMessageHeaders.RemoveAt(index);
            Add(value, context.OutgoingMessageHeaders);
        }

        public static void CopyHeaderTo(MessageHeaders destination)
        {
            throw new NotImplementedException();
        }

        public static void CopyHeaderFrom(MessageHeaders source)
        {
            throw new NotImplementedException();
        }

    }
}
