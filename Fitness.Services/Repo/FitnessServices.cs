﻿using Dapper;
using Fitness.Models.Requests;
using Fitness.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace Fitness.Services.Repo
{
    public class FitnessServices : IFitnessServices
    {
        private readonly IConfiguration _configuration;
        User _user = new User();

        public FitnessServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("Default"));
            }
        }

        public async Task<User> GetById(int id)
        {
            _user = new User();

            using (IDbConnection dbConnection = Connection)
            {
               

                var user = await Connection.QueryAsync<User>("[dbo].[GetFitUsersById]", new {id}, commandType: CommandType.StoredProcedure);

                _user = user.SingleOrDefault();


                return _user;

            }
        }

        public async Task<int> AddUser(UserAddRequest user)
        {
        
            using (IDbConnection dbConnection = Connection )
            {
                var proc = "dbo.Fitness_SingleUser_Insert";
                var parameter = new DynamicParameters();

                parameter.Add("userId", 0, DbType.Int32, ParameterDirection.Output);
                parameter.Add("@firstName", user.FirstName);
                parameter.Add("@lastName", user.LastName);
                parameter.Add("@email", user.Email);
                parameter.Add("@password", user.Password);

                await Connection.QueryAsync<int>(proc, parameter, commandType: CommandType.StoredProcedure);

                int newIdentity = parameter.Get<int>("@userId");

                return newIdentity;

            };
            }

        }
    }

    
