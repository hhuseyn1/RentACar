using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Project.Model;

public interface IUserRepository
{
    bool AuthenticateUser(NetworkCredential credential);
    void Add(User userModel);
    void Edit(User userModel);
    void Remove(int id);
    User GetById(int id);
    User GetByUsername(string username);
    IEnumerable<User> GetByAll();
}
