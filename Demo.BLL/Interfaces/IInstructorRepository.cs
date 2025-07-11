using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IInstructorRepository
    {
        Task<List<Instructor>> GetAllAsync();

        Task<Instructor> GetInstructorAsync(int id);

        Task AddInstructorAsync(Instructor instructor);

        Task UpdateInstructorAsync(Instructor instructor);

        Task DeleteInstructorAsync(int id);

        Task<List<Instructor>> SearchInstructorAsync(string SearchString);

        Task<int> SaveChangesAsync();
    }
}
