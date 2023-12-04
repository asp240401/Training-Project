using Microsoft.EntityFrameworkCore;
using Serilog;
using System.IO.Ports;
using TestWebApi.Hubs;
using TestWebApi.Models;
using TestWebApi.Services;
using TestWebApi.TimerFeatures;

namespace TestWebApi
{
	public class Program
	{
		/// <summary>
		///		Entry point of the application responsible for setting up and configuring various services,
		///		endpoints, and middleware for the ASP.NET application.
		/// </summary>
		/// 
		/// <param name="args">Command-line arguments passed to the application.</param>
		public static void Main(string[] args)
		{
            var builder = WebApplication.CreateBuilder(args);
			// Services configuration
			// - Adds controller services
			// - Configures and adds SensorContext as a Singleton service for database interaction
			// - Adds Singleton instances for TimerManager, IDataService, and ISerialPortService
			// - Configures SignalR services for real-time communication
			// - Configures Swagger/OpenAPI services for API documentation
			// - Configures Serilog for logging

			Log.Logger = new LoggerConfiguration()
			.WriteTo.Console() 
			.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
			.CreateLogger();

			builder.Services.AddControllers();

			builder.Services.AddDbContext<SensorContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"));
			}, ServiceLifetime.Singleton);
			
			builder.Services.AddSingleton<TimerManager>();
			
			builder.Services.AddScoped<IDataService, DataService>();
			builder.Services.AddScoped<ISerialPortService,SerialPortService>();
			builder.Services.AddSingleton<IFileService, FileService>();
			builder.Services.AddSingleton<IHubService, HubService>();

			builder.Services.AddSignalR();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();
			// Pipeline configuration
			// - Enables Swagger/OpenAPI UI in the development environment
			// - Configures HTTPS redirection, authorization, CORS policies allowing any header, method, and origin
			// - Maps endpoints for controllers and DataHub for SignalR communication

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseCors(o =>
			{
				o.AllowAnyHeader();
				o.AllowAnyMethod();
				o.AllowAnyOrigin();
			});

			app.MapControllers();

			app.MapHub<DataHub>("/data");

			app.Run();
		}
	}
}