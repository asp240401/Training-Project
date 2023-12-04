using TestWebApi.Models;

namespace TestWebApi.DataStorage
{
	/// <summary>
	/// class used for testing out the graph without the device. (simulate random sensor data)
	/// </summary>
	public class DataManager
	{
		public static int xval = 0;

		/// <summary>
		/// generates random data to simulate sensor values
		/// </summary>
		/// <returns>an object representing sensor data</returns>
		public static object GetData()
		{
			SensorData data = new SensorData();
			data.timestamp = DateTime.Now;
			data.current = new Random().Next(5, 25);
			return new { label = data.timestamp.ToString("h:mm:ss tt"), y = data.current};
		}
	}
}
