using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.IO.Ports;
using System.Net;
using TestWebApi.Hubs;
using TestWebApi.Services;


namespace TestWebApi.Controllers
{
	/// <summary>
	/// Api Controller used to handle manual mode commands coming from the web app.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	public class ManualModeController : ControllerBase
	{
		private readonly ISerialPortService _serialPortService;
		private readonly IHubContext<DataHub> _hub;
		private readonly IDataService _dataService;

		public ManualModeController(ISerialPortService serialPortService, IHubContext<DataHub> hub,IDataService dataService)
		{
			_serialPortService = serialPortService;
			_hub = hub;
			_dataService = dataService;
		}

		/// <summary>
		/// action method used for posting the manual mode commands from the web app.
		/// the method will write these commands to the serialport which will be received by
		/// the hardware.
		/// </summary>
		/// <param name="value">string representing the manual mode command</param>
		/// <returns>returns an ActionResult object</returns>
		///	<exception cref="InvalidOperationException">
		///	If the device is physically removed from the port while trying to write, InvalidOperationException
		///	will be raised. It is handled by sending an error message to the web-app via SignalR causing the web-app
		///	to redirect to home page.
		///	</exception>
		[HttpPost]
		public IActionResult Post([FromBody] string value)
		{
			try
			{
				_serialPortService.serialPort.Write(value + "\r");
				Log.Information("SENT:" + value);
			}
			catch (OperationCanceledException ex)
			{
				Log.Error("operation was cancelled");
				_serialPortService.serialPort.DataReceived -= _dataService.dataReceived;
				_hub.Clients.All.SendAsync("TransferReplyError", "operation cancelled");
			}
			catch (InvalidOperationException ex)
			{
				Log.Error("port is closed");
				_serialPortService.serialPort.DataReceived -= _dataService.dataReceived;
				_hub.Clients.All.SendAsync("TransferReplyError", "Port is Closed");
			}

			return Ok();
		}
	}
}
