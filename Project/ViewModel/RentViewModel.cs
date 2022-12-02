using Project.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel;

class RentViewModel : ViewModelBase
{
    public ICommand RentBtn { get; }

    public RentViewModel()
    {
        RentBtn = new RelayCommand(ExecuteRentCommand);

    }

    private void ExecuteRentCommand(object obj)
    {
        MessageBox.Show("Test");
    }
}