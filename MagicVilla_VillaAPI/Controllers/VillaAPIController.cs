using AutoMapper;
using MagicVilla.DataAccess;
using MagicVilla.Domain.Models;
using MagicVilla.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public VillaAPIController(ApplicationDbContext db, IMapper mapper) 
		{
			_db = db;
			_mapper = mapper;
		}

		// This method will return list of Villa.
		// Here I get all Villas.

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
		{
			IEnumerable<Villa> villaList = await _db.Villa.ToListAsync();
			return Ok(_mapper.Map<List<VillaDTO>>(villaList));
		}

		// Here I get only one Villa.

		[HttpGet("{id:int}",Name="GetVilla")]  //http://localhost:[port]/api/VillaAPI/2
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<ActionResult<VillaDTO>> GetVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest("Invalid input. The Id can not be null.");
				}

				if (id <= 0)
				{
					return BadRequest("Invalid Input. Id can not be a negative number.");
				}

				var villa = await _db.Villa.FirstOrDefaultAsync(x => x.Id == id);

				if (villa == null)
				{
					return NotFound();
				}
				return Ok(_mapper.Map<VillaDTO>(villa));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Please contact the support team");
			}
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		// Always when we are working with HttpPost the object that we receive here is FromBody.
		// When we are creating Villa, the ID of the Villa should be zero.
		public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
		{
			// Here we check is it already created the new Entity with the same Name.
			if (await _db.Villa.FirstOrDefaultAsync(u => u.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
			{
				ModelState.AddModelError("CustomError", "Villa is already created");
				return BadRequest(ModelState);
			};

			if (villaCreateDTO == null)
			{
				return BadRequest(villaCreateDTO);
			}

			//if (villaDTO.Id < 0)
			//{
			//	return StatusCode(StatusCodes.Status500InternalServerError, "The Id which is lower than zero can not be created.");
			//}

			// We retrive the meximum Id and increment that by one and assign that to VillaDTO.

			Villa model = _mapper.Map<Villa>(villaCreateDTO);

			//Villa model = new()
			//{
			//	Amenity = villaCreateDTO.Amenity,
			//	Details = villaCreateDTO.Details,
			//	ImageUrl = villaCreateDTO.ImageUrl,
			//	Name = villaCreateDTO.Name,
			//	Occupancy = villaCreateDTO.Occupancy,
			//	Rate = villaCreateDTO.Rate,
			//	Sqrt = villaCreateDTO.Sqrt
			//};
			
			await _db.Villa.AddAsync(model);
			await _db.SaveChangesAsync();

			return CreatedAtRoute("GetVilla",new {id = model.Id },model);

		}

		[HttpDelete("{id:int}",Name ="DeleteVilla")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType (StatusCodes.Status204NoContent)]
		[ProducesResponseType (StatusCodes.Status500InternalServerError)]

		public async Task<ActionResult<VillaDTO>> DeleteVilla(int id)
		{
			try
			{
				if (id == 0)
				{
					return BadRequest("Invalid input. The Id can not be null.");
				}
				var villa = await _db.Villa.FirstOrDefaultAsync(x => x.Id == id);

				if (villa == null)
				{
					return NotFound();
				}

				_db.Villa.Remove(villa);
				await _db.SaveChangesAsync();
				return NoContent();

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Please contact the support team");
			}
		}

		[HttpPut("{id:int}",Name ="UpdateVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IActionResult> UpdateVilla(int id,[FromBody]VillaUpdateDTO villaUpdateDTO)
		{
			try
			{
				if(villaUpdateDTO == null || id != villaUpdateDTO.Id)
				{
					return BadRequest();
				}

				var villa = await _db.Villa.FirstOrDefaultAsync(x => x.Id == id);


				Villa model = _mapper.Map<Villa>(villaUpdateDTO);

				//Villa model = new()
				//{
				//	Amenity = villaUpdateDTO.Amenity,
				//	Details = villaUpdateDTO.Details,
				//	Id = villaUpdateDTO.Id,
				//	ImageUrl = villaUpdateDTO.ImageUrl,
				//	Name = villaUpdateDTO.Name,
				//	Occupancy = villaUpdateDTO.Occupancy,
				//	Rate = villaUpdateDTO.Rate,
				//	Sqrt = villaUpdateDTO.Sqrt
				//};

				_db.Villa.Update(model);
				await _db.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Please contact the support team");
			}
		}

		[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
		{
			try
			{
				if(patchDTO == null || id == 0)
				{
					return BadRequest("Invalid input");
				}

				var villa = await _db.Villa.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

				VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa); 

				//VillaUpdateDTO villaDTO = new()
				//{
				//	Amenity = villa.Amenity,
				//	Details = villa.Details,
				//	Id = villa.Id,
				//	ImageUrl = villa.ImageUrl,
				//	Name = villa.Name,
				//	Occupancy = villa.Occupancy,
				//	Rate = villa.Rate,
				//	Sqrt = villa	.Sqrt
				//};

				if(villa == null)
				{
					return NotFound();
				}

				patchDTO.ApplyTo(villaDTO);

				Villa model = _mapper.Map<Villa>(villaDTO);

				//Villa model = new Villa()
				//{
				//	Amenity = villaDTO.Amenity,
				//	Details = villaDTO.Details,
				//	Id = villa.Id,
				//	ImageUrl = villa.ImageUrl,
				//	Name = villa.Name,
				//    Occupancy= villa.Occupancy,
				//	Rate = villa.Rate,
				//	Sqrt = villa .Sqrt
				//};

				//_db.Villa.Update(model);
				await _db.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Please contact the support team");
			}
		}
	}
}
