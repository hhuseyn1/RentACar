﻿using Newtonsoft.Json;
using Project.Model;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

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

    private List<Car> _cars;
    public List<Car> Cars
    {
        get { return _cars; }
        set { _cars = value; OnPropChanged(nameof(Cars)); }
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
                Cars = JsonConvert.DeserializeObject<List<Car>>(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetCars"));
                // CurrentUserAccount = new UserAccountModel()
                // {
                //     Username = jsonString.Username,
                //     DisplayName = $"Welcome {jsonString.Name} {jsonString.Lastname}",
                //     ProfilePicture = null
                // };
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
}