using Project.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel;

class RentViewModel : ViewModelBase
{
    public ICommand RentBtn { get; }

    public RentViewModel()
    {
        RentBtn = new RelayCommand(ExecuteRentCommand);

    }

    private async void ExecuteRentCommand(object obj)
    {
        AllCars.Remove(BaseSelectedCar);
        await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/RentVehicle?Id={BaseSelectedCar.Id}");

        Application.Current.Windows[1].Close();
    }
}