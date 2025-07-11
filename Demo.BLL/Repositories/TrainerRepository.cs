using Demo.BLL.Interfaces;
using Demo.DAL.DbContext;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly EducationAppDbContext _educationAppDbContext;

        public TrainerRepository(EducationAppDbContext educationAppDbContext)
        {
            _educationAppDbContext = educationAppDbContext;
        }

        public async Task AddTraineeAsync(Trainee trainee)
        {
            await _educationAppDbContext.Trainees.AddAsync(trainee);
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task DeleteTraineeAsync(int id)
        {
            var Trainee = await _educationAppDbContext.Trainees.FirstOrDefaultAsync(c => c.Id == id);
            if (Trainee is not null)
                _educationAppDbContext.Trainees.Remove(Trainee);
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Trainee>> GetAllAsync()
        {
            var trainees = await _educationAppDbContext.Trainees.OrderBy(t => t.Name).ToListAsync();
            return trainees;
        }

        public async Task<Trainee> GetTraineeAsync(int id)
        {
            var trainee = await _educationAppDbContext.Trainees.Include(x => x.department).FirstOrDefaultAsync(u => u.Id == id);
            return trainee!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Trainee>> SearchTraineeAsync(string SearchString)
        {
            var Tra = await _educationAppDbContext.Trainees.Include(d => d.department).Where(x => x.Name.StartsWith(SearchString)).ToListAsync();
            return Tra;
        }

        public async Task UpdateTraineeAsync(Trainee trainee)
        {
            var oldTrainee = await _educationAppDbContext.Trainees.FirstOrDefaultAsync(c => c.Id == trainee.Id);
            if (oldTrainee != null)
            {
                oldTrainee.Name = trainee.Name;
                oldTrainee.Address = trainee.Address;
                oldTrainee.Level = trainee.Level;
                oldTrainee.PhoneNumber = trainee.PhoneNumber;
                oldTrainee.ImageUrl = trainee.ImageUrl;
                oldTrainee.dept_id = trainee.dept_id;
            }
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<Trainee> GetTraineeWithDepartmentAsync(int id)
        {
            var trainee = await _educationAppDbContext.Trainees.Include(t => t.department).FirstOrDefaultAsync(t => t.Id == id);
            return trainee!;
        }
    }
}
