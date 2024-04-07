using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
	public class MedicalAnalystRepository : IMedicalAnalystRepository
	{
		private readonly AppDbContext _context;
		public MedicalAnalystRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task Add(MedicalAnalyst medicalAnalyst) => await _context.MedicalAnalysts.AddAsync(medicalAnalyst);

		public async Task<IEnumerable<MedicalAnalyst>> GetMedicalAnalysts()
		{
			return await _context.MedicalAnalysts
				 .Include(m => m.User)
				 .Include(m => m.Lab)
				 .ToListAsync();
		}

		public async Task<MedicalAnalyst> GetMedicalAnalyst(int id)
		{
			return await _context.MedicalAnalysts
				 .Include(m => m.User)
				 .Include(m => m.Lab)
				 .Include(m => m.medicalTests)
				 .FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<MedicalAnalyst> GetProfile(string userId)
		{
			return await _context.MedicalAnalysts
				 .Include(m => m.User)
				 .Include(m => m.Lab)
				 .Include(m => m.medicalTests)
				 .FirstOrDefaultAsync(m => m.UserId == userId);
		}

		public void Remove(MedicalAnalyst medicalAnalyst) =>
			_context.MedicalAnalysts.Remove(medicalAnalyst);

		public MedicalAnalyst Get_MedicalAnalyst(int id)
		{
			return _context.MedicalAnalysts
				 .Include(m => m.User)
				 .Include(m => m.Lab)
				 .Include(m => m.medicalTests)
				 .FirstOrDefault(m => m.Id == id);
		}

		public async Task<IEnumerable<MedicalAnalyst>> FilterMedicalAnalyst(string search)
		{
			var medicalAnalysts = await GetMedicalAnalysts();
			if (!string.IsNullOrEmpty(search))
			{
				medicalAnalysts = await _context.MedicalAnalysts.
				Where(x => x.User.FirstName.Contains(search) || x.User.LastName.Contains(search)).ToListAsync();
			}
			return medicalAnalysts;
		}

		public bool Delete(int id)
		{
			var isDeleted = false;

			var medicalAnalyst = _context.MedicalAnalysts
				 .Include(m => m.User)
				 .Include(m => m.Lab)
				 .Include(m => m.medicalTests)
				 .FirstOrDefault(m => m.Id == id);

			if (medicalAnalyst is null)
				return isDeleted;

			_context.Remove(medicalAnalyst);
			var effectedRows = _context.SaveChanges();

			if (effectedRows > 0)
			{
				isDeleted = true;
			}

			return isDeleted;
		}
	}
}
