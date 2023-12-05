using Microsoft.EntityFrameworkCore;
using Serilog;
using TestWebApi.Models;

namespace TestWebApi.Services
{
	public interface IDataHandlerService
	{
		Task onDataRecievedFromDevice(string message);
	}

	public class DataHandlerService: IDataHandlerService
	{
		private readonly IHubService _hubService;
		private readonly IFileService _fileService;

		public DataHandlerService(IHubService hubService, IFileService fileService)
		{
			_hubService = hubService;
			_fileService = fileService;
		}

		/// <summary>
		/// Method to write received sensor data to a SQL Server database
		/// </summary>
		/// <param name="data">an object representing the data received</param>
		/// <returns>Returns a Task representing the asynchronous operation</returns>
		public async Task writeToDatabase(SensorData data)
		{
			var options = new DbContextOptionsBuilder<SensorContext>().UseSqlServer("Data Source=TJ14AA136-PC\\SQLEXPRESS;Initial Catalog=CurrentSensor;Integrated Security=true;MultipleActiveResultSets=true;TrustServerCertificate=True;").Options;

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
			//catch (Exception e)
			//{
			//	Log.Error("An unexpected error occurred: " + e.Message);
			//}
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

			if (words[1] != "SET") //if not THR SET reply from device
			{
				decimal lt = Convert.ToDecimal(words[1]);
				decimal ht = Convert.ToDecimal(words[2]);

				var existingSettings = _fileService.readSettings();

				var settings = new Settings();
				settings.dataAcquisitionRate = existingSettings.dataAcquisitionRate == 0 ? 1000 : existingSettings.dataAcquisitionRate;
				settings.thresholdLow = lt;
				settings.thresholdHigh = ht;

				_fileService.writeSettings(settings);
			}
		}

		public async Task onDataRecievedFromDevice(string message)
		{
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
	}
}
