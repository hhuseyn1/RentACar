using Newtonsoft.Json;
using Project.Model;
using Project.Repositories;
using System;
using System.Net.Http;
using System.Threading;
using System.Windows;

namespace Project.ViewModel;

public class MainViewModel : ViewModelBase
{
	private IUserRepository userRepository;

	private UserAccountModel _currentUserAccount;
	public UserAccountModel CurrentUserAccount
	{
		get { return _currentUserAccount; }
		set { _currentUserAccount = value; OnPropChanged(nameof(CurrentUserAccount)); }
	}

	public MainViewModel()
	{
		userRepository = new UserRepository();
		LoadCurrentUserData();
	}

    private async void LoadCurrentUserData()
    {
		if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseApi"]))
		{
			var jsonString = JsonConvert.DeserializeObject<User>(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetUser?Username={IUserRepository.CurrentUsername}&Password={IUserRepository.CurrentPassword}"));
            if (jsonString != null)
            {
                CurrentUserAccount = new UserAccountModel()
                {
                    Username = jsonString.Username,
                    DisplayName = $"Welcome {jsonString.Name} {jsonString.Lastname}",
                    ProfilePicture = null
                };
            }
            else
            {
                MessageBox.Show("Invalid User");
                Application.Current.Shutdown();
            }
		}
		else
		{
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount = new UserAccountModel()
                {
                    Username = user.Username,
                    DisplayName = $"Welcome {user.Name} {user.Lastname}",
                    ProfilePicture = null
                };
                MessageBox.Show("Test");
            }
            else
            {
                MessageBox.Show("Invalid User");
                Application.Current.Shutdown();
            }
        }
        
    }
}
