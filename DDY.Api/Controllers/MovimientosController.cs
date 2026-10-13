using DDY.Api.Data;
using DDY.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DDY.Api.Controllers;

public class MovimientosController(PokemonContext db) : ODataController
{
    // GET /odata/Movimientos?$filter=Poder gt 80&$expand=Pokemon
    [EnableQuery(MaxExpansionDepth = 2, PageSize = 200)]
    public IQueryable<Movimiento> Get() => db.Movimientos;

    // GET /odata/Movimientos(1)
    [EnableQuery]
    public SingleResult<Movimiento> Get([FromODataUri] int key) =>
        SingleResult.Create(db.Movimientos.Where(m => m.Id == key));

    // POST /odata/Movimientos  (requiere PokemonId existente)
    public async Task<IActionResult> Post([FromBody] Movimiento movimiento)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await db.Pokemones.AnyAsync(p => p.Id == movimiento.PokemonId))
            return BadRequest($"No existe un Pokémon con Id {movimiento.PokemonId}.");

        movimiento.Id = 0;
        movimiento.Pokemon = null;
        db.Movimientos.Add(movimiento);
        await db.SaveChangesAsync();
        return Created(movimiento);
    }

    // DELETE /odata/Movimientos(1)
    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var movimiento = await db.Movimientos.FindAsync(key);
        if (movimiento is null) return NotFound();

        db.Movimientos.Remove(movimiento);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
