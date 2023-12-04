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
	}
}
