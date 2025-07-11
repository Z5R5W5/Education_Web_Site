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
    public class CourseResultRepository : ICourseResultRepository
    {
        private readonly EducationAppDbContext _educationAppDbContext;

        public CourseResultRepository(EducationAppDbContext educationAppDbContext)
        {
            _educationAppDbContext = educationAppDbContext;
        }
        public async Task AddCourseResultAsync(CourseResult crsResult)
        {
            await _educationAppDbContext.CourseResult.AddAsync(crsResult);
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task DeleteCourseResultAsync(int id)
        {
            var result = await _educationAppDbContext.CourseResult.FirstOrDefaultAsync(c => c.Id == id);
            if (result is not null)
                _educationAppDbContext.CourseResult.Remove(result);
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<CourseResult>> GetAllAsync()
        {
            var results = await _educationAppDbContext.CourseResult.Include(x => x.Course).Include(x => x.Trainee).OrderBy(x => x.trainee_id).ToListAsync();
            return results;
        }

        public async Task<CourseResult> GetCourseResultAsync(int id)
        {
            var result = await _educationAppDbContext.CourseResult.Include(x => x.Course).Include(x => x.Trainee).FirstOrDefaultAsync(u => u.Id == id);
            return result!;
        }

        public async Task<List<CourseResult>> GetResultsByIdAsync(int id)
        {
            var result = await _educationAppDbContext.CourseResult.Include(x => x.Course).Include(x => x.Trainee).Where(x => x.trainee_id == id).ToListAsync();
            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _educationAppDbContext.SaveChangesAsync();

        }

        public async Task<List<CourseResult>> SearchCourseResultAsync(string SearchString)
        {
            var item = await _educationAppDbContext.CourseResult.Include(x => x.Trainee).Include(x => x.Course).Where(x => x.Trainee!.Name.StartsWith(SearchString)).ToListAsync();
            return item!;
        }

        public async Task UpdateCourseResultAsync(CourseResult crsResult)
        {
            var result = await _educationAppDbContext.CourseResult.FirstOrDefaultAsync(c => c.Id == crsResult.Id);
            if (result != null)
            {
                result.Degree = crsResult.Degree;
                result.crs_id = crsResult.crs_id;
                result.trainee_id = crsResult.trainee_id;
            }
            await _educationAppDbContext.SaveChangesAsync();
        }
    }
}
