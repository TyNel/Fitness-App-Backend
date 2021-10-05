using Fitness.Models.Domain;
using Fitness.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace Fitness.Services.Interfaces
{
    public interface IFitnessServices
    {
        Task<User> GetById(int id);

        Task<IEnumerable<CompletedExercise>> GetExercises(int id);

        Task<IEnumerable<CompletedExercise>> GetExerciseByDate(int id, DateTime userDate);

        Task<IEnumerable<ExerciseType>> GetExerciseType();

        Task<int> AddUser(UserAddRequest user);

        Task<User> Login(UserLogin user);

        Task<int> AddExercise(ExerciseAddRequest exercise);

        Task<UpdateUser> UpdateUser(UpdateUser user);

        Task DeleteExercise(int id);



    }
}
