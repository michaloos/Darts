using darts.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace darts.Core.Services;

/// <summary>
/// Klasa pomocnicza do obsługi konfiguracji trybów gry bez zapisywania w bazie danych
/// </summary>
public static class GameConfigurationHelper
{
    // Słownik przechowujący domyślne konfiguracje, które mogą być zmodyfikowane w czasie działania aplikacji
    // Klucz to ID trybu gry, wartość to obiekt konfiguracji
    private static readonly Dictionary<Guid, GameModeConfiguration> _customConfigurations = new();

    /// <summary>
    /// Pobiera domyślną konfigurację dla danego trybu gry
    /// </summary>
    public static GameModeConfiguration GetDefaultConfiguration(Guid gameModeId)
    {
        // Sprawdź, czy istnieje niestandardowa konfiguracja
        if (_customConfigurations.TryGetValue(gameModeId, out var customConfig))
            return customConfig;

        // Pobierz tryb gry i zwróć standardową konfigurację
        var gameMode = GameModes.Modes.FirstOrDefault(m => m.Id == gameModeId);
        if (gameMode == null)
            throw new ArgumentException($"Nie znaleziono trybu gry o ID {gameModeId}");

        return gameMode.CreateDefaultConfiguration();
    }

    /// <summary>
    /// Aktualizuje domyślną konfigurację dla danego trybu gry
    /// </summary>
    public static void UpdateDefaultConfiguration(GameModeConfiguration configuration)
    {
        configuration.Validate(); // Upewnij się, że konfiguracja jest poprawna
        _customConfigurations[configuration.GameModeId] = configuration;
    }

    /// <summary>
    /// Resetuje domyślną konfigurację dla danego trybu gry
    /// </summary>
    public static void ResetDefaultConfiguration(Guid gameModeId)
    {
        if (_customConfigurations.ContainsKey(gameModeId))
            _customConfigurations.Remove(gameModeId);
    }

    /// <summary>
    /// Tworzy konfigurację dla danego trybu gry na podstawie istniejącego obiektu GameMode
    /// </summary>
    public static T CreateConfiguration<T>(GameMode gameMode) where T : GameModeConfiguration
    {
        if (gameMode == null)
            throw new ArgumentNullException(nameof(gameMode));

        // Użyj fabryki do utworzenia konfiguracji
        var config = gameMode.CreateDefaultConfiguration();
        if (config is T typedConfig)
            return typedConfig;

        throw new InvalidOperationException($"Nie można utworzyć konfiguracji typu {typeof(T).Name} dla trybu gry {gameMode.Name}");
    }

    /// <summary>
    /// Waliduje konfigurację i zwraca listę błędów lub pustą listę, jeśli konfiguracja jest poprawna
    /// </summary>
    public static List<string> ValidateConfiguration(GameModeConfiguration configuration)
    {
        var errors = new List<string>();
        try
        {
            configuration.Validate();
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }
        return errors;
    }
}
