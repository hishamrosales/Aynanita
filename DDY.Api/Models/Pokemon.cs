using System.ComponentModel.DataAnnotations;

namespace DDY.Api.Models;

public class Pokemon
{
    public int Id { get; set; }

    [Required, StringLength(30, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    public string Rareza { get; set; } = string.Empty;

    [Required]
    public string Estado { get; set; } = string.Empty;

    [Range(1, 300)]
    public int Hp { get; set; }

    // Navigation Property (lado "1" de la relación 1:N)
    public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
