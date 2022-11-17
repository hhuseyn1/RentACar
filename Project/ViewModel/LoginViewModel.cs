using System;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel;

public class LoginViewModel : ViewModelBase
{

    private string _userName;
    private string _password;
    private string _errorMessage;
    private bool _isviewVisible = true;

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

    private void ExecuteLoginCommand(object obj)
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
}
