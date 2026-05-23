namespace Step017.Domain.Repositories;

public interface IUserRepository
{
    User GetById(int id);
    User GetByUsername(string username);
    List<User> GetAll();
    void Add(User user);
    void Update(User user);
    bool UsernameExists(string username);
}
