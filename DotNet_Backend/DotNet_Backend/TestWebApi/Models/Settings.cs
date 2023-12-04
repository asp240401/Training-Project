using System.Text.Json.Serialization;

namespace TestWebApi.Models
{
	/// <summary>
	/// Class used to handle the configuration settings. 
	/// Settings will be sent and received as an object of this class.
	/// </summary>
	public class Settings
	{
		public decimal thresholdLow { get; set; }
		public decimal thresholdHigh { get; set; }
		public int dataAcquisitionRate {  get; set; }
	}
}
