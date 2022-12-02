
using Project.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Project.ViewModel;
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public static List<Car> AllCars;
    public static Car BaseSelectedCar { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropChanged(string propName)
    {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
