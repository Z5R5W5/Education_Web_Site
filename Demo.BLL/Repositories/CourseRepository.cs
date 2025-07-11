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
    public class CourseRepository : ICourseRepository
    {
        private readonly EducationAppDbContext _educationAppDbContext;

        public CourseRepository(EducationAppDbContext educationAppDbContext) 
        {
            _educationAppDbContext = educationAppDbContext;
        }
        public async Task AddCourseAsync(Course course)
        {
           await _educationAppDbContext.Courses.AddAsync(course);
           _educationAppDbContext.SaveChanges();

        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _educationAppDbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course is not null)
                _educationAppDbContext.Courses.Remove(course);
            _educationAppDbContext.SaveChanges();

        }

        public async Task<List<Course>> GetAllAsync()
        {
            var courses= await _educationAppDbContext.Courses.OrderBy(c => c.Name).ToListAsync();
            return courses;
        }

        public async Task<Course> GetCourseAsync(int? id)
        {
            var course = await _educationAppDbContext.Courses.FirstOrDefaultAsync(u => u.Id == id);
            return course!;
        }

        public async Task<List<Course>> GetCoursePerDepartmentAsync(int deptID)
        {
            var courses = await _educationAppDbContext.Courses
                .Where(c => c.dept_id == deptID)
                .ToListAsync();
            return courses;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Course>> SearchCourseAsync(string SearchString)
        {
            var item = await _educationAppDbContext.Courses.Include(x => x.department).Where(x => x.Name!.StartsWith(SearchString)).ToListAsync();
            return item;
        }

        public async Task UpdateCourseAsync(Course course)
        {
            var oldCourse = await _educationAppDbContext.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);
            if (oldCourse != null)
            {
                oldCourse.Id = course.Id;
                oldCourse.Name = course.Name;
                oldCourse.Grade = course.Grade;
                oldCourse.MinDegree = course.MinDegree;
                oldCourse.dept_id = course.dept_id;
            }
            _educationAppDbContext.SaveChanges();
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentIdAsync(int deptId)
        {
            return await _educationAppDbContext.Courses
                .Where(c => c.dept_id == deptId)
                .ToListAsync();
        }
    }
}
