namespace RuiSantos.AutoFill.Web.ViewModels.Commons;

internal static class ViewModelExtensions
{
    public static IServiceCollection AddAllViewModels(this IServiceCollection services)
    {
        var assembly = typeof(ViewModelExtensions).Assembly;

        var viewModels = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IViewModel).IsAssignableFrom(t))
            .ToList();

        foreach (var viewModel in viewModels)
        {
            services.AddTransient(viewModel);
        }

        return services;
    }
}