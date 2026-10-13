namespace DDY.Dtos;

public class MovimientoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Poder { get; set; }
    public int Precision { get; set; }
    public int PokemonId { get; set; }
}
