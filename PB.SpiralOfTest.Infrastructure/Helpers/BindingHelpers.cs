using Microsoft.ServiceBus.Messaging;
using System;
using System.Diagnostics;
using System.Net.Security;
using System.ServiceModel;

namespace PB.SpiralOfTest.Infrastructure.Helpers
{
    /// <summary>
    /// Helper functions for wcf hosts and proxies
    /// </summary>
    public static class BindingHelpers
    {
        public static Uri CreateAddress(Uri baseAddress, string endpointName)
        {
            var uriBuilder = new UriBuilder(baseAddress);
            uriBuilder.Path += endpointName;
            return uriBuilder.Uri;
        }

        public class Intranet
        {
            public static NetTcpBinding Binding(long maxReceivedMessageSize, TimeSpan timeout, TimeSpan debugTimeout)
            {
                NetTcpBinding intranetBinding;
                try
                {
                    // Try to create binding from the config file
                    intranetBinding = new NetTcpBinding("Intranet");
                }
                catch
                {
                    intranetBinding = DefaultBinding();
                }

                EnforceBindingPolicies(intranetBinding, maxReceivedMessageSize, timeout, debugTimeout);

                return intranetBinding;
            }

            private static void EnforceBindingPolicies(NetTcpBinding binding, long maxReceivedMessageSize, TimeSpan timeout, TimeSpan debugTimeout)
            {
                binding.MaxReceivedMessageSize = maxReceivedMessageSize;
                binding.ReceiveTimeout = timeout;
                if (Debugger.IsAttached)
                {
                    binding.ReceiveTimeout = debugTimeout;
                }
            }

            public static Uri CreateAddress(string hostName)
            {
                // Remove schema if present
                var index = hostName.IndexOf("//");
                if (index != -1)
                {
                    hostName = hostName.Substring(index + 2);
                }
                hostName = "net.tcp://" + hostName;
                var uriBuilder = new UriBuilder(hostName);
                return uriBuilder.Uri;
            }

            private static NetTcpBinding DefaultBinding()
            {
                //This works on Azure: 
                var netTcpBinding = new NetTcpBinding
                {
                    Security = new NetTcpSecurity { Mode = SecurityMode.None }
                };
                return netTcpBinding;

                //// With certificate
                //return new NetTcpBinding
                //{
                //    TransactionFlow = true,
                //    ReliableSession = { Enabled = true },
                //    Security = new NetTcpSecurity
                //    {
                //        Mode = SecurityMode.Transport,
                //        Transport = new TcpTransportSecurity
                //        {
                //            ClientCredentialType = TcpClientCredentialType.Certificate,
                //            ProtectionLevel = ProtectionLevel.EncryptAndSign
                //        }
                //    },
                //};

            }

        }

        public class ServiceBus
        {
            // TODO: Same "policy enforcing" logic as Intranet
            public static NetMessagingBinding Binding(long maxReceivedMessageSize, TimeSpan timeout)
            {
                return new NetMessagingBinding
                {
                    OpenTimeout = timeout,
                    CloseTimeout = timeout,
                    SendTimeout = timeout,
                    ReceiveTimeout = timeout,
                    TransportSettings = { BatchFlushInterval = TimeSpan.FromSeconds(1) }
                };
            }

        }

    }
}
