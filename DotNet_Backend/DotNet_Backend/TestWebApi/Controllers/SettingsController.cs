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

		public SettingsController(ISerialPortService serialPortService, IHubService hubService,IFileService fileService)
		{
			_serialPortService = serialPortService;
			_hubService = hubService;
			_fileService = fileService;
		}

		/// <summary>
		///		Action method used to retrieve the configuration settings stored in the JSON file.
		///		The method reads from the JSON file, deserializes the data into a Settings object.
		/// </summary>
		/// <returns>returns an ActionResult object which wraps a Settings object</returns>
		[HttpGet]
		public ActionResult<Settings> getSettings()
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
		/// <param name="value">string containing the configuration settings</param>
		[HttpPost]
		public void postSettings([FromBody] string value)
		{
			string[] words = value.Split(' ');
			var settings = new Settings();
			settings.thresholdLow = Convert.ToDecimal(words[0]);
			settings.thresholdHigh = Convert.ToDecimal(words[1]);
			settings.dataAcquisitionRate = Convert.ToInt32(words[2]);

			_fileService.writeSettings(settings);
			
			try
			{
				_serialPortService.writeToPort($"SETTHR {settings.thresholdLow} {settings.thresholdHigh}" + "\r");
			}
			catch (OperationCanceledException ex)
			{
				_serialPortService.closeSerialPort();
				Log.Error("Write Attempted while port is closed - Operation Cancelled");
				
				_hubService.sendToHub("TransferReplyError", "operation cancelled");
			}
			catch (InvalidOperationException e)
			{
				_serialPortService.closeSerialPort();
				Log.Error("Write Attempted while port is closed - Invalid Operation");

				_hubService.sendToHub("TransferReplyError", "Port is Closed");
			}
			catch(NullReferenceException e)
			{
				Log.Error("serial port instance not created - cannot write");
			}
		}
	}
}
