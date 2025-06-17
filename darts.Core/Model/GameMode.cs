using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace darts.Core.Model;

public class GameMode
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    
    public required int StartingScore { get; set; }
    public required bool RequiresDoubleOut { get; set; }
    public required bool UsesSpecificTargets { get; set; }
    public required bool HasScoringSystem { get; set; }
    public required bool Disabled { get; set; }
    
    public required Func<GameModeConfiguration> CreateDefaultConfiguration { get; set; }
    public GameModeConfiguration Configuration => CreateDefaultConfiguration();


    [SetsRequiredMembers]
    public GameMode(Guid id, string name, string description, Func<GameModeConfiguration> defaultConfig,
        int startingScore = 0, bool requiresDoubleOut = false, bool usesSpecificTargets = false, bool hasScoringSystem = false, bool disabled = true)
    {
        Name = name;
        Description = description;
        StartingScore = startingScore;
        RequiresDoubleOut = requiresDoubleOut;
        UsesSpecificTargets = usesSpecificTargets;
        HasScoringSystem = hasScoringSystem;
        Disabled = disabled;
        CreateDefaultConfiguration = defaultConfig;
    }
}

public static class GameModes
{
    public static readonly ObservableCollection<GameMode> Modes =
    [
        new (Guid.Parse("9EED0709-B22F-4F78-8DAC-FE84675505A3"), "501 / 301", 
            "Gracze zaczynają z 501 lub 301 punktami i muszą dojść do dokładnie 0. W wersji Double Out ostatni rzut musi trafić w podwójne pole.",
            () => new StandardX01Configuration(), 501, true, false, true, false),
        new (Guid.Parse("237587C5-3223-4B1C-AECC-FADA6FCF10DE"), "Cricket",
            "Gracze muszą zamknąć pola 15-20 i bullseye, trafiając je 3 razy. Po zamknięciu pola zdobywa się punkty, dopóki przeciwnik go nie zamknie.",
            () => new CricketConfiguration(), 0, false, true, true),
        new (Guid.Parse("246ED1BB-D57A-4778-B062-2B7445E356BD"), "Around the Clock",
            "Gracze muszą trafić każdą liczbę od 1 do 20 po kolei. Pierwszy, kto trafi bullseye po 20, wygrywa.", 
            () => new AroundTheClockConfiguration(), 0, false, true, false),
        new (Guid.Parse("98C579D9-A550-416F-88B1-C896C5ED62FA"), "Shanghai",
            "W każdej rundzie można trafiać tylko wyznaczony numer (np. w 1. rundzie tylko ‘1’). Wygrywa ten, kto pierwszy zdobędzie Shanghai (pojedynczy, podwójny i potrójny jednego numeru w jednej rundzie).",
            () => new ShanghaiConfiguration(), 0, false, true, true),
        new (Guid.Parse("3D58E5C8-EFE6-4DC4-8BA9-7DBC9E8D718D"), "Killer",
            "Każdy gracz losuje swój numer i musi go trafić 5 razy, by stać się Killerem. Następnie atakuje przeciwników, trafiając ich numery. Ostatni ocalały wygrywa.",
            () => new KillerConfiguration(), 0, false, true, false),
        new (Guid.Parse("66B95992-2771-4BD8-8CB6-EE36FD0507E5"), "High Score (170)",
            "Celem jest zdobycie jak największej liczby punktów w określonej liczbie rund. Wygrywa ten, kto osiągnie najwyższy wynik.",
            () => new HighScoreConfiguration(), 170, false, false, true),
        new (Guid.Parse("B23C5C17-230D-4ECF-986B-0D01B8E2F678"), "Legs (Sety)",
            "Gracze rywalizują na zasadzie wygrania określonej liczby legów (np. Best of 5 – kto pierwszy zdobędzie 3 legi, wygrywa).",
            () => new LegsConfiguration(), 0, false, false, true),
        new (Guid.Parse("6088201C-6649-4904-B24C-CEFBEC355F24"), "Halve-It",
            "Gracze muszą trafić wyznaczone liczby. Nietrafienie powoduje podział punktów na pół. Wygrywa gracz z najwyższym wynikiem.",
            () => new HalveItConfiguration(), 0, false, true, true),
        new (Guid.Parse("59E102BA-719D-4A1A-8271-BE5A637792C1"), "Gotcha!",
            "Każdy gracz ma celny wynik do osiągnięcia (np. 250). Kto pierwszy dokładnie trafi tę liczbę, wygrywa. Przekroczenie wyniku cofa do poprzedniego stanu.",
            () => new GotchaConfiguration(), 250, false, false, true),
        new (Guid.Parse("97544A51-2217-449A-BA61-D83BA90664CD"), "Bob’s 27",
            "Gracze zaczynają z 27 punktami i rzucają w pola podwójne. Nietrafienie oznacza odjęcie wartości pola od wyniku. Wygrywa ten, kto po przejściu przez wszystkie podwójne ma najwyższy wynik.",
            () => new Bob27Configuration(), 27, false, true, true)
    ];
}

public interface IValidatable
{
    void Validate();
}

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
}

public class StandardX01Configuration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("9EED0709-B22F-4F78-8DAC-FE84675505A3");
    public int StartingScore { get; set; } = 501;
    public bool DoubleOut { get; set; } = true;
    public bool DoubleIn { get; set; } = false;
    public int NumberOfLegs { get; set; } = 3; // Liczba legów w meczu (Best of X)
    public bool MasterOut { get; set; } = false; // Czy zakończenie wymaga podwójnego lub potrójnego pola

    public override void Validate()
    {
        base.Validate();
        if (StartingScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(StartingScore), "Punkty startowe muszą być większe od zera");
        if (StartingScore > 1001) // Praktyczne ograniczenie
            throw new ArgumentOutOfRangeException(nameof(StartingScore), "Punkty startowe nie mogą przekraczać 1001");
        if (NumberOfLegs <= 0)
            throw new ArgumentOutOfRangeException(nameof(NumberOfLegs), "Liczba legów musi być większa od zera");
        if (DoubleOut && MasterOut)
            throw new ArgumentException("Nie można jednocześnie używać DoubleOut i MasterOut");
    }
}

public class CricketConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("237587C5-3223-4B1C-AECC-FADA6FCF10DE");
    public bool UseCutThroatRules { get; set; } = false;
    public bool EnableScoring { get; set; } = true;
    public List<int> TargetNumbers { get; set; } = new() { 15, 16, 17, 18, 19, 20, 25 }; 
    public int HitsToClose { get; set; } = 3; // Ile trafień potrzeba, żeby zamknąć liczbę
    public bool RequireAllNumbersClosed { get; set; } = true; // Czy wszytkie liczby muszą być zamknięte, aby wygrać
    public bool AllowRandomNumbers { get; set; } = false; // Czy liczby mogą być losowo wybrane

    public override void Validate()
    {
        base.Validate();
        if (TargetNumbers == null || !TargetNumbers.Any())
            throw new ArgumentException("Lista numerów celów nie może być pusta", nameof(TargetNumbers));
        if (HitsToClose <= 0)
            throw new ArgumentOutOfRangeException(nameof(HitsToClose), "Liczba trafień do zamknięcia musi być większa od zera");
        foreach (var number in TargetNumbers)
        {
            if ((number < 1 || number > 20) && number != 25)
                throw new ArgumentOutOfRangeException(nameof(TargetNumbers), $"Nieprawidłowy numer celu: {number}");
        }
    }
}

public class AroundTheClockConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("246ED1BB-D57A-4778-B062-2B7445E356BD");
    public bool IncludeBullseye { get; set; } = true;
    public bool AllowAnySegment { get; set; } = false; // Jeśli false, wymagany jest tylko pojedynczy segment
    public int StartingNumber { get; set; } = 1;
    public int EndingNumber { get; set; } = 20;

    public override void Validate()
    {
        base.Validate();
        if (StartingNumber < 1 || StartingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(StartingNumber), "Numer początkowy musi być między 1 a 20");
        if (EndingNumber < StartingNumber || EndingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(EndingNumber), "Numer końcowy musi być między numerem początkowym a 20");
    }
}

public class ShanghaiConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("98C579D9-A550-416F-88B1-C896C5ED62FA");
    public int StartingNumber { get; set; } = 1;
    public int EndingNumber { get; set; } = 7;
    public bool AllowInstantWin { get; set; } = true; // Czy zdobycie Shanghai (pojedynczy, podwójny i potrójny) daje natychmiastowe zwycięstwo
    public int PointsForSingle { get; set; } = 1;
    public int PointsForDouble { get; set; } = 2;
    public int PointsForTriple { get; set; } = 3;

    public override void Validate()
    {
        base.Validate();
        if (StartingNumber < 1 || StartingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(StartingNumber), "Numer początkowy musi być między 1 a 20");
        if (EndingNumber < StartingNumber || EndingNumber > 20)
            throw new ArgumentOutOfRangeException(nameof(EndingNumber), "Numer końcowy musi być między numerem początkowym a 20");
        if (PointsForSingle <= 0 || PointsForDouble <= 0 || PointsForTriple <= 0)
            throw new ArgumentOutOfRangeException("Punkty za trafienie muszą być większe od zera");
    }
}

public class KillerConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("3D58E5C8-EFE6-4DC4-8BA9-7DBC9E8D718D");
    public int HitsToQualifyAsKiller { get; set; } = 5;
    public int Lives { get; set; } = 3; // Liczba żyć każdego gracza
    public bool RequireDoubleToKill { get; set; } = false; // Czy tylko podwójne trafiają przeciwnika
    public bool AutoAssignTargets { get; set; } = true; // Automatycznie przypisz numery celów dla graczy

    public override void Validate()
    {
        base.Validate();
        if (HitsToQualifyAsKiller <= 0)
            throw new ArgumentOutOfRangeException(nameof(HitsToQualifyAsKiller), "Liczba trafień do zostania Killerem musi być większa od zera");
        if (Lives <= 0)
            throw new ArgumentOutOfRangeException(nameof(Lives), "Liczba żyć musi być większa od zera");
    }
}

public class HighScoreConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("66B95992-2771-4BD8-8CB6-EE36FD0507E5");
    public int TargetScore { get; set; } = 170; // Domyślny cel punktowy
    public int NumberOfRounds { get; set; } = 10; // Liczba rund w grze
    public bool AllowExceedingTargetScore { get; set; } = true; // Czy można przekroczyć cel punktowy

    public override void Validate()
    {
        base.Validate();
        if (TargetScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(TargetScore), "Cel punktowy musi być większy od zera");
        if (NumberOfRounds <= 0)
            throw new ArgumentOutOfRangeException(nameof(NumberOfRounds), "Liczba rund musi być większa od zera");
    }
}

public class LegsConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("B23C5C17-230D-4ECF-986B-0D01B8E2F678");
    public int LegsToWin { get; set; } = 3; // Ilość legów potrzebnych do wygrania (np. Best of 5 = 3 legi)
    public int SetsToWin { get; set; } = 0; // Ilość setów potrzebnych do wygrania (0 = gra tylko na legi)
    public int LegsPerSet { get; set; } = 3; // Ilość legów potrzebnych do wygrania setu
    public bool AlternateStart { get; set; } = true; // Czy gracze naprzemiennie rozpoczynają legi

    public override void Validate()
    {
        base.Validate();
        if (LegsToWin <= 0 && SetsToWin <= 0)
            throw new ArgumentOutOfRangeException("Należy określić liczbę legów lub setów do wygrania");
        if (SetsToWin > 0 && LegsPerSet <= 0)
            throw new ArgumentOutOfRangeException(nameof(LegsPerSet), "Liczba legów w secie musi być większa od zera");
    }
}

public class HalveItConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("6088201C-6649-4904-B24C-CEFBEC355F24");
    public List<int> TargetNumbers { get; set; } = new() { 15, 16, 17, 18, 19, 20, 25 };
    public bool IncludeDoubles { get; set; } = true; // Czy dodać rundę na podwójne pola
    public bool IncludeTriples { get; set; } = true; // Czy dodać rundę na potrójne pola
    public bool StrictRules { get; set; } = true; // Czy wymagane jest trafienie w określony cel w danej rundzie

    public override void Validate()
    {
        base.Validate();
        if (TargetNumbers == null || !TargetNumbers.Any())
            throw new ArgumentException("Lista numerów celów nie może być pusta", nameof(TargetNumbers));
        foreach (var number in TargetNumbers)
        {
            if ((number < 1 || number > 20) && number != 25)
                throw new ArgumentOutOfRangeException(nameof(TargetNumbers), $"Nieprawidłowy numer celu: {number}");
        }
    }
}

public class GotchaConfiguration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("59E102BA-719D-4A1A-8271-BE5A637792C1");
    public int TargetScore { get; set; } = 250; // Cel punktowy do osiągnięcia
    public bool BustRule { get; set; } = true; // Czy przekroczenie celu cofa do poprzedniego wyniku
    public bool CanAttackOthers { get; set; } = true; // Czy można atakować innych graczy
    public bool RequireExactScore { get; set; } = true; // Czy wymagane jest dokładne trafienie celu

    public override void Validate()
    {
        base.Validate();
        if (TargetScore <= 0)
            throw new ArgumentOutOfRangeException(nameof(TargetScore), "Cel punktowy musi być większy od zera");
    }
}

public class Bob27Configuration : GameModeConfiguration
{
    public override Guid GameModeId => Guid.Parse("97544A51-2217-449A-BA61-D83BA90664CD");
    public int StartingPoints { get; set; } = 27;
    public bool GameOverOnNegativeScore { get; set; } = false; // Czy gra kończy się, gdy gracz osiągnie wynik ujemny
    public bool IncludeBullseye { get; set; } = true; // Czy uwzględnić bullseye jako ostatni cel
    public int DartsPerDouble { get; set; } = 3; // Liczba rzutów na każde podwójne pole

    public override void Validate()
    {
        base.Validate();
        if (StartingPoints <= 0)
            throw new ArgumentOutOfRangeException(nameof(StartingPoints), "Punkty startowe muszą być większe od zera");
        if (DartsPerDouble <= 0)
            throw new ArgumentOutOfRangeException(nameof(DartsPerDouble), "Liczba rzutów na podwójne pole musi być większa od zera");
    }
}