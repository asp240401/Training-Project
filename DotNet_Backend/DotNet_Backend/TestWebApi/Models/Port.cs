using System.IO.Ports;
using System.Text.Json.Serialization;

namespace TestWebApi.Models
{
	/// <summary>
	/// This class is used to transfer the connection paremeters for Serial Port connection 
	/// from the frontend to backend.
	/// </summary>
	public class Port
	{
		[JsonPropertyName("PortName")]
		public string PortName { get; set; }
		public int BaudRate { get; set; }
		public int DataBits { get; set; }
		public string Handshake { get; set; }
		public string StopBits { get; set; }
		public string Parity { get; set; }
	}
}
