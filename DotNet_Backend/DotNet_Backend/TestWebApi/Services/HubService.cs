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

		/// <summary>
		/// method used to send data to the signalR hub
		/// </summary>
		/// <param name="eventName">name of the event which will be raised in the hub</param>
		/// <param name="data">the data to be sent to the hub</param>
		/// <returns></returns>
		public async Task sendToHub(string eventName,object data)
		{
			await _hub.Clients.All.SendAsync(eventName,data);
		}
	}
}
