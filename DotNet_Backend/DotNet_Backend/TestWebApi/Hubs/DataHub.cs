using Microsoft.AspNetCore.SignalR;

namespace TestWebApi.Hubs
{
	///<summary>
	/// represents a SignalR hub which allows server code to send asynchronous notifications to the client-side web application
	/// this hub will be used for the following:
	///		1. sending sensor data to the web app whenever it is received in the serial port
	///		2. sending error messages to the web app if the device is removed or some unexpected error occurs
	///</summary>
	public class DataHub:Hub
	{
		
	}
}
