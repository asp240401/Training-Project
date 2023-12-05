using System.ComponentModel.DataAnnotations;

namespace SensorDataRepository
{
	/// <summary>
	/// This classs represents the sensor data. Instances of this class will be written to the database and
	/// also sent to the front-end whenever data is received from the hardware.
	/// </summary>
	public class SensorData
	{
		[Key]
		public int ID{ get; set; }
		public DateTime timestamp { get; set; }
		public decimal current {  get; set; }
	}
}
