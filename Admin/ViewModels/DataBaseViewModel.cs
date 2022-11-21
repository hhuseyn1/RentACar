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

    [ObservableProperty]
    private string _make;
    [ObservableProperty]
    private string _model;
    [ObservableProperty]
    private string _plate;
    [ObservableProperty]
    private string _price;
    [ObservableProperty]
    private string _username;
    [ObservableProperty]
    private string _password;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _lastname;
    [ObservableProperty]
    private string _email;
    [ObservableProperty]
    private bool _isChecked = false;

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
    public void AddUser(object obj)
    {
        if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Lastname) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email))
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox();

            messageBox.ButtonLeftName = "Add";
            messageBox.ButtonRightName = "Nevermind";

            messageBox.ButtonLeftClick += MessageBox_LeftButtonClickAddUser;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            messageBox.Show("CarRental - Admin", "Do you want add user?");
        }
    }

    private async void MessageBox_LeftButtonClickAddUser(object sender, RoutedEventArgs e)
    {
        var result = await new HttpClient().GetStringAsync($"http://localhost:8000/AddUser?Username={Username}&Password={Password}&Name={Name}&Lastname={Lastname}&Email={Email}&IsAdmin={IsChecked}");
        if (result.ToString().Trim('"').TrimEnd('"') == "Username Exist!")
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox();

            messageBox.ButtonLeftName = "Ok";
            messageBox.ButtonRightName = "Ok";

            messageBox.ButtonLeftClick += MessageBox_RightButtonClick;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            messageBox.Show("CarRental - Admin", "Username Exist!");
        } else
            User.AllUsers.Add(new User() { Username = Username, Password = Password, Name = Name, Lastname = Lastname, Email = Email });
        
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }

    [RelayCommand]
    public void AddVehicle(object obj)
    {
        if (!string.IsNullOrWhiteSpace(Make) && !string.IsNullOrWhiteSpace(Model) && !string.IsNullOrWhiteSpace(Plate) && !string.IsNullOrWhiteSpace(Price))
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox();

            messageBox.ButtonLeftName = "Add";
            messageBox.ButtonRightName = "Nevermind";

            messageBox.ButtonLeftClick += MessageBox_LeftButtonClickAddVehicle;
            messageBox.ButtonRightClick += MessageBox_RightButtonClick;

            messageBox.Show("CarRental - Admin", "Do you want add car?");
        }
    }

    private async void MessageBox_LeftButtonClickAddVehicle(object sender, RoutedEventArgs e)
    {
        await new HttpClient().GetStringAsync($"http://localhost:8000/AddVehicle?Make={Make}&Model={Model}&Plate={Plate}&Price={Price}");
        Car.AllCars.Add(new Car(){ Make = Make, Model = Model, Plate = Plate, Price = Price });
        (sender as Wpf.Ui.Controls.MessageBox)?.Close();
    }

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
