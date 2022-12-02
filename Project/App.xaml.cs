using Project.Views;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Project;

public partial class App : Application
{
    protected void ApplicationStartup(object sender, StartupEventArgs e)
    {
        var LoadingView = new LoadindView();
        LoadingView.Show();
        LoadingView.IsVisibleChanged += (s1, ev1) =>
        {
            if (LoadingView.IsVisible == false)
            {
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
            
        };    
    }
}
