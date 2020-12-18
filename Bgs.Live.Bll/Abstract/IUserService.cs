using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Bgs.Live.Bll.Abstract
{
    public interface IUserService
    {
        public void RegisterUser(string email, string firstname, string username, string lastname, string password, string personalId, int genderId,  DateTime birthDate, string address);
        public User GetUserById(int Id);
        public User AuthenticateUser(string username, string password);
        public void SaveDetails(int userId, string firstname, string lastname);
        public void SaveUserAddress(int userId, string address);
        public string GetUserAddress(int userId);
        public UserAccountDto GetUserAccountDetails(int userId);
        public void ChangeUserPassword(int userId, string oldPassword, string newPassword);
        public void AddBalance(int userId, decimal amount);
        public decimal GetBalance(int userId);
        public string UploadUserAvatar(int userId, IFormFile file);
        public void DeleteAvatar(int userId);
        public int GetUsersCount(string pinCode, string email, string firstname, string lastname);
        public AdminUserDetailsDto GetDetails(int userId, int pageNumber, int pageSize);
        public IEnumerable<UserListItemDto> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId);

    }
}
