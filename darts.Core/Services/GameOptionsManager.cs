using darts.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace darts.Core.Services;

public class GameOptionsManager
{
    // Pamięć podręczna ostatnio używanych konfiguracji dla szybkiego dostępu
    private readonly Dictionary<Guid, ObservableCollection<GameModeConfiguration>> _recentConfigurations = new();
    private const int MaxRecentConfigurations = 5; // Maksymalna liczba zapamiętanych konfiguracji dla każdego trybu

    /// <summary>
    /// Pobiera domyślną konfigurację dla wybranego trybu gry
    /// </summary>
    public GameModeConfiguration GetConfiguration(Guid gameModeId)
    {
        return GameConfigurationHelper.GetDefaultConfiguration(gameModeId);
    }

    /// <summary>
    /// Zapisuje konfigurację jako ostatnio używaną (bez zapisu do bazy danych)
    /// </summary>
    public void SaveRecentConfiguration(GameModeConfiguration configuration)
    {
        configuration.Validate();
        var gameModeId = configuration.GameModeId;

        if (!_recentConfigurations.TryGetValue(gameModeId, out var configs))
        {
            configs = new ObservableCollection<GameModeConfiguration>();
            _recentConfigurations[gameModeId] = configs;
        }

        // Sprawdź, czy już istnieje podobna konfiguracja
        var similar = configs.FirstOrDefault(c => IsConfigurationSimilar(c, configuration));
        if (similar != null)
            configs.Remove(similar);

        // Dodaj na początek listy
        configs.Insert(0, configuration);

        // Ogranicz liczbę zapamiętanych konfiguracji
        while (configs.Count > MaxRecentConfigurations)
            configs.RemoveAt(configs.Count - 1);
    }

    /// <summary>
    /// Sprawdza, czy dwie konfiguracje są podobne (mają takie same wartości)
    /// </summary>
    private bool IsConfigurationSimilar(GameModeConfiguration config1, GameModeConfiguration config2)
    {
        if (config1.GetType() != config2.GetType())
            return false;

        // Serializuj do JSON i porównaj
        var json1 = System.Text.Json.JsonSerializer.Serialize(config1);
        var json2 = System.Text.Json.JsonSerializer.Serialize(config2);

        return json1 == json2;
    }

    /// <summary>
    /// Pobiera listę ostatnio używanych konfiguracji dla danego trybu gry
    /// </summary>
    public ObservableCollection<GameModeConfiguration> GetRecentConfigurations(Guid gameModeId)
    {
        if (!_recentConfigurations.TryGetValue(gameModeId, out var configs))
            return new ObservableCollection<GameModeConfiguration>();

        return configs;
    }

    /// <summary>
    /// Ustawia konfigurację jako domyślną dla danego trybu gry
    /// </summary>
    public void SetAsDefault(GameModeConfiguration configuration)
    {
        GameConfigurationHelper.UpdateDefaultConfiguration(configuration);
    }

    /// <summary>
    /// Resetuje domyślną konfigurację dla danego trybu gry do wartości fabrycznych
    /// </summary>
    public void ResetToFactoryDefaults(Guid gameModeId)
    {
        GameConfigurationHelper.ResetDefaultConfiguration(gameModeId);

        // Opcjonalnie możemy też wyczyścić ostatnio używane konfiguracje
        if (_recentConfigurations.ContainsKey(gameModeId))
            _recentConfigurations.Remove(gameModeId);
    }
}
