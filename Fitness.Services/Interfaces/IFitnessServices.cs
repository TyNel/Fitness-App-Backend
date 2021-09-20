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

        Task<int> AddUser(UserAddRequest user);
     
       
    }
}
