namespace RuiSantos.AutoFill.Web.Components.Pages;

public partial class Home
{
    protected override void OnInitialized()
    {
        ViewModel.StateChanged += StateHasChanged;
    }

    public void Dispose()
    {
        ViewModel.StateChanged -= StateHasChanged;
    }
}