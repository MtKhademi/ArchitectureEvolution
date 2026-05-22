using Step02.DAL.Entities;

namespace Step02.DAL.Repositorties;

public interface IUserRepository
{
    void Add(User user);
    void Update(User user);
    User GetById(int id);
    User GetByUsername(string username);
    List<User> GetAll();
    bool UsernameExists(string username);
}
