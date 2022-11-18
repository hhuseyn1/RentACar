using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows;
using Wpf.Ui.Common.Interfaces;
using Admin.Models;
using System;
using Newtonsoft.Json;
using System.Net.Http;

namespace Admin.ViewModels;

public class UsersViewModel : ObservableObject, INavigationAware, INotifyPropertyChanged
{
    DispatcherTimer timer = new DispatcherTimer();
    public ObservableCollection<User> Users
    {
        get { return User.AllUsers; }
        set { User.AllUsers = value; NotifyPropertyChanged(); }
    }

    public Visibility _Visibility;

    public UsersViewModel()
    {
        timer.Interval = new TimeSpan(0, 0, 0, 1);
        timer.Tick += new EventHandler(timerTick_event);
        timer.Start();
    }

    public Visibility Visibility
    {
        get { return _Visibility; }
        set { _Visibility = value; NotifyPropertyChanged(); }
    }

    public void OnNavigatedFrom()
    {
        
    }

    public void OnNavigatedTo()
    {

    }

    private void timerTick_event(object? sender, EventArgs e) => GetData();

    private async void GetData()
    {
        Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(await new HttpClient().GetStringAsync("http://localhost:8000/GetUsers"));
        Visibility = System.Windows.Visibility.Hidden;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
