using Project.Views;
using System.Threading;
using System.Windows;

namespace Project;

public partial class App : Application
{
    protected void ApplicationStartup(object sender, StartupEventArgs e)
    {
        LoadindView view = new LoadindView();
        view.Show();
        
        var loginView = new LoginView();
        loginView.Show();
         loginView.IsVisibleChanged += (s, ev) =>
         {
            if (loginView.IsVisible == false && loginView.IsLoaded)
            {
             var mainView = new MainView();
             mainView.Show();
             loginView.Close();
             }
        };
        }
}
