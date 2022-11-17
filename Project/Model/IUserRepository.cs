using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Project.Model;

public interface IUserRepository
{
    public static string CurrentUsername { get; set; }
    public static string CurrentPassword { get; set; }

    bool AuthenticateUser(NetworkCredential credential);
    void Add(User userModel);
    void Edit(User userModel);
    void Remove(int id);
    User GetById(int id);
    User GetByUsername(string username);
    IEnumerable<User> GetByAll();
}
