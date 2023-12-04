using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.IO.Ports;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using TestWebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using TestWebApi.Services;
using Humanizer;
using Serilog;

namespace TestWebApi.Controllers
{
	/// <summary>
	///		API Controller used to transfer UART connection parameters between frontend and backend.
	///		Used to connect and disconnect from the serial port.
	/// </summary>
	[Route("[controller]")]
	[ApiController]
	public class ConnectionParametersController : ControllerBase
	{
		private readonly ISerialPortService _serialPortService;
		private readonly IDataService _dataService;
		public ConnectionParametersController(ISerialPortService serialPortService, IDataService dataService)
		{
			_serialPortService = serialPortService;
			_dataService = dataService;
		}

		/// <summary>
		///		action method used to retrive names of the currently active ports
		/// </summary>
		/// 
		/// <returns>returns an enumerable collection of port names</returns>
		[HttpGet("portnames")]
		public async Task<IEnumerable<string>> GetPortNames()
		{
			List<string> portNames = new List<string> { };
			foreach (string s in SerialPort.GetPortNames())
			{
				portNames.Add(s);
			}

			return portNames;
		}

		/// <summary>
		///		action method used to retrieve the possible handshake options
		/// </summary>
		/// 
		/// <returns>returns an enumerable collection of handshake options</returns>
		[HttpGet("handshakes")]
		public async Task<IEnumerable<string>> GetHandshakeOptions()
		{
			List<string> handshakes = new List<string> { };
			foreach (string s in Enum.GetNames(typeof(Handshake)))
			{
				handshakes.Add(s);
			}

			return handshakes;
		}

		/// <summary>
		///		action method used to retrieve the possible parity options
		/// </summary>
		/// 
		/// <returns>returns an enumerable collection of parity options</returns>
		[HttpGet("parity")]
		public async Task<IEnumerable<string>> GetParityOptions()
		{
			List<string> parity = new List<string> { };
			foreach (string s in Enum.GetNames(typeof(Parity)))
			{
				parity.Add(s);
			}


			return parity;
		}

		/// <summary>
		///		action method used to retrieve possible stop bits 
		/// </summary>
		/// 
		/// <returns>returns an enumerable collection of stopbits (as string)</returns>
		[HttpGet("stopbit")]
		public async Task<IEnumerable<string>> GetStopBits()
		{
			List<string> stopbits = new List<string> { };
			foreach (string s in Enum.GetNames(typeof(StopBits)))
			{
				stopbits.Add(s);
			}

			return stopbits;
		}

		/// <summary>
		///		action method used to retrieve random sensor data for testing the graph
		/// </summary>
		/// 
		/// <returns>returns an object representing the sensor data</returns>
		[HttpGet("data")]
		public async Task<object> GetSensorData()
		{
			SensorData data = new SensorData();
			data.timestamp = DateTime.Now;
			data.current = new Random().Next(5, 25);
			return new {dt=data.timestamp.ToString("h:mm:ss tt"),current=data.current};
		}

		/// <summary>
		///		action method used for posting the connection parameters from the frontend.
		///		method receives connection parameters such as Portname, Baudrate etc. from the frontend
		///		and opens a serial port with the given parameters.
		/// </summary>
		/// 
		/// <param name="port">Contains the connection parameters for connecting to the serial port</param>
		/// 
		/// <returns>returns an ActionResult object</returns>
		/// 
		/// <Exceptions cref="IOException">
		///		Trying to open the port when it is already open will throw an exception
		///	</Exceptions>
		[HttpPost]
		public async Task<ActionResult<Port>> PostPort(Port port)
		{
			if (_serialPortService.serialPort.IsOpen)
			{
				Log.Error("port already open");
				_serialPortService.serialPort.Close();
				port.PortName = "error";
				return CreatedAtAction("PostPort", port);
			}

			_serialPortService.serialPort.DataReceived += _dataService.dataReceived;
			_serialPortService.serialPort.PinChanged += _dataService.port_PinChanged;

			_serialPortService.serialPort.PortName = port.PortName;
			_serialPortService.serialPort.BaudRate = port.BaudRate;
			_serialPortService.serialPort.DataBits = port.DataBits;

			switch (port.Parity)
			{
				case "Even":
					_serialPortService.serialPort.Parity = System.IO.Ports.Parity.Even;
					break;
				case "Mark":
					_serialPortService.serialPort.Parity = System.IO.Ports.Parity.Mark;
					break;
				case "None":
					_serialPortService.serialPort.Parity = System.IO.Ports.Parity.None;
					break;
				case "Odd":
					_serialPortService.serialPort.Parity = System.IO.Ports.Parity.Odd;
					break;
				case "Space":
					_serialPortService.serialPort.Parity = System.IO.Ports.Parity.Space;
					break;
				default: break;
			}

			switch (port.Handshake)
			{
				case "None":
					_serialPortService.serialPort.Handshake = System.IO.Ports.Handshake.None;
					break;
				case "RequestToSend":
					_serialPortService.serialPort.Handshake = System.IO.Ports.Handshake.RequestToSend;
					break;
				case "RequestToSendXOnXOff":
					_serialPortService.serialPort.Handshake = System.IO.Ports.Handshake.RequestToSendXOnXOff;
					break;
				case "XOnXOff":
					_serialPortService.serialPort.Handshake = System.IO.Ports.Handshake.XOnXOff;
					break;
				default: break;
			}

			switch (port.StopBits)
			{
				case "None":
					_serialPortService.serialPort.StopBits = System.IO.Ports.StopBits.None;
					break;
				case "One":
					_serialPortService.serialPort.StopBits = System.IO.Ports.StopBits.One;
					break;
				case "OnePointFive":
					_serialPortService.serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
					break;
				case "Two":
					_serialPortService.serialPort.StopBits = System.IO.Ports.StopBits.Two;
					break;
				default: break;
			}

			_serialPortService.serialPort.WriteTimeout = 1000;

			try
			{
				_serialPortService.serialPort.Open();
			}
			catch(Exception ex)
			{
				Log.Error("error in connecting to serial port");
				port.PortName = "error";
				_serialPortService.serialPort.Close();
				return CreatedAtAction("PostPort", port);
			}

			Log.Information("Connected to serial port");
			Log.Information("SENT: GETTHR");

			_serialPortService.serialPort.Write("GETTHR\r");

			return CreatedAtAction("PostPort", port);

		}

		/// <summary>
		///		action method used to close the serial port connection when user clicks disconnect button in the UI.
		///		also unsubscribes from the serial port's dataReceived event handler.
		/// </summary>
		/// 
		/// <param name="port">represents the port to be closed</param>
		/// 
		/// <returns>returns an ActionResult object</returns>
		/// 
		/// <Exception cref="IOException">
		///		Trying to close the port when the port is in an invalid state or if an
		///		attempt to set the state of the underlying port failed then an IOException is thrown.
		/// </Exception>
		[HttpPost("disconnect")]
		public ActionResult disconnect(Port port)
		{
			try
			{
				_serialPortService.serialPort.Close();
				_serialPortService.serialPort.DataReceived -= _dataService.dataReceived;
			}
			catch(Exception ex)
			{
				Log.Error("Could not close port");
				return Ok();
			}
			Log.Information("Disconnected from serial port");
			return Ok();
		}
	}
}
