using Bgs.Bll.Abstract;
using Bgs.DataConnectionManager.SqlServer;
using Bgs.DataConnectionManager.SqlServer.Extensions;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Bgs.Live.Dal
{
    public class UserRepository : SqlServerRepository, IUserRepository
    {
        private const string _schemaUser = "User";

        public UserRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }

        public void AddUser(string email, string firstname, string username, string lastname, string password, int statusId, string pincode, string personalId, int genderId, DateTime registrationDate, DateTime birthDate, string address)
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

                cmd.ExecuteNonQuery();
            };
        }

        public void AddUserAddress(int userId, string address)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.AddUserAddress"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Address", address);

                cmd.ExecuteNonQuery();
            }
        }

        public string GetAvailablePincode()
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetAvailablePincode"))
            {
                return cmd.ExecuteReaderPrimitive<string>("Pincode");
            };
        }

        public decimal? GetBalance(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetBalance"))
            {
                cmd.AddParameter("UserId", userId);

                return cmd.ExecuteReaderPrimitive<decimal?>("Balance");
            };
        }

        public User GetUserByUsername(string username)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByUsername"))
            {
                cmd.AddParameter("Username", username);


                return cmd.ExecuteReaderSingle<User>();
            }
        }

        public User GetByCredentials(string username, string password, int statusId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByCredentials"))
            {
                cmd.AddParameter("Username", username);
                cmd.AddParameter("Password", password);
                cmd.AddParameter("StatusIdActive", statusId);

                return cmd.ExecuteReaderSingle<User>();
            }
        }

        public string GetUserAddress(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserAddress"))
            {
                cmd.AddParameter("UserId", userId);

                return cmd.ExecuteReaderPrimitive<string>("Address");

            }
        }

        public User GetUserByEmail(string email)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserByEmail"))
            {
                cmd.AddParameter("Email", email);

                return cmd.ExecuteReaderSingle<User>();
            }
        }

        public User GetUserById(int Id)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserById"))
            {
                cmd.AddParameter("Id", Id);

                return cmd.ExecuteReaderSingle<User>();
            }
        }

        public UserDto GetUserDetails(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserDetails"))
            {
                cmd.AddParameter("Id", userId);

                return cmd.ExecuteReaderSingle<UserDto>();
            }
        }

        public UserForPasswordUpdateDto GetUserForPasswordUpdate(int userId)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.GetUserForPasswordUpdate"))
            {
                cmd.AddParameter("Id", userId);

                return cmd.ExecuteReaderSingle<UserForPasswordUpdateDto>();
            }
        }

        public IEnumerable<UserListItemDto> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId)
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

                return cmd.ExecuteReader<UserListItemDto>();
            }
        }

        public int GetUsersCount(string pinCode, string email, string firstname, string lastname)
        {
            using (var cmd = GetSpCommand($"{ _schemaUser}.GetUsersCount"))
            {
                cmd.AddParameter("PinCode", pinCode);
                cmd.AddParameter("Email", email);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Lastname", lastname);

                return cmd.ExecuteReaderPrimitive<int>("Count");
            }
        }

        public void ReleasePincode(string pincode, DateTime releaseDate)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.ReleasePincode"))
            {
                cmd.AddParameter("Pincode", pincode);
                cmd.AddParameter("ReleaseDate", releaseDate);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBalance(int userId, decimal? amount)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateBalance"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Amount", amount);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDetails(int userId, string firstname, string lastname)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserDetails"))
            {
                cmd.AddParameter("Id", userId);
                cmd.AddParameter("Firstname", firstname);
                cmd.AddParameter("Lastname", lastname);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUserAddress(int userId, string address)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserAddress"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Address", address);


                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUserAvatarUrl(int userId, string avatarUrl)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserAvatarUrl"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("AvatarUrl", avatarUrl);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUserPassword(int userId, string password)
        {
            using (var cmd = GetSpCommand($"{_schemaUser}.UpdateUserPassword"))
            {
                cmd.AddParameter("UserId", userId);
                cmd.AddParameter("Password", password);


                cmd.ExecuteNonQuery();
            }
        }
    }
}
