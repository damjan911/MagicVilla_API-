using AutoMapper;
using MagicVilla.Domain.Models;
using MagicVilla.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla.Mappers
{
	public class MappingConfig : Profile
	{
		public MappingConfig() 
		{
			CreateMap<Villa, VillaDTO>().ReverseMap();

			CreateMap<Villa,VillaCreateDTO>().ReverseMap();
			CreateMap<Villa,VillaUpdateDTO>().ReverseMap();

		}
	}
}
