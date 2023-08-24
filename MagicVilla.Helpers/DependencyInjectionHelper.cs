using MagicVilla.DataAccess;
using MagicVilla.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla.Helpers
{
	public static class DependencyInjectionHelper
	{
		public static void InjectDbContext(this IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Data Source=(localdb)\MSSQLServer;Database=Magic_VillaAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
		}
	}

}
