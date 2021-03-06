using System;
using System.Net;
using Raven.Http;
using RavenMQ.Config;
using RavenMQ.Impl;
using RavenMQ.Network;
using RavenMQ.Server;
using RavenMQ.Subscriptions;

namespace Raven.MQ.Server
{
	public class RavenMqServer : IDisposable
	{
		private readonly Queues queues;
		private readonly HttpServer server;
	    private readonly ServerConnection serverConnection;

        public Queues Queues
		{
			get { return queues; }
		}

		public HttpServer Server
		{
			get { return server; }
		}

		public RavenMqServer(InMemoryRavenConfiguration settings)
		{
			settings.LoadLoggingSettings();
			queues = new Queues(settings);

			try
			{
				server = new QueuesHttpServer(settings, queues);
				server.Start();

				serverConnection = new ServerConnection(settings.SubscriptionPort,new QueuesSubscriptionIntegration(queues));
                serverConnection.Start();
			}
			catch (Exception)
			{
				queues.Dispose();
				queues = null;
				
				throw;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
            serverConnection.Dispose();
			server.Dispose();
			queues.Dispose();
		}

		#endregion

	}
}
