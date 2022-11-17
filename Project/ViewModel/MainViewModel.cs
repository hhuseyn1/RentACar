using Newtonsoft.Json;
using Project.Model;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Project.ViewModel;

public class MainViewModel : ViewModelBase
{
    private int _CurrentPage = 1;
    public int CurrentPage
    {
        get { return _CurrentPage; }
        set { _CurrentPage = value; OnPropChanged(nameof(CurrentPage)); }
    }


    public ICommand PrevPage { get; }
    public ICommand NextPage { get; }
    private IUserRepository userRepository;

	private UserAccountModel _currentUserAccount;
	public UserAccountModel CurrentUserAccount
	{
		get { return _currentUserAccount; }
        set { _currentUserAccount = value; OnPropChanged(nameof(CurrentUserAccount)); }
	}

    private List<Car> AllCars;

    private ObservableCollection<Car> _cars;
    public ObservableCollection<Car> Cars
    {
        get { return _cars; }
        set { _cars = value; OnPropChanged(nameof(Cars)); }
    }

    public MainViewModel()
	{
        _cars = new ObservableCollection<Car>();
        PrevPage = new RelayCommand(ExecutePrevCommand, CanExecutePrecCommand);
        NextPage = new RelayCommand(ExecuteNextCommand, CanExecuteNextCommand);
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
                AllCars = JsonConvert.DeserializeObject<List<Car>>(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetCars"));
                foreach (var Car in AllCars)
                {
                    if (Car.Page == CurrentPage)
                        Cars.Add(Car);
                }
                Cars = Cars;
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
            }
            else
            {
                MessageBox.Show("Invalid User");
                Application.Current.Shutdown();
            }
        }
        
    }

    public void ExecutePrevCommand(object obj) => UpdatePage(true);
    public void ExecuteNextCommand(object obj) => UpdatePage(false);
    public bool CanExecutePrecCommand(object obj) => (CurrentPage - 1 > 0);
    public bool CanExecuteNextCommand(object obj) => (AllCars is not null);

    private void UpdatePage(bool Prev)
    {
        if (Prev)
            CurrentPage--;
        else
            CurrentPage++;

        Cars.Clear();
        if (AllCars.Count > 0) 
        {
            foreach (var Car in AllCars)
            {
                if (Car.Page == CurrentPage)
                    Cars.Add(Car);
            }
        }
        Cars = Cars;

    }
}
