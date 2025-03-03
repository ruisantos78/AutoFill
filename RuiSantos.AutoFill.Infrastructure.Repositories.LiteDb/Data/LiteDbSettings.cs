namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Data;

/// <summary>
/// Represents the settings required to configure a LiteDB connection.
/// </summary>
internal class LiteDbSettings
{
    /// <summary>
    /// Gets the connection string used to connect to the LiteDB database.
    /// </summary>
    public required string ConnectionString { get; init; }
}