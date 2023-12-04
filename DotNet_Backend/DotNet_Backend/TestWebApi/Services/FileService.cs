using Serilog;
using System.IO.Ports;
using System.Text.Json;
using TestWebApi.Models;

namespace TestWebApi.Services
{
	public interface IFileService
	{
		void writeSettings(Settings settings);

		Settings readSettings();
	}

	/// <summary>
	/// Implementation of the file service for reading/writing settings to/from the JSON file.
	/// </summary>
	public class FileService: IFileService
	{
		/// <summary>
		/// Writes the settings to a file.
		/// </summary>
		/// <param name="settings">The settings object to be written.</param>
		public void writeSettings(Settings settings)
		{
			string fileName = "configSettings.json";
			StreamWriter sw;
			string jsonString;
			try
			{
				sw = new StreamWriter(fileName);
				jsonString = JsonSerializer.Serialize(settings);
				sw.Write(jsonString);
				sw.Flush();
				sw.Close();
			}
			catch (Exception ex)
			{
				Log.Error("An error occurred while writing settings to the file");
			}
		}

		/// <summary>
		/// Reads the settings from a file.
		/// </summary>
		/// <returns>The settings object read from the file.</returns>
		public Settings readSettings()
		{
			Settings settings=new Settings();
			settings.dataAcquisitionRate = 0;
			StreamReader sr;
			string json;

			try
			{
				sr = new StreamReader("configSettings.json");
				json = sr.ReadToEnd();
				settings = JsonSerializer.Deserialize<Settings>(json);
				sr.Close();
			}
			catch(Exception ex)
			{
				Log.Error("An error occurred while reading settings from the file");
				return settings;
			}
			return settings;
		}
	}
}
