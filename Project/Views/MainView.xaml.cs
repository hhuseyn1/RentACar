using Project.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Project.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }

    private void CarRentScreenClick(object sender, MouseButtonEventArgs e)
    {
        //Current car must send with ctor
        RentView rent = new(new Car() { Make="Bmw",Model="M5",Price="450",Source= "/Images/DefaultCarImage.jpg" });//Example
        rent.Show();
    }

    private void Minimize_Close_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn)
        {
            if (btn.Name == "Minimize")
            {
                this.WindowState = WindowState.Minimized;
            }
            else if (btn.Name == "Close")
            {
                Application.Current.Shutdown();
            }
        }
    }

    private void CarsScreenBtn(object sender, RoutedEventArgs e)
    {
        WelcomeScreen.Visibility = Visibility.Hidden;
        Settings.Visibility = Visibility.Hidden;
        CarsScreen.Visibility = Visibility.Hidden;
        Account.Visibility = Visibility.Hidden;

        if (sender is Button btn)
        {
            if (btn.Name == "Carsbtn")
            CarsScreen.Visibility= Visibility.Visible;
            else if (btn.Name == "Accountbtn")
            Account.Visibility= Visibility.Visible;
            else if (btn.Name == "Settingsbtn")
            Settings.Visibility= Visibility.Visible;
        }
    }

    private void Drag(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == e.LeftButton)
            this.DragMove();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == e.LeftButton)
            this.DragMove();
    }
}
