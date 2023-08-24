using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla.Domain.Models
{
	public class Villa : BaseEntity
	{
		public string Name { get; set; }

		public string Details { get; set; }

		public double Rate { get; set; }

		public int Sqrt { get; set; }

		public int Occupancy { get; set; }

		public string ImageUrl { get; set; }

		public string Amenity { get; set; }

		// We didn't want to have this Property to the end User.
		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }
    }
}
