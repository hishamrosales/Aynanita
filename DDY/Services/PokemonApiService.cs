using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DDY.Dtos;

namespace DDY.Services;

/// <summary>Servicio tipado (AddHttpClient&lt;PokemonApiService&gt;) que consume exclusivamente la API OData propia.</summary>
public class PokemonApiService(HttpClient http)
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private static string Esc(string s) => s.Replace("'", "''");

    /// <summary>GET /odata/Pokemon?$expand=Movimientos[&$filter=...]</summary>
    public async Task<List<PokemonDto>> ObtenerPokemonAsync(string? tipo = null, string? busqueda = null, CancellationToken ct = default)
    {
        var filtros = new List<string>();
        if (!string.IsNullOrWhiteSpace(tipo))
            filtros.Add($"Tipo eq '{Esc(tipo)}'");
        if (!string.IsNullOrWhiteSpace(busqueda))
            filtros.Add($"contains(tolower(Nombre),'{Esc(busqueda.Trim().ToLowerInvariant())}')");

        var url = "odata/Pokemon?$expand=Movimientos&$orderby=Id";
        if (filtros.Count > 0)
            url += "&$filter=" + Uri.EscapeDataString(string.Join(" and ", filtros));

        var respuesta = await http.GetFromJsonAsync<ODataResponse<PokemonDto>>(url, Json, ct);
        return respuesta?.Value ?? new();
    }

    /// <summary>GET /odata/Pokemon(id)?$expand=Movimientos. Devuelve null si no existe.</summary>
    public async Task<PokemonDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        using var resp = await http.GetAsync($"odata/Pokemon({id})?$expand=Movimientos", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return null;
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<PokemonDto>(Json, ct);
    }

    /// <summary>POST /odata/Pokemon y, si se indicó, POST /odata/Movimientos con el Id creado.</summary>
    public async Task<PokemonDto> CrearAsync(PokemonCrearDto modelo, CancellationToken ct = default)
    {
        var cuerpo = new { modelo.Nombre, modelo.Tipo, modelo.Rareza, modelo.Estado, modelo.Hp };
        using var resp = await http.PostAsJsonAsync("odata/Pokemon", cuerpo, Json, ct);
        await AsegurarExitoAsync(resp, ct);
        var creado = await resp.Content.ReadFromJsonAsync<PokemonDto>(Json, ct)
                     ?? throw new InvalidOperationException("La API no devolvió el Pokémon creado.");

        if (!string.IsNullOrWhiteSpace(modelo.MovimientoNombre))
        {
            var mov = new { Nombre = modelo.MovimientoNombre.Trim(), Tipo = modelo.Tipo, Poder = modelo.MovimientoPoder, Precision = 100, PokemonId = creado.Id };
            using var respMov = await http.PostAsJsonAsync("odata/Movimientos", mov, Json, ct);
            await AsegurarExitoAsync(respMov, ct);
        }
        return creado;
    }

    /// <summary>DELETE /odata/Pokemon(id)</summary>
    public async Task EliminarAsync(int id, CancellationToken ct = default)
    {
        using var resp = await http.DeleteAsync($"odata/Pokemon({id})", ct);
        await AsegurarExitoAsync(resp, ct);
    }

    private static async Task AsegurarExitoAsync(HttpResponseMessage resp, CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;
        var detalle = await resp.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException($"La API respondió {(int)resp.StatusCode}: {detalle}", null, resp.StatusCode);
    }
}
