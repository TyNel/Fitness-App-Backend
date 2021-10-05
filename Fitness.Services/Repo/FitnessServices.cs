using Dapper;
using Fitness.Models.Domain;
using Fitness.Models.Requests;
using Fitness.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

        UpdateUser updated_user = new UpdateUser();

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

                var user = await Connection.QueryAsync<User>("[dbo].[Fitness_Select_User]", new { id }, commandType: CommandType.StoredProcedure);

                _user = user.SingleOrDefault();


                return _user;

            }
        }

        public async Task<IEnumerable<CompletedExercise>> GetExercises(int id)
        {

            using (IDbConnection dbConnection = Connection)
            {

                var results = await Connection.QueryAsync<CompletedExercise>("dbo.GetUserExById", new { id }, commandType: CommandType.StoredProcedure);

                return results.ToList();

            }
        }

        public async Task<IEnumerable<CompletedExercise>> GetExerciseByDate(int id, DateTime userDate)
        {
            string date = userDate.ToString("yyyy-MM-dd");

            using (IDbConnection dbConnection = Connection)
            {

                var results = await Connection.QueryAsync<CompletedExercise>("FitnessGetByDateId", new { id, date }, commandType: CommandType.StoredProcedure);

                return results.ToList();

            }
        }

        public async Task<int> AddUser(UserAddRequest user)
        {

            using (IDbConnection dbConnection = Connection)
            {
                var proc = "dbo.Fitness_SingleUser_Insert";
                var parameter = new DynamicParameters();

                parameter.Add("userId", 0, DbType.Int32, ParameterDirection.Output);
                parameter.Add("@firstName", user.FirstName);
                parameter.Add("@lastName", user.LastName);
                parameter.Add("@email", user.Email);
                parameter.Add("@password", CommonMethods.Encrypt(user.Password));

                await Connection.QueryAsync<int>(proc, parameter, commandType: CommandType.StoredProcedure);

                int newIdentity = parameter.Get<int>("@userId");

                return newIdentity;

            };
        }

        //public async Task<int> Register(UserAddRequest user)
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {
        //        var proc = "dbo.Fitness_Users_Insert";
        //        var parameter = new DynamicParameters();

        //        var dt = new DataTable();

        //        dt.Columns.Add("firstName");
        //        dt.Columns.Add("lastName");
        //        dt.Columns.Add("email");
        //        dt.Columns.Add("password");

        //        for (int i = 0; i < 5; i++)
        //        {
        //            dt.Rows.Add(user.FirstName + i, user.LastName + i, user.Email + i, user.Password + i);
        //        }


        //        await Connection.QueryAsync<int>(proc, dt, commandType: CommandType.StoredProcedure);

        //        int newIdentity = .Output<int>("@userId");

        //        return newIdentity;
        //    }
        //}

        public async Task<User> Login(UserLogin user)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var proc = "dbo.Fitness_Login";
                var parameter = new DynamicParameters();

                parameter.Add("@email", user.Email);
                parameter.Add("@password", CommonMethods.Encrypt(user.Password));

                var responseUser = await Connection.QueryAsync<User>(proc, parameter, commandType: CommandType.StoredProcedure);

                _user = responseUser.FirstOrDefault();

                return _user;

            };
        }

        public async Task<IEnumerable<ExerciseType>> GetExerciseType()
        {
            using (IDbConnection dbConnection = Connection)
            {
                var proc = "dbo.GetExerciseType";

                ExerciseType _ExerciseType = new ExerciseType();

                var response = await Connection.QueryAsync<ExerciseType>(proc, commandType: CommandType.StoredProcedure);

                return response;

            };
        }

        //public async Task<int> AddExercise(ExerciseAddRequest exercise)
        //{
        //    using (IDbConnection dbConnection = Connection)
        //    {
        //        var proc = "dbo.Fitness_Exercise_Insert";

        //        var dt = new DataTable();

        //        dt.Columns.Add("userId");
        //        dt.Columns.Add("exercise_name");
        //        dt.Columns.Add("weight");
        //        dt.Columns.Add("reps");
        //        dt.Columns.Add("exercise_type");
        //        dt.Columns.Add("status_id");
        //        dt.Columns.Add("userNotes");
        //        dt.Columns.Add("dateAdded");
        //        dt.Columns.Add("dateModified");


        //        for (int i = 0; i < 9; i++)
        //        {
        //            dt.Rows.Add(exercise.UserId + i, exercise.Exercise_Name + i, exercise.Weight + i, exercise.Reps + i, exercise.Exercise_Type + i, exercise.UserNotes + i, exercise.DateAdded + i, exercise.DateModified + i);
        //        }



        //        var result = await Connection.QueryAsync<int>(proc, new { exercise = dt.AsTableValuedParameter("dbo.Exercise_Insert_UDT") }, commandType: CommandType.StoredProcedure);

        //        return result.FirstOrDefault();
        //    }
        //}


        public async Task<int> AddExercise(ExerciseAddRequest exercise)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var proc = "dbo.Fitness_Exercise_Insert_Single";

                var parameter = new DynamicParameters();

                parameter.Add("id", 0, DbType.Int32, ParameterDirection.Output);
                parameter.Add("userId", exercise.UserId);
                parameter.Add("exercise_name", exercise.Exercise_Name);
                parameter.Add("weight", exercise.Weight);
                parameter.Add("reps", exercise.Reps);
                parameter.Add("exercise_type", exercise.Exercise_Type);
                parameter.Add("status_id", exercise.Status_Id);
                parameter.Add("userNotes", exercise.UserNotes);
                parameter.Add("dateAdded", exercise.DateAdded);
                parameter.Add("dateModified", exercise.DateModified);


                var result = await Connection.QueryAsync<int>(proc, parameter, commandType: CommandType.StoredProcedure);

                int newExercises = parameter.Get<int>("id");

                return newExercises;

            }

        }

        public async Task<UpdateUser> UpdateUser(UpdateUser user)
        {
            updated_user = new UpdateUser();

            using (IDbConnection dbConnection = Connection)
            {

                var proc = "dbo.FitnessUserUpdate";
                var parameter = new DynamicParameters();

                parameter.Add("@id", user.userId);
                parameter.Add("@firstName", user.FirstName);
                parameter.Add("@lastName", user.LastName);
                parameter.Add("@email", user.Email);
                parameter.Add("@password", CommonMethods.Encrypt(user.Password));

                var response = await Connection.QueryAsync<UpdateUser>(proc, parameter, commandType: CommandType.StoredProcedure);

                updated_user = response.FirstOrDefault();

                return updated_user;

            }


        }

        public async Task DeleteExercise(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var proc = "dbo.Exercise_Delete";
                var parameter = new DynamicParameters();

                parameter.Add("@id", id);

                await Connection.QueryAsync<int>(proc, parameter, commandType: CommandType.StoredProcedure);

            };
        }

    }

}
  

    

