using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using System.IO.Ports;
using System.Text.Json;
using TestWebApi.Hubs;
using TestWebApi.Models;

namespace TestWebApi.Services
{
	/// <summary>
	/// Represents a service for handling data received from a serial port.
	/// </summary>
	public interface IDataService
	{
		void dataReceived(object sender, SerialDataReceivedEventArgs e);
	}

	/// <summary>
	/// Manages the handling and processing of sensor data received from a serial port,
	/// providing methods to write data to a SQL Server database and manage port events.
	/// </summary>
	/// <remarks>
	/// This class contains methods to handle serial data reception, process different types of messages,
	/// interact with a SignalR hub to send messages to connected clients, and store sensor data in a database.
	/// It also handles exceptions that may occur during data processing and responds to changes in the
	/// serial port's pin state, potentially indicating device removal.
	/// </remarks>
	public class DataService : IDataService
	{
		private readonly IFileService _fileService;

		private readonly IHubService _hubService;

		public DataService(IFileService fileService, IHubService hubService)
		{
			_fileService = fileService;
			_hubService = hubService;
		}

		/// <summary>
		/// Method to write received sensor data to a SQL Server database
		/// </summary>
		/// <param name="data">an object representing the data received</param>
		/// <returns>Returns a Task representing the asynchronous operation</returns>
		public async Task writeToDatabase(SensorData data)
		{
			var options = new DbContextOptionsBuilder<SensorContext>().UseSqlServer("Data Source=TJ16AA050-PC\\SQLEXPRESS;Initial Catalog=Current_Sensor;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True;").Options;

			try
			{
				using (var dbContext = new SensorContext(options))
				{
					dbContext.SensorData.Add(data);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (DbUpdateException ex)
			{
				Log.Error("DbUpdateException occurred: " + ex.Message);
			}
			catch (Exception e)
			{
				Log.Error("An unexpected error occurred: " + e.Message);
			}
		}

		/// <summary>
		/// Method is used to handle the adc data received from the device.
		/// Writes the data to the DB and also sends it to the frontend via SignalR
		/// </summary>
		/// <param name="message">data received from the device</param>
		/// <returns></returns>
		public async Task handleAdcData(string message)
		{
			string[] words = message.Split(' ');
			if (words.Length == 3)
			{
				await _hubService.sendToHub("TransferAdcData", message);
			}
			else
			{
				decimal val = Convert.ToDecimal(words[1]);
				Console.WriteLine("value:" + val);

				SensorData data = new SensorData();
				data.timestamp = DateTime.Now;
				data.current = val;

				await _hubService.sendToHub("TransferReplyData", new { label = data.timestamp.ToString("h:mm:ss tt"), y = data.current });

				await writeToDatabase(data);
			}
		}

		/// <summary>
		/// method used to handle threshold values received from the device.
		/// saves threshold values to JSON file.
		/// </summary>
		/// <param name="message">data received from the device</param>
		public void handleThresholdData(string message)
		{
			string[] words = message.Split(' ');
			if (words[1] == "SET") //THR SET reply from device
			{

			}
			else
			{
				decimal lt = Convert.ToDecimal(words[1]);
				decimal ht = Convert.ToDecimal(words[2]);
				var settings = new Settings();
				var existingSettings = _fileService.readSettings();

				if (existingSettings.dataAcquisitionRate == 0)
				{
					settings.dataAcquisitionRate = 1000;
				}
				else
				{
					settings.dataAcquisitionRate = existingSettings.dataAcquisitionRate;
				}

				settings.thresholdLow = lt;
				settings.thresholdHigh = ht;

				_fileService.writeSettings(settings);
			}
		}
		
		/// <summary>
		/// Event handler for processing data received from a serial port.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">The event arguments.</param>
		/// <exception cref="OperationCanceledException">
		/// Thrown when the operation is canceled, such as when the device being read from is removed.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when an invalid operation occurs, such as attempting to read from a closed port.
		/// </exception>
		/// <exception cref="NullReferenceException">
		/// Thrown when a null reference is encountered, such as writitng to a serial port which is not instantiated.
		/// </exception>
		public async void dataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string message;
			SerialPort sp = (SerialPort)sender;
	
			try
			{
				message = sp.ReadTo("\r");
	
				if (message.StartsWith("SETTING"))
					await _hubService.sendToHub("TransferReplyLed", message);
	
				else if (message.StartsWith("BUTTON"))
					await _hubService.sendToHub("TransferReplyButton", message);
	
				else if (message.StartsWith("EEPROM"))
					await _hubService.sendToHub("TransferReplyErm", message);
	
				else if (message.StartsWith("ADC"))
				{
					await handleAdcData(message);
				}
				else if (message.StartsWith("THR"))
				{
					handleThresholdData(message);
				}
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
