using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ITrainerRepository
    {
        Task<List<Trainee>> GetAllAsync();

        Task<Trainee> GetTraineeAsync(int id);

        Task AddTraineeAsync(Trainee trainee);

        Task UpdateTraineeAsync(Trainee trainee);

        Task DeleteTraineeAsync(int id);

        Task<List<Trainee>> SearchTraineeAsync(string SearchString);
        Task<Trainee> GetTraineeWithDepartmentAsync(int id);


        Task<int> SaveChangesAsync();
    }
}
