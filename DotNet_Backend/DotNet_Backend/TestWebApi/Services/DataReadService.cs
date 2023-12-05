using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System.IO.Ports;
using System.Text.Json;
using TestWebApi.Hubs;
using TestWebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestWebApi.Services
{
	/// <summary>
	/// Represents a service for handling data received from a serial port.
	/// </summary>
	public interface IDataService
	{
		event DataHandler DataReceived;
		void dataReceived(object sender, SerialDataReceivedEventArgs e);
	}

	public delegate Task DataHandler(string data);

	/// <summary>
	/// This class contains methods to handle serial data reception, process different types of messages,
	/// interact with a SignalR hub to send messages to connected clients, and store sensor data in a database.
	/// It also handles exceptions that may occur during data processing and responds to changes in the
	/// serial port's pin state, potentially indicating device removal.
	/// </summary>
	public class DataReadService : IDataService
	{
		
		private readonly IHubService _hubService;
		private readonly IDataHandlerService _dataHandlerService;
		
		public event DataHandler DataReceived;

		public DataReadService(IHubService hubService, IDataHandlerService dataHandlerService)
		{
			_hubService = hubService;
			_dataHandlerService = dataHandlerService;

			DataReceived += _dataHandlerService.onDataRecievedFromDevice;
		}

		/// <summary>
		/// Event handler for processing data received from a serial port.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		public async void dataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string message;
			SerialPort sp = (SerialPort)sender;
	
			try
			{
				message = sp.ReadTo("\r");

				DataReceived?.Invoke(message);
			}
				
			catch (OperationCanceledException ex)
			{
				Log.Error("operation was cancelled");
				await _hubService.sendToHub("TransferReplyError", "operation cancelled");
			}
			catch (InvalidOperationException ex)
			{
				Log.Error("operation was cancelled");
				await _hubService.sendToHub("TransferReplyError", "operation cancelled:port closed");
			}
			catch (NullReferenceException ex)
			{
				Log.Error("port is closed");
				await _hubService.sendToHub("TransferReplyError", "port closed");
			}
		}
	}
}
