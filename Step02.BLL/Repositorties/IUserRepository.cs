using Step02.BLL.Entities;

namespace Step02.BLL.Repositorties;

public interface IUserRepository
{
    void Add(User user);
    void Update(User user);
    User GetById(int id);
    User GetByUsername(string username);
    List<User> GetAll();
    bool UsernameExists(string username);
}
