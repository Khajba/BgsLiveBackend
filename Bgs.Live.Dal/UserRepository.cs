using Bgs.Bll.Abstract;
using Bgs.DataConnectionManager.SqlServer;
using Bgs.DataConnectionManager.SqlServer.Extensions;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bgs.Live.Dal
{
    public class UserRepository : SqlServerRepository, IUserRepository
    {
        private const string _schemaUser = "User";

        public UserRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }

        public async Task AddUser(string email, string firstname, string username, string lastname, string password, int statusId, string pincode, string personalId, int genderId, DateTime registrationDate, DateTime birthDate, string address)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.AddUser"))
            {
                cmd.AddParameter("Email", email);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Username", username);
                cmd.AddParameter("Lastname", lastname);
                cmd.AddParameter("Password", password);
                cmd.AddParameter("StatusId", statusId);
                cmd.AddParameter("PinCode", pincode);
                cmd.AddParameter("PersonalNumber", personalId);
                cmd.AddParameter("GenderId", genderId);
                cmd.AddParameter("BirthDate", birthDate);
                cmd.AddParameter("Address", address);
                cmd.AddParameter("RegistrationDate", registrationDate);

               await cmd.ExecuteNonQueryAsync();
            };
        }

        public async Task AddUserAddress(int userId, string address)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.AddUserAddress"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Address", address);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<string> GetAvailablePincode()
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetAvailablePincode"))
            {
                return await cmd.ExecuteReaderPrimitiveAsync<string>("Pincode");
            };
        }

        public async Task<decimal?> GetBalance(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetBalance"))
            {
                cmd.AddParameter("UserId", userId);

                return await cmd.ExecuteReaderPrimitiveAsync<decimal?>("Balance");
            };
        }

        public async Task<User> GetUserByUsername(string username)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByUsername"))
            {
                cmd.AddParameter("Username", username);


                return await cmd.ExecuteReaderSingleAsync<User>();
            }
        }

        public async Task<User> GetByCredentials(string username, string password)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByCredentials"))
            {
                cmd.AddParameter("Username", username);
                cmd.AddParameter("Password", password);
                

                return await cmd.ExecuteReaderSingleAsync<User>();
            }
        }
        
        public async Task<string>GetUserAddress(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserAddress"))
            {
                cmd.AddParameter("UserId", userId);

                return await cmd.ExecuteReaderPrimitiveAsync<string>("Address");

            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByEmail"))
            {
                cmd.AddParameter("Email", email);

                return await cmd.ExecuteReaderSingleAsync<User>();
            }
        }

        public async Task<User> GetUserById(int Id)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserById"))
            {
                cmd.AddParameter("Id", Id);

                return await cmd.ExecuteReaderSingleAsync<User>();
            }
        }

        public async Task<UserDto> GetUserDetails(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserDetails"))
            {
                cmd.AddParameter("Id", userId);

                return await cmd.ExecuteReaderSingleAsync<UserDto>();
            }
        }

        public async Task<UserForPasswordUpdateDto> GetUserForPasswordUpdate(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserForPasswordUpdate"))
            {
                cmd.AddParameter("Id", userId);

                return await cmd.ExecuteReaderSingleAsync<UserForPasswordUpdateDto>();
            }
        }

        public async Task<IEnumerable<UserListItemDto>> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId)
        {
            using (var cmd = GetSpCommand($"{ _schemaUser}.GetUsers"))
            {
                cmd.AddParameter("PinCode", pinCode);
                cmd.AddParameter("Email", email);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Username", username);
                cmd.AddParameter("Lastname", lastname);
                cmd.AddParameter("PageNumber", pageNumber);
                cmd.AddParameter("PageSize", PageSize);
                cmd.AddParameter("Personald", personalId);

                return await cmd.ExecuteReaderAsync<UserListItemDto>();
            }
        }

        public async Task<int> GetUsersCount(string pinCode, string email, string firstname, string lastname)
        {
            using (var cmd = GetSpCommand($"{ _schemaUser}.GetUsersCount"))
            {
                cmd.AddParameter("PinCode", pinCode);
                cmd.AddParameter("Email", email);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Lastname", lastname);

                return await cmd.ExecuteReaderPrimitiveAsync<int>("Count");
            }
        }

        public async Task ReleasePincode(string pincode, DateTime releaseDate)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.ReleasePincode"))
            {
                cmd.AddParameter("Pincode", pincode);
                cmd.AddParameter("ReleaseDate", releaseDate);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateBalance(int userId, decimal? amount)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateBalance"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Amount", amount);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDetails(int userId, string firstname, string lastname)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserDetails"))
            {
                cmd.AddParameter("Id", userId);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Lastname", lastname);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateUserAddress(int userId, string address)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserAddress"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Address", address);


                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateUserAvatarUrl(int userId, string avatarUrl)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserAvatarUrl"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("AvatarUrl", avatarUrl);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateUserPassword(int userId, string password)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserPassword"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Password", password);


                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
