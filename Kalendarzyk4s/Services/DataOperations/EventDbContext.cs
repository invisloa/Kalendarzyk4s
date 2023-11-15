using Kalendarzyk4s.Models.EventModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Kalendarzyk4s.Services.DataOperations
{
	public class EventDbContext : DbContext
	{
		public DbSet<IGeneralEventModel> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=testerDB;Trusted_Connection=True;");
		}
	}
}
