using DDY.Api.Data;
using DDY.Api.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// SQLite en memoria compartida: la BD vive mientras el proceso esté activo.
// La conexión "guardián" evita que SQLite destruya la BD al cerrar el último contexto.
const string connectionString = "DataSource=file:pokedb?mode=memory&cache=shared";
var keeper = new SqliteConnection(connectionString);
keeper.Open();
builder.Services.AddSingleton(keeper);
builder.Services.AddDbContext<PokemonContext>(o => o.UseSqlite(connectionString));

static IEdmModel GetEdmModel()
{
    var mb = new ODataConventionModelBuilder();
    mb.EntitySet<Pokemon>("Pokemon");        // /odata/Pokemon
    mb.EntitySet<Movimiento>("Movimientos"); // /odata/Movimientos
    return mb.GetEdmModel();
}

builder.Services.AddControllers().AddOData(o => o
    .Select().Filter().Expand().OrderBy().Count().SetMaxTop(200)
    .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Crea el esquema y aplica los datos semilla (HasData)
    scope.ServiceProvider.GetRequiredService<PokemonContext>().Database.EnsureCreated();
}

app.UseCors();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/odata/Pokemon?$expand=Movimientos"));

app.Run();
