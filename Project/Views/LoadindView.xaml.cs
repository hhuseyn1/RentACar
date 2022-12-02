using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Project.Views;

public partial class LoadindView : Window
{
    public int Value { get; set; }
    public LoadindView()
    {
        InitializeComponent();
        ThreadSleep();
    }

    private async void ThreadSleep()
    {
        await Task.Delay(3000);
        Visibility = Visibility.Hidden;
    }

    private void MouseEnter(object sender, MouseEventArgs e)
    {
        Logo.Width= 200;
        Logo.Height= 200;
    }

    private void MouseLeave(object sender, MouseEventArgs e)
    {
        Logo.Width = 50;
        Logo.Height = 50;
    }


}
