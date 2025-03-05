namespace RuiSantos.AutoFill.Web.ViewModels.Commons;

internal static class RegisterServiceExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services)
    {
        var viewModelTypes = typeof(RegisterServiceAttribute).Assembly.GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(RegisterServiceAttribute), true).Any());

        foreach (var viewModelType in viewModelTypes)
        {
            var attribute = 
                (RegisterServiceAttribute)viewModelType.GetCustomAttributes(typeof(RegisterServiceAttribute), true)
                    .First();
            
            switch (attribute.Lifetime)
            {
                case Lifetime.Scoped:
                    services.AddScoped(viewModelType);
                    break;
                
                case Lifetime.Transient:
                    services.AddTransient(viewModelType);
                    break;
                
                case Lifetime.Singleton:
                    services.AddSingleton(viewModelType);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return services;
    }
}