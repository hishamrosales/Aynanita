using DDY.Api.Data;
using DDY.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace DDY.Api.Controllers;

public class PokemonController(PokemonContext db) : ODataController
{
    // GET /odata/Pokemon?$expand=Movimientos&$filter=Tipo eq 'Fuego'
    [EnableQuery(MaxExpansionDepth = 3, PageSize = 100)]
    public IQueryable<Pokemon> Get() => db.Pokemones;

    // GET /odata/Pokemon(1)?$expand=Movimientos
    [EnableQuery(MaxExpansionDepth = 3)]
    public SingleResult<Pokemon> Get([FromODataUri] int key) =>
        SingleResult.Create(db.Pokemones.Where(p => p.Id == key));

    // POST /odata/Pokemon
    public async Task<IActionResult> Post([FromBody] Pokemon pokemon)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        pokemon.Id = 0;
        pokemon.Movimientos = new List<Movimiento>();
        db.Pokemones.Add(pokemon);
        await db.SaveChangesAsync();
        return Created(pokemon);
    }

    // DELETE /odata/Pokemon(1)
    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var pokemon = await db.Pokemones.Include(p => p.Movimientos).FirstOrDefaultAsync(p => p.Id == key);
        if (pokemon is null) return NotFound();

        db.Pokemones.Remove(pokemon);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
