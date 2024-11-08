using server.Application.DTOs;
using server.Core.Entities;
using server.Core.Interfaces;

namespace server.Application.UseCases
{
    public class CreateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public CreateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Execute(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> Execute(AddUserRequest addUserRequest)
        {
            if (addUserRequest == null)
            {
                throw new ArgumentNullException(nameof(addUserRequest));
            }

            if (string.IsNullOrEmpty(addUserRequest.UserId) || string.IsNullOrEmpty(addUserRequest.Email))
            {
                throw new ArgumentException("UserId and Email are required.");
            }

            var newUser = new User
            {
                UserId = addUserRequest.UserId,
                FirstName = addUserRequest.FirstName,
                LastName = addUserRequest.LastName,
                Email = addUserRequest.Email,
                Phone = addUserRequest.Phone,
                Username = addUserRequest.Username,
                Password = addUserRequest.Password,
                RoleId = addUserRequest.RoleId,
                Role = new Role
                {
                    RoleId = addUserRequest.RoleId,
                    RoleName = addUserRequest.RoleName
                },
                Permissions = addUserRequest.Permissions.Select(permission => new Permission
                {
                    PermissionId = permission.PermissionId,
                    PermissionName = permission.PermissionName,
                    UserId = addUserRequest.UserId
                }).ToList()
            };

            return await _userRepository.AddAsync(newUser);
        }

    }
}
