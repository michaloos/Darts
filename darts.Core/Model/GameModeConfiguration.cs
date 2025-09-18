using darts.Core.Interface;

namespace darts.Core.Model;

public abstract class GameModeConfiguration : IValidatable
{
    public abstract Guid GameModeId { get; }
    public virtual void Validate()
    {
        // Ewentualna walidacja ogólna
    }

    // Pomocnicza metoda do tworzenia kopii konfiguracji
    public T Clone<T>() where T : GameModeConfiguration
    {
        var json = System.Text.Json.JsonSerializer.Serialize(this);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    // public abstract void Deconstruct(params object[] args);
}