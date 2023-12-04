using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Text.Json;
using TestWebApi.Hubs;
using TestWebApi.Models;
using TestWebApi.Services;

namespace TestWebApi.Controllers
{
	/// <summary>
	/// Api Controller used to get and post the configuration settings (lower threshold
	/// and upper threshold)
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	public class SettingsController : ControllerBase
	{
		private readonly ISerialPortService _serialPortService;
		private readonly IHubService _hubService;
		private readonly IFileService _fileService;
		private readonly IDataService _dataService;

		public SettingsController(ISerialPortService serialPortService, IHubService hubService,IFileService fileService, IDataService dataService)
		{
			_serialPortService = serialPortService;
			_hubService = hubService;
			_fileService = fileService;
			_dataService = dataService;
		}

		/// <summary>
		///		Action method used to retrieve the configuration settings stored in the JSON file.
		///		The method reads from the JSON file, deserializes the data into a Settings object.
		/// </summary>
		/// 
		/// <returns>returns an ActionResult object which wraps a Settings object</returns>
		[HttpGet]
		public ActionResult<Settings> Get()
		{
			Settings settings = new Settings();

			try
			{
				settings = _fileService.readSettings();
			}
			catch (FileNotFoundException ex)
			{
				Log.Error(ex, "Settings file not found");
				return NotFound("Settings file not found");
			}
			catch (JsonException ex)
			{
				Log.Error(ex, "Error parsing JSON settings");
				return StatusCode(500, "Error parsing settings");
			}
			catch (Exception ex)
			{
				Log.Error("An error occurred while reading settings from the file");
				return StatusCode(500, "Internal server error");
			}
			return settings;
		}


		/// <summary>
		///		action method used to post the configuration settings from the web app.
		///		these settings will be stored in a JSON file and also written to the serial port.
		/// </summary>
		/// 
		/// <param name="value">string containing the configuration settings</param>
		/// 
		/// <exception cref="InvalidOperationException">
		///		If the device is physically removed from the port while trying to write, InvalidOperationException
		///		will be raised. It is handled by sending an error message to the web-app via SignalR causing the web-app
		///		to redirect to home page. 
		///	</exception>
		///	
		///	<exception cref="NullReferenceException">
		///		If write is attempted without creating an instance of serial port then NullReferenceException is thrown.
		///		It is handled by sending an error message to the web-app via SignalR causing the web-app
		///		to redirect to home page. 
		/// </exception>
		/// 
		/// <exception cref="OperationCanceledException">
		///		Thrown when the operation is canceled, such as when the device being written to is removed.
		///		It is handled by sending an error message to the web-app via SignalR causing the web-app
		///		to redirect to home page. 
		/// </exception>
		[HttpPost]
		public void PostSettings([FromBody] string value)
		{
			string[] words = value.Split(' ');
			var settings = new Settings();
			settings.thresholdLow = Convert.ToDecimal(words[0]);
			settings.thresholdHigh = Convert.ToDecimal(words[1]);
			settings.dataAcquisitionRate = Convert.ToInt32(words[2]);

			_fileService.writeSettings(settings);
			
			try
			{
				_serialPortService.serialPort.Write($"SETTHR {settings.thresholdLow} {settings.thresholdHigh}" + "\r");
				Log.Information("SENT: "+$"SETTHR {settings.thresholdLow} {settings.thresholdHigh}");
			}
			catch (OperationCanceledException ex)
			{
				Log.Error("Write Attempted while port is closed - Operation Cancelled");
				_serialPortService.serialPort.DataReceived -= _dataService.dataReceived;

				_hubService.sendToHub("TransferReplyError", "operation cancelled");
			}
			catch (InvalidOperationException e)
			{
				Log.Error("Write Attempted while port is closed - Invalid Operation");
				_serialPortService.serialPort.DataReceived -= _dataService.dataReceived;

				_hubService.sendToHub("TransferReplyError", "Port is Closed");
			}
			catch(NullReferenceException e)
			{
				Log.Error("serial port instance not created - cannot write");
			}
		}
	}
}
