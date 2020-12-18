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
using System.Threading.Tasks;

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

        public async Task RegisterUser(string email, string firstname, string username, string lastname, string password, string personalId, int genderId, DateTime birthDate, string address)
        {
            var pincode =await  _userRepository.GetAvailablePincode();
            var user = await _userRepository.GetUserByEmail(email);

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
                await _userRepository.AddUser(email, firstname, username, lastname, password.ToSHA256(pincode), (int)UserStatus.Active, pincode, personalId, genderId, DateTime.Now, birthDate, address);
                await _userRepository.ReleasePincode(pincode, DateTime.Now);

                transaction.Complete();
            }
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            var user =await _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                throw new BgsException((int)WebApiErrorCodes.UsernameOrPasswordIncorrect);
            }

            if (user.StatusId != (int)UserStatus.Active)
            {
                throw new BgsException((int)WebApiErrorCodes.UsernameOrPasswordIncorrect);
            }

            user = await _userRepository.GetByCredentials(username, password.ToSHA256(user.Pincode));

            return user;


        }

        public async Task<User> GetUserById(int Id)
        {
            return await _userRepository.GetUserById(Id);
        }

        public async Task SaveDetails(int userId, string firstname, string lastname)
        {
            await _userRepository.UpdateDetails(userId, firstname, lastname);
        }

        public async Task SaveUserAddress(int userId, string address)
        {
            var currentaddress =await _userRepository.GetUserAddress(userId);
            if (currentaddress == null)
            {
               await _userRepository.AddUserAddress(userId, address);
            }
            else
            {
                await _userRepository.UpdateUserAddress(userId, address);
            }

        }

        public async Task<string> GetUserAddress(int userId)
        {
            return await _userRepository.GetUserAddress(userId);
        }

        public async Task ChangeUserPassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetUserForPasswordUpdate(userId);

            if (user.Password == oldPassword.ToSHA256(user.Pincode))
            {
               await _userRepository.UpdateUserPassword(userId, newPassword.ToSHA256(user.Pincode));
            }

            else
            {
                throw new BgsException((int)WebApiErrorCodes.OldPasswordIsIncorrect);
            }
        }

        public async Task AddBalance(int userId, decimal amount)
        {
            var balance = await _userRepository.GetBalance(userId) ?? 0;
            balance = balance + amount;

            using (var transaction = new BgsTransactionScope())
            {
                await _userRepository.UpdateBalance(userId, balance);
                await _transactionRepository.AddTransaction((int)TransactionType.Deposit, userId, DateTime.Now, amount);
                transaction.Complete();


            };
        }

        public async Task<decimal> GetBalance(int userId)
        {
            return await _userRepository.GetBalance(userId) ?? 0;
        }

        public async Task<string> UploadUserAvatar(int userId, IFormFile file)
        {
            var multiContent = file.ToHttpContent();

            var response = _httpClient.PostAsync($"{_multimediaApiBaseUri}/image/add", multiContent).Result;

            if (response.IsSuccessStatusCode)
            {
                var avatarUrl = response.Content.ReadAsStringAsync().Result;
                await _userRepository.UpdateUserAvatarUrl(userId, avatarUrl);
                return avatarUrl;
            }

            throw new BgsException((int)WebApiErrorCodes.CouldNotUploadAvatar);
        }

        public async Task DeleteAvatar(int userId)
        {
           await _userRepository.UpdateUserAvatarUrl(userId, null);
        }

        public async Task<int> GetUsersCount(string pinCode, string email, string firstname, string lastname)
        {
            return await _userRepository.GetUsersCount(pinCode, email, firstname, lastname);
        }

        public async Task<AdminUserDetailsDto> GetDetails(int userId, int pageNumber, int pageSize)
        {
            var details = await _userRepository.GetUserDetails(userId);
            var transactions =await _transactionRepository.GetTransactions(userId, null, null, null, null, null, null, pageNumber, pageSize);

            return new AdminUserDetailsDto
            {
                UserDetails = details,
                Transactions = transactions
            };
        }

        public async Task<IEnumerable<UserListItemDto>> GetUsers(string pinCode, string email, string firstname, string username, string lastname, int? pageNumber, int? PageSize, string personalId)
        {
            return await _userRepository.GetUsers(pinCode, email, firstname, username, lastname, pageNumber, PageSize, personalId);
        }

        public async Task<UserDto> GetUserAccountDetails(int userId)
        {
           return await  _userRepository.GetUserDetails(userId);
        }
    }
}
