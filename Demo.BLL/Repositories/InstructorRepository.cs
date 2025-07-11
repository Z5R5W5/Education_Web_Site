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
    public class InstructorRepository : IInstructorRepository
    {
        private readonly EducationAppDbContext _educationAppDbContext;

        public InstructorRepository(EducationAppDbContext educationAppDbContext)
        {
            _educationAppDbContext = educationAppDbContext;
        }
        public async Task AddInstructorAsync(Instructor instructor)
        {
            await _educationAppDbContext.Instructors.AddAsync(instructor);
            await _educationAppDbContext.SaveChangesAsync();

        }

        public async Task DeleteInstructorAsync(int id)
        {
            var instructor = await _educationAppDbContext.Instructors.FirstOrDefaultAsync(c => c.Id == id);
            if (instructor is not null)
                _educationAppDbContext.Instructors.Remove(instructor);

            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Instructor>> GetAllAsync()
        {
            var instructors = await _educationAppDbContext.Instructors.OrderBy(i => i.Name).ToListAsync();
            return instructors;
        }

        public async Task<Instructor> GetInstructorAsync(int id)
        {
            var instructor = await _educationAppDbContext.Instructors.Include(x => x.department).Include(x => x.course).FirstOrDefaultAsync(u => u.Id == id);
            return instructor!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Instructor>> SearchInstructorAsync(string SearchString)
        {
            var Ins = await _educationAppDbContext.Instructors.Include(x => x.course).Include(x => x.department).Where(x => x.Name.Contains(SearchString)).ToListAsync();
            return Ins;
        }

        public async Task UpdateInstructorAsync(Instructor instructor)
        {
            var oldInstructor = await _educationAppDbContext.Instructors.FirstOrDefaultAsync(c => c.Id == instructor.Id);
            if (oldInstructor != null)
            {
                oldInstructor.Name = instructor.Name;
                oldInstructor.Address = instructor.Address;
                oldInstructor.PhoneNumber = instructor.PhoneNumber;
                oldInstructor.ImageUrl = instructor.ImageUrl;
                oldInstructor.crs_id = instructor.crs_id;
                oldInstructor.dept_id = instructor.dept_id;
            }
            await _educationAppDbContext.SaveChangesAsync();
        }
    }
}
