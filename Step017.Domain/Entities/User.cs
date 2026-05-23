namespace Step017.Domain.Entities;


public class User
{
    public int Id { get; set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLogin { get; private set; }

    public override string ToString()
        => $"Id: {Id} | {Username} | Role: {Role} | Active: {IsActive} | Created: {CreatedAt:yyyy-MM-dd}";



    private User()
    {
        
    }


    public static User Register(string username, string passwordHash)
    {
   
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required.");

        if (username.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password is required.");


        return new User
        {
            Username = username.Trim(),
            PasswordHash = passwordHash,
            Role = "Customer",
            IsActive = true,
            CreatedAt = DateTime.Now
        };

    }


    public void Login(string passwordHash)
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is deactivated. Contact admin.");

        if (PasswordHash != passwordHash)
            throw new UnauthorizedAccessException("Invalid username or password.");

        LastLogin = DateTime.Now;
    }


    public void ChangePassword(string oldPasswordHash, string newPasswordHash)
    {
        if (PasswordHash != oldPasswordHash)
            throw new UnauthorizedAccessException("Current password is incorrect.");

        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("New password is required.");

        if (oldPasswordHash == newPasswordHash)
            throw new ArgumentException("New password must be different from current password.");

        PasswordHash = newPasswordHash;
    }

    public void ChangeRole(string newRole)
    {
        if (string.IsNullOrWhiteSpace(newRole))
            throw new ArgumentException("Role is required.");

        Role = newRole;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("User is already deactivated.");

        if (Role == "Admin")
            throw new InvalidOperationException("Cannot deactivate admin user.");

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("User is already active.");

        IsActive = true;
    }


    internal void SetId(int id) => Id = id;

}