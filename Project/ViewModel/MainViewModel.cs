using Newtonsoft.Json;
using Project.Model;
using Project.Repositories;
using Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel;

public class MainViewModel : ViewModelBase
{
    public Visibility _Visibility;
    public Visibility Visibility
    {
        get { return _Visibility; }
        set { _Visibility = value; OnPropChanged(nameof(Visibility)); }
    }

    private int _CurrentPage = 1;
    public int CurrentPage
    {
        get { return _CurrentPage; }
        set { _CurrentPage = value; OnPropChanged(nameof(CurrentPage)); }
    }

    public ICommand PrevPage { get; }
    public ICommand NextPage { get; }
    public ICommand CarRentScreenClick { get; }
    private IUserRepository userRepository;

    private string _currentUserName;
    public string CurrentUserName
    {
        get { return _currentUserName; }
        set { _currentUserName = value; OnPropChanged(nameof(CurrentUserName)); }
    }

	private UserAccountModel _currentUserAccount;
	public UserAccountModel CurrentUserAccount
	{
		get { return _currentUserAccount; }
        set { _currentUserAccount = value; OnPropChanged(nameof(CurrentUserAccount)); }
	}

    

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
        CarRentScreenClick = new RelayCommand(ExecuteCarRentScreenClick);
        userRepository = new UserRepository();
        //Cars.Add(new Car());
		LoadCurrentUserData();
	}

    private async void LoadCurrentUserData()
    {
		if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseApi"]))
		{
			var jsonString = JsonConvert.DeserializeObject<User>(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetUser?Username={IUserRepository.CurrentUsername}&Password={IUserRepository.CurrentPassword}"));
            if (jsonString != null)
            {
                CurrentUserName = IUserRepository.CurrentUsername;
                AllCars = JsonConvert.DeserializeObject<List<Car>>(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetCars"));
                foreach (var Car in AllCars)
                {
                    if (Car.isRented == 0)
                        if (Car.Page == CurrentPage)
                            Cars.Add(Car);
                }

                Visibility = Visibility.Hidden;
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


    public void ExecuteCarRentScreenClick(object obj)
    {
        int index = int.Parse(obj.ToString()); 
        if (CurrentPage != 1)
            index = index + (CurrentPage * 10) - 10;

        BaseSelectedCar = AllCars[index];
        RentView rentView = new RentView(AllCars[index]);
        rentView.ShowDialog();
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
        foreach (var Car in AllCars)
        {
            if (Car.isRented == 0)
                if (Car.Page == CurrentPage)
                    Cars.Add(Car);
        }
    }
}
