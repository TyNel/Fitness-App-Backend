using Fitness.Models.Domain;
using Fitness.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Repo
{
    public class InMemoryRefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public Task Create(RefreshToken refreshtoken)
        {
            _refreshTokens.Add(refreshtoken);

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetByToken(string token)
        {
            RefreshToken refreshToken = _refreshTokens.FirstOrDefault(r => r.Token == token);

            return Task.FromResult(refreshToken);
        }

        public Task Delete(int id)
        {
            _refreshTokens.RemoveAll(r => r.UserId == id);

            return Task.CompletedTask;
        }

        public Task DeleteAll(int userId)
        {
            _refreshTokens.RemoveAll(r => r.UserId == userId);

            return Task.CompletedTask;
        }
    }
}
