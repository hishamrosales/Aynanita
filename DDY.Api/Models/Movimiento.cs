using System.ComponentModel.DataAnnotations;

namespace DDY.Api.Models;

public class Movimiento
{
    public int Id { get; set; }

    [Required, StringLength(30, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    [Range(0, 250)]
    public int Poder { get; set; }

    [Range(1, 100)]
    public int Precision { get; set; } = 100;

    // Llave foránea + Navigation Property (lado "N" de la relación 1:N)
    public int PokemonId { get; set; }
    public Pokemon? Pokemon { get; set; }
}
