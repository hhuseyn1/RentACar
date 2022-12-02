using Project.Views;
using System.Windows;

namespace Project;

public partial class App : Application
{
    protected void ApplicationStartup(object sender, StartupEventArgs e)
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
}
