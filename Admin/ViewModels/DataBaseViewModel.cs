using Admin.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Admin.ViewModels;

public partial class DataBaseViewModel : ObservableObject, INavigationAware, INotifyPropertyChanged
{
    //[ObservablePropertyAttribute]
    //public string _selectedUser;

    private int _selectedIndex;
    public int SelectedIndex
    {
        get { return _selectedIndex; }
        set { _selectedIndex = value; NotifyPropertyChanged(); }
    }

    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users
    {
        get { return _users; }
        set { _users = value; NotifyPropertyChanged(); }
    }

    private ObservableCollection<Car> _cars;
    public ObservableCollection<Car> Cars
    {
        get { return _cars; }
        set { _cars = value; NotifyPropertyChanged(); }
    }

    public Visibility _Visibility;
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
        if (Car.AllCars is not null && User.AllUsers is not null && Car.AllCars.Count > 0 && User.AllUsers.Count > 0)
        {
            Cars = Car.AllCars;
            Users = User.AllUsers;
            Visibility = System.Windows.Visibility.Hidden;
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    [RelayCommand]
    public void RemoveUser()
    {
        var messageBox = new Wpf.Ui.Controls.MessageBox();

        messageBox.ButtonLeftName = SelectedIndex.ToString();
        messageBox.ButtonRightName = "Just close me";


        messageBox.Show("Something weird", "May happen");
    }
}
