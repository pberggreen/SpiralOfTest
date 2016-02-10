﻿using Microsoft.ServiceBus.Messaging;
using System;
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
            static NetTcpBinding _intranetBinding;

            public static NetTcpBinding Binding(long maxReceivedMessageSize, TimeSpan timeout)
            {
                if (_intranetBinding == null)
                {
                    try
                    {
                        // Try to create binding from the config file
                        _intranetBinding = new NetTcpBinding("Intranet");
                    }
                    catch
                    {
                        _intranetBinding = DefaultBinding();
                    }
                }

                EnforceBindingPolicies(_intranetBinding, maxReceivedMessageSize, timeout);

                return _intranetBinding;
            }

            private static void EnforceBindingPolicies(NetTcpBinding binding, long maxReceivedMessageSize, TimeSpan timeout)
            {
                binding.MaxReceivedMessageSize = maxReceivedMessageSize;
                binding.ReceiveTimeout = timeout;
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
                return new NetTcpBinding
                {
                    TransactionFlow = true,
                    ReliableSession = { Enabled = true }
                };
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
