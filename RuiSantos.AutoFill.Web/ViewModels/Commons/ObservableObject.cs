namespace RuiSantos.AutoFill.Web.ViewModels.Commons;

public class ObservableObject
{
    // Event to notify UI of state changes
    public event Action? StateChanged;

    protected bool SetProperty<TValue>(ref TValue field, TValue value)
    {
        if (field != null && field.Equals(value))
            return false;
        
        field = value;
        OnStateChanged();
        
        return true;
    }
    
    protected virtual void OnStateChanged()
    {
        StateChanged?.Invoke();
    }
}