using Bgs.Bll.Abstract;
using Bgs.DataConnectionManager.SqlServer.SqlClient;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Dtos;
using Bgs.Live.Common.Entities;
using Bgs.Live.Common.Enums;
using Bgs.Live.Common.ErrorCodes;
using Bgs.Live.Core.Exceptions;
using Bgs.Live.Dal.Abstract;
using Bgs.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bgs.Live.Bll
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;
        private readonly string _multimediaApiBaseUri;
        private readonly ITransactionRepository _transactionRepository;


        public UserService(
            IUserRepository userRepository, 
            IHttpClientFactory httpClientFactory,
            ITransactionRepository transactionRepository,
            IConfiguration configuration)

        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _httpClient = httpClientFactory.CreateClient();
            _multimediaApiBaseUri = configuration["MultimediaApiBaseUri"];
            
        }
        public void RegisterUser(string email, string firstname, string username, string lastname, string password, string personalId, string gender, DateTime birthDate, string address)
        {
            var pincode = _userRepository.GetAvailablePincode();
            var user = _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                throw new BgsException((int)WebApiErrorCodes.EmailAlreadyExists);
            }

            var userbyUsername = _userRepository.GetUserByUsername(username);

            if (userbyUsername != null)
            {
                throw new BgsException((int)WebApiErrorCodes.UsernameAlreadyExists);
            }

            using (var transaction = new BgsTransactionScope())
            {

                _userRepository.AddUser(email, firstname, username,  lastname, password.ToSHA256(pincode), (int)UserStatus.Active, pincode,personalId,gender,birthDate,address);
                _userRepository.ReleasePincode(pincode, DateTime.Now);

                transaction.Complete();
            }
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);            

            if (user == null)
            {
                throw new BgsException((int)WebApiErrorCodes.EmailOrPasswordIncorrect);
            }

            if (user.StatusId != (int)UserStatus.Active)
            {
                throw new BgsException((int)WebApiErrorCodes.EmailOrPasswordIncorrect);
            }

            user = _userRepository.GetByCredentials(username, password.ToSHA256(user.Pincode), (int)UserStatus.Active);

            return user;


        }

        public User GetUserById(int Id)
        {
            return _userRepository.GetUserById(Id);
        }

        public void SaveDetails(int userId, string firstname, string lastname)
        {
            _userRepository.UpdateDetails(userId, firstname, lastname);
        }

        public void SaveUserAddress(int userId, string address)
        {
            var currentaddress = _userRepository.GetUserAddress(userId);
            if (currentaddress == null)
            {
                _userRepository.AddUserAddress(userId, address);
            }
            else
            {
                _userRepository.UpdateUserAddress(userId, address);
            }

        }

        public string GetUserAddress(int userId)
        {
           return  _userRepository.GetUserAddress(userId);
        }

        public void ChangeUserPassword(int userId, string oldPassword, string newPassword)
        {
            var user = _userRepository.GetUserForPasswordUpdate(userId);

            if (user.Password == oldPassword.ToSHA256(user.Pincode))
            {
                _userRepository.UpdateUserPassword(userId, newPassword.ToSHA256(user.Pincode));
            }

            else
            {
                throw new BgsException((int)WebApiErrorCodes.OldPasswordIsIncorrect);
            }
        }

        public void AddBalance(int userId, decimal amount)
        {
            var balance = _userRepository.GetBalance(userId) ?? 0;
            balance = balance + amount;

            using (var transaction = new BgsTransactionScope())
            {
                _userRepository.UpdateBalance(userId, balance);
                _transactionRepository.AddTransaction((int)TransactionType.Deposit, userId, DateTime.Now, amount);
                transaction.Complete();


            };
        }

        public decimal GetBalance(int userId)
        {
            return _userRepository.GetBalance(userId) ?? 0;
        }

        public string UploadUserAvatar(int userId, IFormFile file)
        {
            var multiContent = file.ToHttpContent();

            var response = _httpClient.PostAsync($"{_multimediaApiBaseUri}/image/add", multiContent).Result;

            if (response.IsSuccessStatusCode)
            {
                var avatarUrl = response.Content.ReadAsStringAsync().Result;
                _userRepository.UpdateUserAvatarUrl(userId, avatarUrl);
                return avatarUrl;
            }

            throw new BgsException((int)WebApiErrorCodes.CouldNotUploadAvatar);
        }

        public void DeleteAvatar(int userId)
        {
            _userRepository.UpdateUserAvatarUrl(userId, null);
        }

        public int GetUsersCount(string pinCode, string email, string firstname, string lastname)
        {
            return _userRepository.GetUsersCount(pinCode, email, firstname, lastname);
        }

        public AdminUserDetailsDto GetDetails(int userId, int pageNumber, int pageSize)
        {
            var details = _userRepository.GetUserDetails(userId);            
            var transactions = _transactionRepository.GetTransactions(userId, null, null, null, null, null, null, pageNumber, pageSize);

            return new AdminUserDetailsDto
            {
                UserDetails = details,                
                Transactions = transactions
            };
        }

        public IEnumerable<UserListItemDto> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId)
        {
            return _userRepository.GetUsers(pinCode, email, firstname, username, lastname, pageNumber, PageSize, personalId);
        }

        public UserAccountDto GetUserAccountDetails(int userId)
        {
            var userDetails = _userRepository.GetUserDetails(userId);
            var userAddress = _userRepository.GetUserAddress(userId);
            

            return new UserAccountDto
            {
                UserDetails = userDetails,
                UserAddress = userAddress,
                
            };
        }
    }
}
