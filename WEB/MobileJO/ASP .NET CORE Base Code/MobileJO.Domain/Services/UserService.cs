using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using AutoMapper;

namespace MobileJO.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IUserRepository and IMapper
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Calls User repository method FindAll().
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel Search(UserSearchViewModel searchModel)
        {
            return _userRepository.FindAll(searchModel);
        }

        /// <summary>
        ///     Calls User repository method Update().
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        /// <summary>
        ///    Calls User repository method Delete().
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id, string userName)
        {
            _userRepository.Delete(id, userName);
        }

        /// <summary>
        ///     Calls User repository method Find().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserViewModel Find(int id)
        {
            UserViewModel userViewModel = null;

            if (id > 0)
            {
                var user = _userRepository.Find(id);

                if (user != null)
                {
                    userViewModel = _mapper.Map<UserViewModel>(user);
                }
            }

            return userViewModel;
        }

        /// <summary>
        ///     Calls User repository method HasPendingJO().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasPendingJO(int id)
        {
            bool result = false;

            if (id > 0)
            {
                result = _userRepository.HasPendingJO(id);
            }

            return result;
        }

        /// <summary>
        ///     Calls User repository method IsUserExists().
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsUserExists(string userName)
        {
            bool result = false;

            if (userName != null)
            {
                result = _userRepository.IsUserExists(userName);
            }

            return result;
        }

        /// <summary>
        ///     Calls User repository method FindUser().
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User FindUser(string userName)
        {
            return _userRepository.FindUser(userName);
        }

        /// <summary>
        ///     Calls User repository method FindUserAsync().
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<User> FindUserAsync(string userName, string password)
        {
            return _userRepository.FindUserAsync(userName, password);
        }

        /// <summary>
        ///     Calls User repository method RegisterUser().
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(User user)
        {
            return await _userRepository.RegisterUser(user);
        }

        /// <summary>
        ///       Calls User repository method UpdateUser().
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdatePassword(User user)
        {
            return await _userRepository.UpdatePassword(user);
        }

        public bool CanLogin(int id)
        {
            return _userRepository.CanLogin(id);
        }
    }
}
