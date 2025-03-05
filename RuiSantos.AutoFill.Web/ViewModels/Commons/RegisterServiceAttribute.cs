namespace RuiSantos.AutoFill.Web.ViewModels.Commons;

[AttributeUsage(AttributeTargets.Class)]
internal class RegisterServiceAttribute(Lifetime lifetime = Lifetime.Scoped) : Attribute
{
    public Lifetime Lifetime { get; } = lifetime;
}

internal enum Lifetime
{
    Scoped,
    Transient,
    Singleton
}