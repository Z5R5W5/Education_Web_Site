using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursePerDepartmentAsync(int deptID);

        Task<List<Course>> GetAllAsync();

        Task<Course> GetCourseAsync(int? id);

        Task AddCourseAsync(Course course);

        Task UpdateCourseAsync(Course course);

        Task DeleteCourseAsync(int id);

        Task<List<Course>> SearchCourseAsync(string SearchString);

        Task<int> SaveChangesAsync();

        Task<IEnumerable<Course>> GetCoursesByDepartmentIdAsync(int deptId);
    }
}
