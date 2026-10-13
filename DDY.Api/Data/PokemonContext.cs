using DDY.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DDY.Api.Data;

public class PokemonContext(DbContextOptions<PokemonContext> options) : DbContext(options)
{
    public DbSet<Pokemon> Pokemones => Set<Pokemon>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Movimientos)
            .WithOne(m => m.Pokemon)
            .HasForeignKey(m => m.PokemonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pokemon>().HasData(SeedData.Pokemones);
        modelBuilder.Entity<Movimiento>().HasData(SeedData.Movimientos);
    }
}
