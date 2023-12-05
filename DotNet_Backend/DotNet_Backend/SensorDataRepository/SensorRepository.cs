using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataRepository
{
	public interface ISensorRepository
	{
		Task SaveSensorData(SensorData data);
	}

	public class SensorRepository: ISensorRepository
	{
		private SensorContext _sensorContext;
		public SensorRepository(SensorContext sensorContext)
		{
			_sensorContext = sensorContext;
		}

		public async Task SaveSensorData(SensorData data)
		{
			try
			{
				_sensorContext.SensorData.Add(data);
				await _sensorContext.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine("DbUpdateException occurred: " + ex.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("An unexpected error occurred: " + e.Message);
			}
		}
	}
}
