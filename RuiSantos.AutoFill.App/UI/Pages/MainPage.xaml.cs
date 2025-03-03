using RuiSantos.AutoFill.App.ViewModels;

namespace RuiSantos.AutoFill.App.UI.Pages;

public partial class MainPage
{
    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;
        
        InitializeComponent();
    }
}