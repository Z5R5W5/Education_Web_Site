using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetDepartmentsAsync();

        Task<Department> GetDepartmentAsync(int? id);

        Task AddDepartmentAsync(Department department);

        Task UpdateDepartmentAsync(Department department);

        Task DeleteDepartmentAsync(int id);

        Task<List<Department>> SearchDepartmentAsync(string SearchString);
        Task<int> SaveChangesAsync();
    }
}
