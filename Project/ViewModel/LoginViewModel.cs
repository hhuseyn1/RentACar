using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Model;
using Project.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Project.ViewModel;

public class LoginViewModel : ViewModelBase
{

    private string _userName;
    private string _password;
    private string _errorMessage;
    private bool _isviewVisible = true;

    private IUserRepository userRepository;

    public string UserName { get => _userName; set { _userName = value; OnPropChanged(nameof(UserName)); } }
    public string Password { get => _password; set { _password = value; OnPropChanged(nameof(Password)); } }
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropChanged(nameof(ErrorMessage)); } }
    public bool IsVisible { get => _isviewVisible; set { _isviewVisible = value; OnPropChanged(nameof(IsVisible)); } }

    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExcetueLoginCommand);
        RecoverPasswordCommand = new RelayCommand(p => ExecuteRecoverPasswordCommand("", ""));
    }

    private void ExecuteRecoverPasswordCommand(string username, string email)
    {
        throw new NotImplementedException();
    }

    private bool CanExcetueLoginCommand(object obj)
    {
        bool validData = true;
        if (string.IsNullOrWhiteSpace(UserName) || UserName.Length < 8 || Password == null || Password.Length < 8)
            validData = false;

        return validData;

    }

    private async void ExecuteLoginCommand(object obj)
    {
        var isValidUser = false;
        if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseApi"]))
        {
            var jsonString = JsonConvert.DeserializeObject(await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/GetUser?Username={UserName}&Password={Password}"));
            if (jsonString.ToString() != "[]") 
            {
                IUserRepository.CurrentUsername = (jsonString as JObject)["Username"].ToString();
                IUserRepository.CurrentPassword = (jsonString as JObject)["Password"].ToString();
                //userRepository.CurrentUsername = (jsonString as JObject)["Username"].ToString();
                isValidUser = true;
            }
        }
        else
            isValidUser = userRepository.AuthenticateUser(new NetworkCredential(UserName, Password));

        if (isValidUser)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
            IsVisible = false;
        }
        else
            ErrorMessage = "* Invalid username or password";
    }
}
