using Wpf.Ui.Common.Interfaces;

namespace Admin.Views.Pages;

public partial class UsersPage : INavigableView<ViewModels.UsersViewModel>
{
    public ViewModels.UsersViewModel ViewModel
    {
        get;
    }

    public UsersPage(ViewModels.UsersViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}