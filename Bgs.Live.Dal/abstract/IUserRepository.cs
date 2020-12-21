using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bgs.Bll.Abstract
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail(string email);
        public Task<User> GetUserByUsername(string username);
        public Task AddUser(string email, string firstname, string username, string lastname, string password, int statusId, string pincode, string personalId, int genderId, DateTime RegistrationDate, DateTime birthDate, string address);
        public Task<User> GetUserById(int Id);
        public Task<string> GetAvailablePincode();
        public Task<User> GetUserByPersonalNumber(string personalNumber);
        public Task ReleasePincode(string pincode, DateTime releaseDate);
        public Task<User> GetByCredentials(string username, string password);
        public Task<UserDto> GetUserDetails(int userId);        
        public Task UpdateDetails(int userId, string firstname, string lastname, DateTime birthDate, int genderId, string address, string phoneNumber);       
        public Task UpdateUserPassword(int userId, string password);
        public Task UpdateBalance(int userId, decimal? amount);
        public Task<decimal?> GetBalance(int userId);
        public Task<UserForPasswordUpdateDto> GetUserForPasswordUpdate(int userId);
        public Task UpdateUserAvatarUrl(int userId, string avatarUrl);
        public Task<IEnumerable<UserListItemDto>> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId);
        public Task<int> GetUsersCount(string pinCode, string email, string username, string firstname, string lastname, string personalId);
    }

}
