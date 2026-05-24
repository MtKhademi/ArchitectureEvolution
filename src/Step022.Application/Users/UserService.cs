using Step02.Common;

namespace Step022.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }


    // ============================================================
    // Register
    // ============================================================
    public User Register(string username, string password)
    {
        // ⭐ Entity خودش اعتبارسنجی می‌کنه
        var passwordHash = PasswordHasher.Hash(password);
        var user = User.Register(username, passwordHash);

        // ⭐ Application فقط چک‌های دیتابیسی رو انجام می‌ده
        if (_userRepo.UsernameExists(username))
            throw new InvalidOperationException($"Username '{username}' is already taken.");

        _userRepo.Add(user);

        return user;
    }


    // ============================================================
    // Login
    // ============================================================
    public User Login(string username, string password)
    {
        //TODO -> _userRepo.GetByUsernameOrThrow
        var user = _userRepo.GetByUsername(username)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        // ⭐ Entity خودش اعتبارسنجی می‌کنه
        user.Login(PasswordHasher.Hash(password));

        _userRepo.Update(user);

        return user;
    }


    // ============================================================
    // User Management (Admin)
    // ============================================================
    public List<User> GetAllUsers()
    {
        return _userRepo.GetAll();
    }

    public User GetUser(int id)
    {
        var user = _userRepo.GetById(id)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        return user;
    }

    public User DeactivateUser(int id)
    {
        var user = GetUser(id);

        // ⭐ Entity خودش اعتبارسنجی می‌کنه
        user.Deactivate();
        _userRepo.Update(user);

        return user;
    }

    public User ActivateUser(int id)
    {
        var user = GetUser(id);
        user.Activate();
        _userRepo.Update(user);

        return user;
    }

    public void ChangePassword(int userId, string oldPassword, string newPassword)
    {
        var user = GetUser(userId);

        user.ChangePassword(
            PasswordHasher.Hash(oldPassword),
            PasswordHasher.Hash(newPassword)
        );

        _userRepo.Update(user);
    }

}
