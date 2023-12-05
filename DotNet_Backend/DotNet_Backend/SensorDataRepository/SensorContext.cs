using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SensorDataRepository
{
	/// <summary>
	/// This class represents the data context that is used for making changes to the sensor values
	/// table in the database
	/// </summary>
    public class SensorContext: DbContext
	{
		public SensorContext(DbContextOptions<SensorContext> options) : base(options)
		{

		}

		public DbSet<SensorData> SensorData { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
