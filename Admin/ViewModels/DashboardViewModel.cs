using Admin.Models;
using Admin.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Interfaces;

namespace Admin.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware, INotifyPropertyChanged
{
    public ObservableCollection<Car> Cars
    {
        get { return Car.AllCars; }
        set { Car.AllCars = value; NotifyPropertyChanged(); }
    }

    public Visibility _Visibility;
    public Visibility Visibility
    {
        get { return _Visibility; }
        set { _Visibility = value; NotifyPropertyChanged(); }
    }

    [ObservableProperty]
    private int _counter = 0;

    public DashboardViewModel()
    {
        GetData();
    }

    public async void OnNavigatedTo()
    {
        
    }

    public void OnNavigatedFrom()
    {

    }

    private async void GetData()
    {
        Cars = JsonConvert.DeserializeObject<ObservableCollection<Car>>(await new HttpClient().GetStringAsync("http://localhost:8000/GetCars"));
        Visibility = System.Windows.Visibility.Hidden;
    } 

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
