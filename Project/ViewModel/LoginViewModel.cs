using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;

namespace Project.ViewModel;

public class LoginViewModel : ViewModelBase
{
    Random rnd = new Random();
    // Register

    private int SecretCode;

    private string _username;
    private string _passWord;
    private string _RepassWord;
    private string _email;
    private string _Name;
    private string _lastname;
    private string _verification;

    public string RegisterUsername { get => _username; set { _username = value; OnPropChanged(nameof(RegisterUsername)); } }
    public string RegisterPassword { get => _passWord; set { _passWord = value; OnPropChanged(nameof(RegisterPassword)); } }
    public string RegisterRePassword { get => _RepassWord; set { _RepassWord = value; OnPropChanged(nameof(RegisterRePassword)); } }
    public string RegisterEmail { get => _email; set { _email = value; OnPropChanged(nameof(RegisterEmail)); } }
    public string RegisterName { get => _Name; set { _Name = value; OnPropChanged(nameof(RegisterName)); } }
    public string RegisterLastname { get => _lastname; set { _lastname = value; OnPropChanged(nameof(RegisterLastname)); } }
    public string RegisterVerification { get => _verification; set { _verification = value; OnPropChanged(nameof(RegisterVerification)); } }

    // Login
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
    public ICommand RegisterCommand { get; }
    public ICommand GetVerification { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExcetueLoginCommand);
        RegisterCommand = new RelayCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
        GetVerification = new RelayCommand(ExecuteVerificationCommand, CanExecuteVerificationCommand);
        RecoverPasswordCommand = new RelayCommand(p => ExecuteRecoverPasswordCommand("", ""));
    }

    private void ExecuteRecoverPasswordCommand(string username, string email)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteVerificationCommand(object obj)
    {
        if (!string.IsNullOrWhiteSpace(RegisterEmail))
        {
            try
            {
                MailAddress m = new MailAddress(RegisterEmail);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        return false;
    }
    
    

    private void ExecuteVerificationCommand(object obj)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("rclash958@gmail.com", "zpnnhpkvkfnrdgnb"),
            EnableSsl = true,
        };
        SecretCode = rnd.Next();
        smtpClient.Send(RegisterEmail, RegisterEmail, "Email Verification Code (OTP)", $"Your Code: {SecretCode}");
    }

    private bool CanExecuteRegisterCommand(object obj)
    {
        if (!string.IsNullOrWhiteSpace(RegisterUsername) && !string.IsNullOrWhiteSpace(RegisterPassword) && !string.IsNullOrWhiteSpace(RegisterRePassword) && !string.IsNullOrWhiteSpace(RegisterEmail) && !string.IsNullOrWhiteSpace(RegisterName) && !string.IsNullOrWhiteSpace(RegisterLastname))
        {
            if (RegisterUsername.Length > 8 && RegisterPassword.Length > 8)
            {
                if (RegisterPassword == RegisterRePassword)
                {
                    if (RegisterVerification == SecretCode.ToString())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private async void ExecuteRegisterCommand(object obj)
    {
        if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseApi"]))
        {
            SecretCode = 012399999;
            var result = await new HttpClient().GetStringAsync($"{System.Configuration.ConfigurationManager.AppSettings["ApiConnectionHost"]}/Register?Firstname={RegisterName}&Lastname={RegisterLastname}&Email={RegisterEmail}&Username={RegisterUsername}&Password={RegisterPassword}");
            if (result.Trim('"').TrimEnd('"') == "Same Username")
                ErrorMessage = result.Trim('"').TrimEnd('"');

        }
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
