using Admin.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

namespace Admin.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware, INotifyPropertyChanged
    {
        DispatcherTimer timer = new DispatcherTimer();

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

        [ObservableProperty]
        private int _counter = 0;

        public async void OnNavigatedTo()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += new EventHandler(timerTick_event);
            timer.Start();
        }

        private void timerTick_event(object? sender, EventArgs e) => GetData();

        private async void GetData()
        {
            Cars = JsonConvert.DeserializeObject<ObservableCollection<Car>>(await new HttpClient().GetStringAsync("http://localhost:8000/GetCars"));
            Visibility = System.Windows.Visibility.Hidden;
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
