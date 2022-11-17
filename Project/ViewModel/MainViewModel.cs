using Project.Model;
using Project.Repositories;
using System;
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

    private void LoadCurrentUserData()
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
