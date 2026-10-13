namespace DDY.Dtos;

public class PokemonDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Rareza { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int Hp { get; set; }
    public List<MovimientoDto> Movimientos { get; set; } = new();

    // Propiedades calculadas (relacionadas con la colección Movimientos)
    public int TotalMovimientos => Movimientos.Count;
    public int PoderTotal => Movimientos.Sum(m => m.Poder);
    public double PoderPromedio => TotalMovimientos == 0 ? 0 : Math.Round((double)PoderTotal / TotalMovimientos, 1);
}
