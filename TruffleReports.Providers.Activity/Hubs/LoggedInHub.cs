using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;
using TruffleReports.Providers.Activity.Providers;

namespace TruffleReports.Providers.Activity.Hubs
{
    /// <summary>
    /// A <see cref="Hub"/> that will rely to registered clients report updates for their host.
    /// </summary>
    public class LoggedInHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> registrations = new ConcurrentDictionary<string, List<string>>();
        private static readonly object lockObject = new object();
        private static bool hasSubscribed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInHub"/> class.
        /// </summary>
        public LoggedInHub()
        {
            if (hasSubscribed == false)
            {
                lock (lockObject)
                {
                    LoggedInProvider.Reports.Subscribe(Echo);
                    hasSubscribed = true;
                }
            }
        }

        /// <summary>
        /// Registers the Client to receive updates about the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        public void Register(string host)
        {
            registrations.AddOrUpdate(
                host,
                a => new List<string> { Context.ConnectionId },
                (key, value) =>
                {
                    value.Add(Context.ConnectionId);
                    return value;
                });
        }

        /// <summary>
        /// Echoes the specified report to clients registered for that host.
        /// </summary>
        /// <param name="report">The report.</param>
        private void Echo(LoggedInReport report)
        {
            if (registrations.ContainsKey(report.Host))
            {
                foreach (var connectionId in registrations[report.Host])
                {
                    Clients.Client(connectionId).update(report);
                }
            }
        }
    }

    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly _assembly;
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            _assembly = typeof(Connection).Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.Equals(_assembly))
            {
                return _defaultContractSerializer.ResolveContract(type);
            }

            return _camelCaseContractResolver.ResolveContract(type);
        }
    }
}
