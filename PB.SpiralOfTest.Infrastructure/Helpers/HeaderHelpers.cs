using PB.SpiralOfTest.Infrastructure.Headers;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PB.SpiralOfTest.Infrastructure.Helpers
{
    public static class HeaderHelpers
    {
        internal static void AddCommonHeaders(MessageHeaders destination)
        {
            //Header<SessionContext>.Add(new SessionContext(), destination);  // TODO: Find out what to do here
            // Add more contexts here
        }

        internal static void CopyCommonHeadersTo(MessageHeaders destination)
        {
            if (OperationContext.Current == null)
                throw new InvalidOperationException("No current OperationContext");

            Header<SessionContext>.CopyHeaderTo(destination);
            // Add more contexts here
        }

        internal static void CopyCommonHeadersFrom(MessageHeaders destination)
        {
            if (OperationContext.Current == null)
                throw new InvalidOperationException("No current OperationContext");

            Header<SessionContext>.CopyHeaderFrom(destination);
            // Add more contexts here
        }

        internal static bool CanCopyHeaders()
        {
            throw new NotImplementedException();
        }
    }
}
