namespace Step02.DAL.Entities;


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }

    public override string ToString()
        => $"Id: {Id} | {Username} | Role: {Role} | Active: {IsActive} | Created: {CreatedAt:yyyy-MM-dd}";
}