using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;

namespace Project.Views;

public partial class LoadindView : Window
{
    public LoadindView()
    {
        InitializeComponent();
    }

    private void MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Logo.Width= 200;
        Logo.Height= 200;
    }

    private void MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Logo.Width = 50;
        Logo.Height = 50;
    }
}
