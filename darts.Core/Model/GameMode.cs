using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace darts.Core.Model;

public class GameMode : INotifyPropertyChanged
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    
    public required int StartingScore { get; set; }
    public required bool RequiresDoubleOut { get; set; }
    public required bool UsesSpecificTargets { get; set; }
    public required bool HasScoringSystem { get; set; }
    public required bool Disabled { get; set; }

    public required bool IsSelected
    {
        get => _isSelected;
        set {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged();
        }
}
    private bool _isSelected;


    [SetsRequiredMembers]
    public GameMode(Guid id, string name, string description, 
        int startingScore = 0, bool requiresDoubleOut = false, bool usesSpecificTargets = false, bool hasScoringSystem = false, bool disabled = true)
    {
        Name = name;
        Description = description;
        StartingScore = startingScore;
        RequiresDoubleOut = requiresDoubleOut;
        UsesSpecificTargets = usesSpecificTargets;
        HasScoringSystem = hasScoringSystem;
        Disabled = disabled;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

public static class GameModes
{
    public static readonly ObservableCollection<GameMode> Modes =
    [
        new (Guid.Parse("9EED0709-B22F-4F78-8DAC-FE84675505A3"), "501 / 301",
            "Gracze zaczynają z 501 lub 301 punktami i muszą dojść do dokładnie 0. W wersji Double Out ostatni rzut musi trafić w podwójne pole.",
            501, true, false, true, false),
        new (Guid.Parse("237587C5-3223-4B1C-AECC-FADA6FCF10DE"), "Cricket",
            "Gracze muszą zamknąć pola 15-20 i bullseye, trafiając je 3 razy. Po zamknięciu pola zdobywa się punkty, dopóki przeciwnik go nie zamknie.",
            0, false, true, true),
        new (Guid.Parse("246ED1BB-D57A-4778-B062-2B7445E356BD"), "Around the Clock",
            "Gracze muszą trafić każdą liczbę od 1 do 20 po kolei. Pierwszy, kto trafi bullseye po 20, wygrywa.", 0,
            false, true, false),
        new (Guid.Parse("98C579D9-A550-416F-88B1-C896C5ED62FA"), "Shanghai",
            "W każdej rundzie można trafiać tylko wyznaczony numer (np. w 1. rundzie tylko ‘1’). Wygrywa ten, kto pierwszy zdobędzie Shanghai (pojedynczy, podwójny i potrójny jednego numeru w jednej rundzie).",
            0, false, true, true),
        new (Guid.Parse("3D58E5C8-EFE6-4DC4-8BA9-7DBC9E8D718D"), "Killer",
            "Każdy gracz losuje swój numer i musi go trafić 5 razy, by stać się Killerem. Następnie atakuje przeciwników, trafiając ich numery. Ostatni ocalały wygrywa.",
            0, false, true, false),
        new (Guid.Parse("66B95992-2771-4BD8-8CB6-EE36FD0507E5"), "High Score (170)",
            "Celem jest zdobycie jak największej liczby punktów w określonej liczbie rund. Wygrywa ten, kto osiągnie najwyższy wynik.",
            170, false, false, true),
        new (Guid.Parse("B23C5C17-230D-4ECF-986B-0D01B8E2F678"), "Legs (Sety)",
            "Gracze rywalizują na zasadzie wygrania określonej liczby legów (np. Best of 5 – kto pierwszy zdobędzie 3 legi, wygrywa).",
            0, false, false, true),
        new (Guid.Parse("6088201C-6649-4904-B24C-CEFBEC355F24"), "Halve-It",
            "Gracze muszą trafić wyznaczone liczby. Nietrafienie powoduje podział punktów na pół. Wygrywa gracz z najwyższym wynikiem.",
            0, false, true, true),
        new (Guid.Parse("59E102BA-719D-4A1A-8271-BE5A637792C1"), "Gotcha!",
            "Każdy gracz ma celny wynik do osiągnięcia (np. 250). Kto pierwszy dokładnie trafi tę liczbę, wygrywa. Przekroczenie wyniku cofa do poprzedniego stanu.",
            250, false, false, true),
        new (Guid.Parse("97544A51-2217-449A-BA61-D83BA90664CD"), "Bob’s 27",
            "Gracze zaczynają z 27 punktami i rzucają w pola podwójne. Nietrafienie oznacza odjęcie wartości pola od wyniku. Wygrywa ten, kto po przejściu przez wszystkie podwójne ma najwyższy wynik.",
            27, false, true, true)
    ];
}

