using System.ServiceProcess;
using RavenMQ.Config;

namespace Raven.MQ.Server
{
	internal partial class RavenService : ServiceBase
	{
		private RavenMqServer server;

		public RavenService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			server = new RavenMqServer(new RavenConfiguration());
		}

		protected override void OnStop()
		{
			if (server != null)
				server.Dispose();
		}
	}
}
