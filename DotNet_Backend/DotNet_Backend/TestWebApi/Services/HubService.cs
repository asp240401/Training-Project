using Microsoft.AspNetCore.SignalR;
using TestWebApi.Hubs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestWebApi.Services
{
	public interface IHubService
	{
		Task sendToHub(string eventName,object arg);
	}

	public class HubService:IHubService
	{
		private readonly IHubContext<DataHub> _hub;

		public HubService(IHubContext<DataHub> hub)
		{
			_hub = hub;
		}

		public async Task sendToHub(string eventName,object arg)
		{
			await _hub.Clients.All.SendAsync(eventName,arg);
		}
	}
}
