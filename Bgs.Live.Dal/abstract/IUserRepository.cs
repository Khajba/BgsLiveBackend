using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;

namespace Bgs.Bll.Abstract
{
    public interface IUserRepository
    {
        public User GetUserByEmail(string email);
        public User GetUserByUsername(string username);
        public void AddUser(string email, string firstname, string username, string lastname, string password, int statusId, string pincode, string personalId, int genderId, DateTime RegistrationDate, DateTime birthDate, string address);
        public User GetUserById(int Id);
        public string GetAvailablePincode();
        public void ReleasePincode(string pincode, DateTime releaseDate);
        public User GetByCredentials(string username, string password, int statusId);
        public UserDto GetUserDetails(int userId);
        public string GetUserAddress(int userId);
        public void UpdateDetails(int userId, string firstname, string lastname);
        public void UpdateUserAddress(int userId, string address);
        public void AddUserAddress(int userId, string address);
        public void UpdateUserPassword(int userId, string password);
        public void UpdateBalance(int userId, decimal? amount);
        public decimal? GetBalance(int userId);
        public UserForPasswordUpdateDto GetUserForPasswordUpdate(int userId);
        public void UpdateUserAvatarUrl(int userId, string avatarUrl);
        public IEnumerable<UserListItemDto> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId);
        public int GetUsersCount(string pinCode, string email, string firstname, string lastname);
    }

}
