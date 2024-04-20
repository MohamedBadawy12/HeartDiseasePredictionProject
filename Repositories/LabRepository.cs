﻿using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly AppDbContext _context;
        public LabRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Lab lab) => await _context.Labs.AddAsync(lab);

        public void Delete(Lab lab) => _context.Labs.Remove(lab);

        public async Task<Lab> GetLab(int id) =>
            await _context.Labs.
             Include(d => d.User).
            FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<Lab>> GetLabs() =>
            await _context.Labs.Include(d => d.User).ToListAsync();

        public async Task<NewLabDropDownViewMode> GetLabDropDownsValues()
        {
            var data = new NewLabDropDownViewMode()
            {
                labs = await _context.Labs.OrderBy(a => a.Name).ToListAsync(),
            };
            return data;
        }

        public Lab Get_Lab(int id) => _context.Labs
            .Include(d => d.User)
            .FirstOrDefault(l => l.Id == id);

        public bool DeleteLab(int id)
        {
            var isDeleted = false;

            var lab = _context.Labs.Find(id);

            if (lab is null)
                return isDeleted;

            _context.Remove(lab);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
            }

            return isDeleted;
        }

        public async Task<IEnumerable<Lab>> SearchForLab(string name, string location)
        {
            var labs = await GetLabs();
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(location))
            {
                labs = await _context.Labs.
                Where(x => x.Name.Contains(name) || x.Location.Contains(location)).ToListAsync();
            }
            return labs;
        }
    }
}
