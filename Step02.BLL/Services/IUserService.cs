using Step02.Common;
using Step02.DAL.Entities;
using Step02.DAL.Repositorties;

namespace Step02.BLL.Services;

public interface IUserService
{
    User Login(string username, string password);
    User Register(string username, string password, string confirmPassword);
    User GetById(int id);
    List<User> GetAll();
    User Deactivate(int id);
    User Activate(int id);
    User ChangePassword(int id, string oldPassword, string newPassword);
    User ChangeRole(int id, string newRole);
}


public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    // ============================================================
    // Login
    // ============================================================
    public User Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        var user = _userRepo.GetByUsername(username);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid username or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated. Contact admin.");

        if (!PasswordHasher.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        user.LastLogin = DateTime.Now;
        _userRepo.Update(user);

        return user;
    }


    // ============================================================
    // Register
    // ============================================================
    public User Register(string username, string password, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        if (username.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.");

        if (password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters.");

        if (password != confirmPassword)
            throw new ArgumentException("Passwords do not match.");

        if (_userRepo.UsernameExists(username))
            throw new InvalidOperationException($"Username '{username}' is already taken.");

        var user = new User
        {
            Username = username.Trim(),
            PasswordHash = PasswordHasher.Hash(password),
            Role = "Customer"
        };

        _userRepo.Add(user);

        return user;
    }

    // ============================================================
    // GetById
    // ============================================================
    public User GetById(int id)
    {
        var user = _userRepo.GetById(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        return user;
    }

    // ============================================================
    // Deactivate
    // ============================================================
    public User Deactivate(int id)
    {
        var user = GetById(id);

        if (!user.IsActive)
            throw new InvalidOperationException($"User '{user.Username}' is already deactivated.");

        if (user.Role == "Admin")
            throw new InvalidOperationException("Cannot deactivate admin user.");

        user.IsActive = false;
        _userRepo.Update(user);

        return user;
    }

    // ============================================================
    // Activate
    // ============================================================
    public User Activate(int id)
    {
        var user = GetById(id);

        if (user.IsActive)
            throw new InvalidOperationException($"User '{user.Username}' is already active.");

        user.IsActive = true;
        _userRepo.Update(user);

        return user;
    }


    // ============================================================
    // ChangePassword
    // ============================================================
    public User ChangePassword(int id, string oldPassword, string newPassword)
    {
        var user = GetById(id);

        if (!PasswordHasher.Verify(oldPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect.");

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password is required.");

        if (newPassword.Length < 6)
            throw new ArgumentException("New password must be at least 6 characters.");

        if (PasswordHasher.Verify(newPassword, user.PasswordHash))
            throw new ArgumentException("New password must be different from current password.");

        user.PasswordHash = PasswordHasher.Hash(newPassword);
        _userRepo.Update(user);

        return user;
    }

    // ============================================================
    // Get all users
    // ============================================================
    public List<User> GetAll() => _userRepo.GetAll();


    // ============================================================
    // ChangeRole
    // ============================================================
    public User ChangeRole(int id, string newRole)
    {
        var user = GetById(id);

        user.Role = newRole;
        _userRepo.Update(user);

        return user;
    }
}