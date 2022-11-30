﻿using Project.Model;
using System.Windows;
using System.Windows.Input;

namespace Project.Views;
public partial class RentView : Window
{
    public Car CurrentCar { get; set; }
    public RentView(Car car)
    {
        InitializeComponent();
        CurrentCar = car;
        CarInfotxtbox.Text = CurrentCar.Make +' '+ CurrentCar.Model;
        Pricetxtbox.Text = CurrentCar.Price;
    }

    private void Cancelbtn(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Drag(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == e.LeftButton)
            this.DragMove();
    }
}
