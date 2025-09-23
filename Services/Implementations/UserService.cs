using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(RegisterDto registerDto)
    {
        try
        {
            // Check if username exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("Username already exists");
            }

            // Check if email exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("Email already exists");
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Name = registerDto.Name,
                AddressOffice = registerDto.AddressOffice,
                AddressResidential = registerDto.AddressResidential,
                Designation = registerDto.Designation,
                PhoneOffice = registerDto.PhoneOffice,
                PhoneResidential = registerDto.PhoneResidential,
                Mobile = registerDto.Mobile,
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userResponse = MapToUserResponse(user);
            return ApiResponse<UserResponseDto>.SuccessResponse(userResponse, "User created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error creating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            var expires = DateTime.UtcNow.AddHours(2);

            var loginResponse = new LoginResponseDto
            {
                Token = token,
                Expires = expires,
                User = MapToUserResponse(user)
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(loginResponse, "Login successful");
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginResponseDto>.ErrorResponse($"Error during login: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");
            }

            var userResponse = MapToUserResponse(user);
            return ApiResponse<UserResponseDto>.SuccessResponse(userResponse);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<UserResponseDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            var userResponses = users.Select(MapToUserResponse).ToList();
            return ApiResponse<List<UserResponseDto>>.SuccessResponse(userResponses);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<UserResponseDto>>.ErrorResponse($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserAsync(int id, UserDetailsDto userDetailsDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");
            }

            // Check if email exists for other users
            if (await _context.Users.AnyAsync(u => u.Email == userDetailsDto.Email && u.Id != id))
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("Email already exists");
            }

            user.Email = userDetailsDto.Email;
            user.Phone = userDetailsDto.Phone;
            user.Name = userDetailsDto.Name;
            user.AddressOffice = userDetailsDto.AddressOffice;
            user.AddressResidential = userDetailsDto.AddressResidential;
            user.Designation = userDetailsDto.Designation;
            user.PhoneOffice = userDetailsDto.PhoneOffice;
            user.PhoneResidential = userDetailsDto.PhoneResidential;
            user.Mobile = userDetailsDto.Mobile;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var userResponse = MapToUserResponse(user);
            return ApiResponse<UserResponseDto>.SuccessResponse(userResponse, "User updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error updating user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> UpdateUserRoleAsync(int id, string role)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("User not found");
            }

            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var userResponse = MapToUserResponse(user);
            return ApiResponse<UserResponseDto>.SuccessResponse(userResponse, "User role updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error updating user role: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ApiResponse<bool>.ErrorResponse("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "User deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResponse($"Error deleting user: {ex.Message}");
        }
    }

    public async Task<ApiResponse<UserResponseDto>> SetupAdminAsync(RegisterDto registerDto)
    {
        try
        {
            // Check if admin already exists
            if (await IsAdminExistsAsync())
            {
                return ApiResponse<UserResponseDto>.ErrorResponse("Admin user already exists");
            }

            registerDto.Role = "admin";
            return await CreateUserAsync(registerDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<UserResponseDto>.ErrorResponse($"Error setting up admin: {ex.Message}");
        }
    }

    public async Task<bool> IsAdminExistsAsync()
    {
        return await _context.Users.AnyAsync(u => u.Role == "admin");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtKey!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static UserResponseDto MapToUserResponse(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Name = user.Name,
            AddressOffice = user.AddressOffice,
            AddressResidential = user.AddressResidential,
            Designation = user.Designation,
            PhoneOffice = user.PhoneOffice,
            PhoneResidential = user.PhoneResidential,
            Mobile = user.Mobile,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}