using DDY.Api.Models;

namespace DDY.Api.Data;

public static class SeedData
{
    public static Pokemon[] Pokemones { get; } =
    [
        new() { Id = 1, Nombre = "Pikachu",   Tipo = "Eléctrico", Rareza = "Común",     Estado = "Saludable", Hp = 35 },
        new() { Id = 2, Nombre = "Charizard", Tipo = "Fuego",     Rareza = "Ultra Rara", Estado = "Saludable", Hp = 78 },
        new() { Id = 3, Nombre = "Blastoise", Tipo = "Agua",      Rareza = "Rara",      Estado = "Cansado",   Hp = 79 },
        new() { Id = 4, Nombre = "Venusaur",  Tipo = "Planta",    Rareza = "Rara",      Estado = "Saludable", Hp = 80 },
        new() { Id = 5, Nombre = "Gengar",    Tipo = "Fantasma",  Rareza = "Rara",      Estado = "Herido",    Hp = 60 },
        new() { Id = 6, Nombre = "Snorlax",   Tipo = "Normal",    Rareza = "Común",     Estado = "Cansado",   Hp = 160 },
        new() { Id = 7, Nombre = "Dragonite", Tipo = "Dragón",    Rareza = "Ultra Rara", Estado = "Saludable", Hp = 91 },
        new() { Id = 8, Nombre = "Mewtwo",    Tipo = "Psíquico",  Rareza = "Ultra Rara", Estado = "Saludable", Hp = 106 },
    ];

    // (PokemonId, Nombre, Tipo, Poder, Precisión)
    private static readonly (int Pokemon, string Nombre, string Tipo, int Poder, int Precision)[] Raw =
    [
        (1, "Impactrueno", "Eléctrico", 40, 100), (1, "Rayo", "Eléctrico", 90, 100), (1, "Cola de Hierro", "Acero", 100, 75),
        (2, "Lanzallamas", "Fuego", 90, 100), (2, "Garra Dragón", "Dragón", 80, 100), (2, "Tajo Aéreo", "Volador", 75, 95),
        (3, "Hidrobomba", "Agua", 110, 80), (3, "Surf", "Agua", 90, 100), (3, "Cabezazo", "Normal", 70, 100),
        (4, "Latigazo", "Planta", 45, 100), (4, "Rayo Solar", "Planta", 120, 100), (4, "Bomba Lodo", "Veneno", 90, 100),
        (5, "Bola Sombra", "Fantasma", 80, 100), (5, "Lengüetazo", "Fantasma", 30, 100), (5, "Bomba Lodo", "Veneno", 90, 100),
        (6, "Golpe Cuerpo", "Normal", 85, 100), (6, "Descanso", "Psíquico", 0, 100), (6, "Hiperrayo", "Normal", 150, 90),
        (7, "Enfado", "Dragón", 120, 100), (7, "Ala de Acero", "Acero", 70, 90), (7, "Hiperrayo", "Normal", 150, 90),
        (8, "Psíquico", "Psíquico", 90, 100), (8, "Bola Sombra", "Fantasma", 80, 100), (8, "Rayo Hielo", "Hielo", 90, 100),
    ];

    public static Movimiento[] Movimientos { get; } = Raw
        .Select((m, i) => new Movimiento
        {
            Id = i + 1,
            PokemonId = m.Pokemon,
            Nombre = m.Nombre,
            Tipo = m.Tipo,
            Poder = m.Poder,
            Precision = m.Precision
        })
        .ToArray();
}
