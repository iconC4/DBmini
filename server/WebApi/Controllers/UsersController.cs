using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Application.DTOs;
using server.Application.UseCases;
using server.Core.Entities;
using server.Core.Interfaces;
using server.Core.Models;
using server.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly AppDbContext _context;

    public UsersController(CreateUserUseCase createUserUseCase, IUserRepository userRepository, AppDbContext context, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {
        _createUserUseCase = createUserUseCase;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync(); 

        return Ok(new Response<IEnumerable<User>>
        {
            Status = new Status
            {
                Code = "200",
                Description = "Success"
            },
            Data = users
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return NotFound(new
            {
                status = new
                {
                    code = "404",
                    description = "User not found"
                }
            });
        }

        var roleResponse = user.Role != null ? new
        {
            roleId = user.Role.RoleId,
            roleName = user.Role.RoleName
        } : null;

        var permissionsResponse = user.Permissions != null
            ? user.Permissions.Select(p => new
            {
                permissionId = p.PermissionId,
                permissionName = p.PermissionName
            }).ToList() : null;

        var response = new
        {
            status = new
            {
                code = "200",
                description = "Success"
            },
            data = new
            {
                userId = user.UserId,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                phone = user.Phone,
                role = roleResponse,
                username = user.Username,
                permissions = permissionsResponse
            }
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] AddUserRequest userDto)
    {
        if (userDto == null)
        {
            return BadRequest(new Response<string>
            {
                Status = new Status
                {
                    Code = "400",
                    Description = "User data cannot be null."
                },
                Data = "Invalid input"
            });
        }

        if (string.IsNullOrEmpty(userDto.UserId))
        {
            return BadRequest(new Response<string>
            {
                Status = new Status
                {
                    Code = "400",
                    Description = "UserId is required."
                },
                Data = "Invalid UserId"
            });
        }

        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUserByUsername = await _userRepository.GetUserByUsername(userDto.Username);
        if (existingUserByUsername != null)
        {
            return Conflict(new Response<string>
            {
                Status = new Status
                {
                    Code = "409",
                    Description = "Username already exists."
                },
                Data = "Conflict"
            });
        }

        var existingUserByEmail = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUserByEmail != null)
        {
            return Conflict(new Response<string>
            {
                Status = new Status
                {
                    Code = "409",
                    Description = "Email already exists."
                },
                Data = "Conflict"
            });
        }

        // Use the userDto to create the new user
        var addUserRequest = new AddUserRequest
        {
            UserId = userDto.UserId,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            Phone = userDto.Phone,
            Username = userDto.Username,
            Password = userDto.Password,
            RoleId = userDto.RoleId,
            RoleName = userDto.RoleName,
            Permissions = userDto.Permissions
        };

        _ = _createUserUseCase.Execute(addUserRequest);
        return Ok(new Response<AddUserRequest>
        {
            Status = new Status
            {
                Code = "201",
                Description = "User created successfully"
            },
            Data = addUserRequest
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] AddUserRequest userDto)
    {
        // Validate model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Fetch the existing user
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound(new Response<string>
            {
                Status = new Status
                {
                    Code = "404",
                    Description = "User not found."
                },
                Data = "User with the provided ID does not exist."
            });
        }

        // Update user properties
        existingUser.FirstName = userDto.FirstName;
        existingUser.LastName = userDto.LastName;
        existingUser.Email = userDto.Email;
        existingUser.Phone = userDto.Phone;
        existingUser.Username = userDto.Username;
        existingUser.Password = userDto.Password;
        existingUser.Role.RoleId = userDto.RoleId;

        // Update permissions
        var permissions = new List<Permission>();
        foreach (var permissionDto in userDto.Permissions)
        {
            var existingPermission = await _context.Permissions.FindAsync(permissionDto.PermissionId);
            if (existingPermission != null)
            {
                // Update existing permission
                existingPermission.IsReadable = permissionDto.IsReadable;
                existingPermission.IsWritable = permissionDto.IsWritable;
                existingPermission.IsDeletable = permissionDto.IsDeletable;
                existingPermission.UserId = existingUser.UserId;
                permissions.Add(existingPermission);
            }
            else
            {
                var newPermission = new Permission
                {
                    PermissionId = permissionDto.PermissionId,
                    PermissionName = permissionDto.PermissionName,
                    IsReadable = permissionDto.IsReadable,
                    IsWritable = permissionDto.IsWritable,
                    IsDeletable = permissionDto.IsDeletable,
                    UserId = existingUser.UserId
                };
                permissions.Add(newPermission);
            }
        }

        // Clear existing permissions before updating
        existingUser.Permissions.Clear();
        existingUser.Permissions.AddRange(permissions);

        await _userRepository.UpdateAsync(existingUser);

        return Ok(new Response<User>
        {
            Status = new Status
            {
                Code = "200",
                Description = "User updated successfully."
            },
            Data = existingUser
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        // ดึงข้อมูล User จาก UserId
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            return NotFound(new Response<string>
            {
                Status = new Status { Code = "404", Description = "User not found." },
                Data = "User with the provided ID does not exist."
            });
        }

        try
        {
            // ลบ Roles และ Permissions ที่เชื่อมโยงกับ User
            var userRoles = await _roleRepository.GetRolesByUserId(existingUser.UserId);
            foreach (var role in userRoles)
            {
                await _roleRepository.DeleteAsync(role);
            }

            var userPermissions = await _permissionRepository.GetPermissionsByUserId(existingUser.UserId);
            foreach (var permission in userPermissions)
            {
                await _permissionRepository.DeleteAsync(permission);
            }

            // ลบ User
            await _userRepository.DeleteAsync(existingUser);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Ok(new Response<object>
        {
            Status = new Status { Code = "204", Description = "User deleted successfully." },
            Data = new
            {
                result = true,
                message = "User has been deleted successfully."
            }
        });
            // Conflict(new Response<string>
            // {
            //     Status = new Status { Code = "409", Description = "Concurrency conflict occurred." },
            //     Data = "Unable to delete user, please try again."
            // });
        }
        catch (Exception ex)
        {
            // จัดการข้อผิดพลาดทั่วไป
            return StatusCode(500, new Response<string>
            {
                Status = new Status { Code = "500", Description = "Internal server error." },
                Data = ex.Message
            });
        }

        return Ok(new Response<object>
        {
            Status = new Status { Code = "204", Description = "User deleted successfully." },
            Data = new
            {
                result = true,
                message = "User has been deleted successfully."
            }
        });
    }

}

public class CreateUserRequest
{
    public required User[] Data { get; set; }
}