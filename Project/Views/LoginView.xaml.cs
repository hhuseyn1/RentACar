using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    //private void Button_MouseEnter(object sender, MouseEventArgs e)
    //{
    //   if(sender is Button btn)
    //    {
    //        if (btn.Name == "Minimize")
    //        {
    //            btn.Background = Brushes.LightSlateGray;
    //        }
    //        else if (btn.Name == "Close")
    //        {
    //            btn.Background = Brushes.Red;
    //        }
    //    }
    //}
    


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

    private void EnterKey(object sender, KeyEventArgs e)
    {
        if (UsernameTxtbox.Text.Length>0 && e.Key == Key.Enter)
            e.Handled = true;

    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
     if (e.ButtonState == e.LeftButton)
        this.DragMove();
        
    }

    private void RegisterClick(object sender, RoutedEventArgs e)
    {
        GridLogin.Visibility = Visibility.Hidden;
        GridRegister.Visibility = Visibility.Visible;
    }

    private void GoBack(object sender, MouseButtonEventArgs e)
    {
        GridLogin.Visibility = Visibility.Visible;
        GridRegister.Visibility = Visibility.Hidden;
    }

    private void MouseEnter(object sender, MouseEventArgs e)
    {
        
    }

    //private async void Button_MouseLeave(object sender, MouseEventArgs e)
    //{
    //    if (sender is Button btn)
    //    {
    //        if (btn.Name == "Minimize")
    //        {
    //            await Task.Delay(1000);
    //            btn.Background = Brushes.LightGray;
    //        }
    //        else if (btn.Name == "Close")
    //        {
    //            await Task.Delay(1000);
    //            btn.Background = Brushes.LightGray;
    //        }
    //    }
    //}
}
