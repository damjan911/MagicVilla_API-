using MagicVilla.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicVilla.DataAccess.Implementation;
using MagicVilla.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.DataAccess.Implementation
{
	public class VillaRepository : IVillaRepository<Villa>
	{
		private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public async Task CreateAsync(Villa entity)
		{
			await _db.Villa.AddAsync(entity);
			await Save();
		}

		public async Task DeleteAsync(Villa entity)
		{
			_db.Villa.Remove(entity);
			await Save();
		}

		public async Task<List<Villa>> GetAllAsync()
		{
			return await _db.Villa.ToListAsync();
		}

		public async Task<Villa> GetByIdAsync(int id)
		{
			return await _db.Villa.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task UpdateAsync(Villa entity)
		{
			_db.Villa.Update(entity);
			await Save();
		}

		public async Task Save()
		{
			await _db.SaveChangesAsync();
		}
	}
}
