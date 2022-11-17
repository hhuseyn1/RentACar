using System.Collections.ObjectModel;

namespace Admin.Models;

public class User
{
    public static ObservableCollection<User> AllUsers { get; set; }
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
}
