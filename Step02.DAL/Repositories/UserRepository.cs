using Step02.BLL.Entities;
using Step02.BLL.Repositorties;
using Step02.DAL.Data;

namespace Step02.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _db;

    public UserRepository(DatabaseContext db)
    {
        _db = db;
    }

    public void Add(User user)
    {
        user.Id = DatabaseContext.UserNextId++;
        user.CreatedAt = DateTime.Now;
        user.IsActive = true;
        _db.Users.Add(user);
    }

    public void Update(User user)
    {
        var index = _db.Users.FindIndex(u => u.Id == user.Id);
        if (index != -1)
            _db.Users[index] = user;
    }

    public User GetById(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    public User GetByUsername(string username)
    {
        return _db.Users.FirstOrDefault(u => u.Username == username);
    }

    public List<User> GetAll()
    {
        return _db.Users.Where(u => u.IsActive).ToList();
    }

    public bool UsernameExists(string username)
    {
        return _db.Users.Any(u => u.Username == username);
    }
}
