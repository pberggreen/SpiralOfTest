using PB.SpiralOfTest.Infrastructure.Headers;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PB.SpiralOfTest.Infrastructure.Helpers
{
    public static class HeaderHelpers
    {
        /// <summary>
        /// Add all common headers 
        /// </summary>
        /// <param name="destination"></param>
        internal static void AddCommonHeaders(MessageHeaders destination)
        {
            Header<SessionContext>.Add(new SessionContext(), destination);
            Header<TraceContext>.Add(new TraceContext(), destination);
            // Add more contexts here
        }

        internal static void CopyCommonHeadersTo(MessageHeaders destination)
        {
            if (OperationContext.Current == null)
                throw new InvalidOperationException("No current OperationContext");

            Header<SessionContext>.CopyHeaderTo(destination);
            Header<TraceContext>.CopyHeaderTo(destination);
            // Add more contexts here
        }

        internal static void CopyCommonHeadersFrom(MessageHeaders destination)
        {
            if (OperationContext.Current == null)
                throw new InvalidOperationException("No current OperationContext");

            Header<SessionContext>.CopyHeaderFrom(destination);
            Header<TraceContext>.CopyHeaderFrom(destination);
            // Add more contexts here
        }

        internal static bool CanCopyHeaders()
        {
            return OperationContext.Current != null;
        }
    }
}
