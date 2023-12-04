using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using System.IO.Ports;
using TestWebApi.DataStorage;
using TestWebApi.Hubs;
using TestWebApi.Models;

namespace TestWebApi.Services
{
	/// <summary>
	/// Represents a service for managing a serial port connection
	/// </summary>
		public interface ISerialPortService
    	{
        	SerialPort serialPort { get; set; }
			IEnumerable<string> GetPortNames();
			IEnumerable<string> GetHandshakeOptions();
			IEnumerable<string> GetParityOptions();
			IEnumerable<string> GetStopBits();
		}

    	/// <summary>
    	/// This class is used for creating a singleton representing the serialPort connection
    	/// </summary>
    	public class SerialPortService : ISerialPortService
    	{
			public SerialPortService()
			{

			}
			public SerialPort serialPort { get; set; } = new SerialPort();

			public IEnumerable<string> GetPortNames()
			{
				List<string> portNames = new List<string> { };
				foreach (string s in SerialPort.GetPortNames())
				{
					portNames.Add(s);
				}
				return portNames;
			}

			public IEnumerable<string> GetHandshakeOptions()
			{
				List<string> handshakes = new List<string> { };
				foreach (string s in Enum.GetNames(typeof(Handshake)))
				{
					handshakes.Add(s);
				}

				return handshakes;
			}

			public IEnumerable<string> GetParityOptions()
			{
				List<string> parity = new List<string> { };
				foreach (string s in Enum.GetNames(typeof(Parity)))
				{
					parity.Add(s);
				}
				return parity;
			}

			public IEnumerable<string> GetStopBits()
			{
				List<string> stopbits = new List<string> { };
				foreach (string s in Enum.GetNames(typeof(StopBits)))
				{
					stopbits.Add(s);
				}

				return stopbits;
			}
		}
}
