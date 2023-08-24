using MagicVilla.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicVilla.DataAccess.Interface
{
	public interface IVillaRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int id);

		Task<List<T>> GetAllAsync();

		Task CreateAsync(T entity);

		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);

		Task Save();
	}
}
