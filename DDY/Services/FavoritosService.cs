using Microsoft.Maui.Storage;

namespace DDY.Services;

/// <summary>Favoritos locales (conserva la función del primer parcial). Se guardan por Id en Preferences.</summary>
public class FavoritosService
{
    private const string Clave = "favoritos_pokemon";
    private readonly HashSet<int> _ids;

    public event Action? Cambio;

    public FavoritosService()
    {
        var guardado = Preferences.Default.Get(Clave, string.Empty);
        _ids = guardado.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => int.TryParse(s, out var n) ? n : -1)
                       .Where(n => n > 0)
                       .ToHashSet();
    }

    public bool EsFavorito(int id) => _ids.Contains(id);
    public IReadOnlyCollection<int> Ids => _ids;

    public void Alternar(int id)
    {
        if (!_ids.Remove(id)) _ids.Add(id);
        Preferences.Default.Set(Clave, string.Join(',', _ids));
        Cambio?.Invoke();
    }
}
