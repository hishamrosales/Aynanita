using System.ComponentModel.DataAnnotations;

namespace DDY.Dtos;

/// <summary>Modelo del formulario de alta, con validaciones declarativas y mensajes individuales.</summary>
public class PokemonCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un tipo.")]
    public string Tipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona una rareza.")]
    public string Rareza { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un estado.")]
    public string Estado { get; set; } = string.Empty;

    [Range(1, 300, ErrorMessage = "Los HP deben estar entre 1 y 300.")]
    public int Hp { get; set; } = 50;

    // Movimiento inicial (opcional)
    [StringLength(30, ErrorMessage = "El movimiento no puede superar 30 caracteres.")]
    public string? MovimientoNombre { get; set; }

    [Range(0, 250, ErrorMessage = "El poder debe estar entre 0 y 250.")]
    public int MovimientoPoder { get; set; } = 40;
}

public static class Catalogos
{
    public static readonly string[] Tipos =
        ["Normal", "Fuego", "Agua", "Planta", "Eléctrico", "Psíquico", "Fantasma", "Dragón"];
    public static readonly string[] Rarezas = ["Común", "Rara", "Ultra Rara"];
    public static readonly string[] Estados = ["Saludable", "Cansado", "Herido"];
}
