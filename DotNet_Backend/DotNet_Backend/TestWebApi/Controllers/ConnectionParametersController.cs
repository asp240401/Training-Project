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

		public ConnectionParametersController(ISerialPortService serialPortService)
		{
			_serialPortService = serialPortService;
		}

		/// <summary>
		///		action method used to retrive names of the currently active ports
		/// </summary>
		/// <returns>returns an enumerable collection of port names</returns>
		[HttpGet("portnames")]
		public async Task<IEnumerable<string>> getPortNames()
		{
			return _serialPortService.GetPortNames();
		}

		/// <summary>
		///		action method used to retrieve the possible handshake options
		/// </summary>
		/// <returns>returns an enumerable collection of handshake options</returns>
		[HttpGet("handshakes")]
		public async Task<IEnumerable<string>> getHandshakeOptions()
		{

			return _serialPortService.GetHandshakeOptions();
		}

		/// <summary>
		///		action method used to retrieve the possible parity options
		/// </summary>
		/// <returns>returns an enumerable collection of parity options</returns>
		[HttpGet("parity")]
		public async Task<IEnumerable<string>> getParityOptions()
		{ 
			return _serialPortService.GetParityOptions();
		}

		/// <summary>
		///		action method used to retrieve possible stop bits 
		/// </summary>
		/// <returns>returns an enumerable collection of stopbits (as string)</returns>
		[HttpGet("stopbit")]
		public async Task<IEnumerable<string>> getStopBits()
		{
			return _serialPortService.GetStopBits();
		}

		/// <summary>
		///		action method used for posting the connection parameters from the frontend.
		///		method receives connection parameters such as Portname, Baudrate etc. from the frontend
		///		and opens a serial port with the given parameters.
		/// </summary>
		/// <param name="port">Contains the connection parameters for connecting to the serial port</param>
		/// <returns>returns an ActionResult object</returns>
		[HttpPost]
		public async Task<ActionResult<Port>> postPort(Port port)
		{
			if (_serialPortService.isOpen())
			{
				Log.Error("port already open");
				_serialPortService.closeSerialPort();
				port.PortName = "error";
				return CreatedAtAction("PostPort", port);
			}

			_serialPortService.setPortname(port.PortName);
			_serialPortService.setBaudrate(port.BaudRate);
			_serialPortService.setDatabits(port.DataBits);
			_serialPortService.setParity(port.Parity);
			_serialPortService.setHandshake(port.Handshake);
			_serialPortService.setStopbits(port.StopBits);

			try
			{
				_serialPortService.openSerialPort();
			}
			catch(Exception ex)
			{
				Log.Error("error in connecting to serial port");
				port.PortName = "error";
				_serialPortService.closeSerialPort();
				return CreatedAtAction("PostPort", port);
			}

			_serialPortService.writeToPort("GETTHR\r");

			return CreatedAtAction("PostPort", port);

		}

		/// <summary>
		///		action method used to close the serial port connection when user clicks disconnect button in the UI.
		///		also unsubscribes from the serial port's dataReceived event handler.
		/// </summary>
		/// <param name="port">represents the port to be closed</param>
		/// <returns>returns an ActionResult object</returns>
		[HttpPost("disconnect")]
		public ActionResult disconnect(Port port)
		{
			try
			{
				_serialPortService.closeSerialPort();
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
