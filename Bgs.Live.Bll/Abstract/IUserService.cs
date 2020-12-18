using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface IUserService
    {
        public Task RegisterUser(string email, string firstname, string username, string lastname, string password, string personalId, int genderId,  DateTime birthDate, string address);
        public Task<User> GetUserById(int Id);
        public Task<User> AuthenticateUser(string username, string password);
        public Task SaveDetails(int userId, string phoneNumber);
        public Task SaveUserAddress(int userId, string address);
        public Task<string> GetUserAddress(int userId);
        public Task<UserDto> GetUserAccountDetails(int userId);
        public Task ChangeUserPassword(int userId, string oldPassword, string newPassword);
        public Task AddBalance(int userId, decimal amount);
        public Task<decimal> GetBalance(int userId);
        public Task<string> UploadUserAvatar(int userId, IFormFile file);
        public Task DeleteAvatar(int userId);
        public Task<int> GetUsersCount(string pinCode, string email,string username, string firstname, string lastname, string personalId);
        public Task<AdminUserDetailsDto> GetDetails(int userId, int pageNumber, int pageSize);
        public Task<IEnumerable<UserListItemDto>> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId);

    }
}
