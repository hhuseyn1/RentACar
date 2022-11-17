
using Project.Model;
using System.ComponentModel;

namespace Project.ViewModel;
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropChanged(string propName)
    {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
