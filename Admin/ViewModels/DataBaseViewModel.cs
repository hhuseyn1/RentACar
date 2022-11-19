using Admin.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace Admin.ViewModels;

public partial class DataBaseViewModel : ObservableObject, INavigationAware, INotifyPropertyChanged
{
    private int SelectedIndex;
    private bool IsSelectedItemVehicle = false;

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
    public void RemoveUser(object obj)
    {
        if (obj is not null)
        {
            int Index = int.Parse(obj.ToString());
            var messageBox = new Wpf.Ui.Controls.MessageBox();

            messageBox.ButtonLeftName = "Delete";
            messageBox.ButtonRightName = "Nevermind";

            messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            SelectedIndex = Index;

            messageBox.Show("CarRental - Admin", $"Do you want delete User: {User.AllUsers[Index].Username}");
        }    
    }

    [RelayCommand]
    public void RemoveVehicle(object obj)
    {
        if (obj is not null)
        {
            int Index = int.Parse(obj.ToString());
            var messageBox = new Wpf.Ui.Controls.MessageBox();

            messageBox.ButtonLeftName = "Delete";
            messageBox.ButtonRightName = "Nevermind";

            messageBox.ButtonLeftClick += MessageBox_LeftButtonClick;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            SelectedIndex = Index;
            IsSelectedItemVehicle = true;

            messageBox.Show("CarRental - Admin", $"Do you want delete Vehicle: {Car.AllCars[Index].Plate}");
        }
    }

    private async void MessageBox_LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
    {
        if (IsSelectedItemVehicle)
        {
            await new HttpClient().GetStringAsync($"http://localhost:8000/DeleteVehicle?Id={SelectedIndex + 1}");
            Car.AllCars.Remove(Car.AllCars[SelectedIndex]);
            IsSelectedItemVehicle = false;
        }
        else
        {
            await new HttpClient().GetStringAsync($"http://localhost:8000/DeleteUser?Username={User.AllUsers[SelectedIndex].Username}");
            User.AllUsers.Remove(User.AllUsers[SelectedIndex]);
        }
        Cars = Car.AllCars;
        Users = User.AllUsers;
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }
    private void MessageBox_RightButtonClick(object sender, System.Windows.RoutedEventArgs e) => (sender as Wpf.Ui.Controls.MessageBox)?.Close();
}
