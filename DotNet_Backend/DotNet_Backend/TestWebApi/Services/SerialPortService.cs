using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using System.Drawing;
using System.IO.Ports;
using TestWebApi.Hubs;
using TestWebApi.Models;

namespace TestWebApi.Services
{
	/// <summary>
	/// Represents a service for managing a serial port connection
	/// </summary>
	public interface ISerialPortService
	{
		IEnumerable<string> GetPortNames();
		IEnumerable<string> GetHandshakeOptions();
		IEnumerable<string> GetParityOptions();
		IEnumerable<string> GetStopBits();

		bool isOpen();
		void openSerialPort();
		void closeSerialPort();

		void setParity(string parity);
		void setHandshake(string handshake);
		void setPortname(string name);
		void setBaudrate(int baudRate);
		void setDatabits(int dataBits);
		void setStopbits(string stopBits);

		void writeToPort(string data);
	}

	/// <summary>
	/// This class is used for creating a singleton representing the serialPort connection
	/// </summary>
	public class SerialPortService : ISerialPortService
	{
		private readonly IDataService _dataReadService;

		private static SerialPort serialPort { get; set; } = new SerialPort();

		public SerialPortService(IDataService dataReadService)
		{
			_dataReadService = dataReadService;
		}
		
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

		public void openSerialPort()
		{
			serialPort.Open();

			serialPort.DataReceived += _dataReadService.dataReceived;
		}

		public void closeSerialPort()
		{
			serialPort.Close();
			serialPort.DataReceived -= _dataReadService.dataReceived;	
		}

		public void setPortname(string name)
		{
			serialPort.PortName = name;
		}

		public void setBaudrate(int baudRate)
		{
			serialPort.BaudRate=baudRate;
		}

		public void setDatabits(int dataBits)
		{
			serialPort.DataBits = dataBits;
		}

		public void setParity(string parity)
		{
			switch (parity)
			{
				case "Even":
					serialPort.Parity = System.IO.Ports.Parity.Even;
					break;
				case "Mark":
					serialPort.Parity = System.IO.Ports.Parity.Mark;
					break;
				case "None":
					serialPort.Parity = System.IO.Ports.Parity.None;
					break;
				case "Odd":
					serialPort.Parity = System.IO.Ports.Parity.Odd;
					break;
				case "Space":
					serialPort.Parity = System.IO.Ports.Parity.Space;
					break;
				default: break;
			}
		}

		public void setHandshake(string handshake)
		{
			switch(handshake)
			{
				case "None":
					serialPort.Handshake = System.IO.Ports.Handshake.None;
					break;
				case "RequestToSend":
					serialPort.Handshake = System.IO.Ports.Handshake.RequestToSend;
					break;
				case "RequestToSendXOnXOff":
					serialPort.Handshake = System.IO.Ports.Handshake.RequestToSendXOnXOff;
					break;
				case "XOnXOff":
					serialPort.Handshake = System.IO.Ports.Handshake.XOnXOff;
					break;
				default: break;
			}
		}

		public void setStopbits(string stopBits)
		{
			switch (stopBits)
			{
				case "None":
					serialPort.StopBits = System.IO.Ports.StopBits.None;
					break;
				case "One":
					serialPort.StopBits = System.IO.Ports.StopBits.One;
					break;
				case "OnePointFive":
					serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
					break;
				case "Two":
					serialPort.StopBits = System.IO.Ports.StopBits.Two;
					break;
				default: break;
			}
		}

		public bool isOpen()
		{
			return serialPort.IsOpen;
		}

		public void writeToPort(string data)
		{
			serialPort.Write(data);
		}
	}
}
