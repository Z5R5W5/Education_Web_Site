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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EducationAppDbContext _educationAppDbContext;

        public DepartmentRepository(EducationAppDbContext educationAppDbContext)
        {
            _educationAppDbContext = educationAppDbContext;
        }
        public async Task AddDepartmentAsync(Department department)
        {
           await _educationAppDbContext.Departments.AddAsync(department);
            await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _educationAppDbContext.Departments.FirstOrDefaultAsync(c => c.Id == id);
            if (department is not null)
                _educationAppDbContext.Departments.Remove(department);
            _educationAppDbContext.SaveChanges();
        }

        public async Task<Department> GetDepartmentAsync(int? id)
        {
            var department = await _educationAppDbContext.Departments.FirstOrDefaultAsync(u => u.Id == id);
            return department!;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            var departments = await _educationAppDbContext.Departments.OrderBy(d => d.Name).ToListAsync();
            return departments;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _educationAppDbContext.SaveChangesAsync();
        }

        public async Task<List<Department>> SearchDepartmentAsync(string SearchString)
        {
            var item = await _educationAppDbContext.Departments.Where(x => x.Name.StartsWith(SearchString)).ToListAsync();
            return item;
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var oldDepartment = _educationAppDbContext.Departments.FirstOrDefault(c => c.Id == department.Id);
            if (oldDepartment != null)
            {
                oldDepartment.Name = department.Name;
                oldDepartment.DeptManager = department.DeptManager;
            }
            await _educationAppDbContext.SaveChangesAsync();
        }
    }
}
