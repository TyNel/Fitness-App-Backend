using Fitness.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Interfaces
{
    public interface IRefreshTokenRepo
    {
        Task Create(RefreshToken refreshtoken);

        Task<RefreshToken> GetByToken(string token);

        Task Delete(int id);

        Task DeleteAll(int userId);


    }
}
