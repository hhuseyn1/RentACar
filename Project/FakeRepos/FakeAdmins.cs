using Project.Model;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Project.FakeRepos;

class FakeAdmins
{
    List<Admin> Admins = new()
    {
        new Admin("husey1","huseyn1@mail.com","huseyn1"),
        new Admin("husey2","huseyn2@mail.com","huseyn2"),
        new Admin("husey3","huseyn3@mail.com","huseyn3")
    };
}
