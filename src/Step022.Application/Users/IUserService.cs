namespace Step022.Application.Users;

public interface IUserService
{
    User Register(string username, string password);
    User Login(string username, string password);
    User GetUser(int id);
    List<User> GetAllUsers();
    User DeactivateUser(int id);
    User ActivateUser(int id);
    void ChangePassword(int userId, string oldPassword, string newPassword);
}
