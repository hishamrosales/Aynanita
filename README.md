# Pokédex OData — MAUI Blazor Hybrid + ASP.NET Core OData

Evolución del proyecto del primer parcial (MAUI XAML/MVVM, .NET 10) a **MAUI Blazor Hybrid** que consume un backend **ASP.NET Core OData** propio con **SQLite en memoria**. Ya no usa la API pública.

```
DDY.slnx
├─ DDY.Api/   Backend (Persona 1)
└─ DDY/       Cliente MAUI Blazor Hybrid (Personas 2 y 3)
```

## Requisitos
.NET SDK 10 y workload MAUI (`dotnet workload install maui`). Windows 10/11 para el cliente de escritorio, o emulador Android.

## Ejecutar
```bash
# 1) Backend (dejar corriendo)
cd DDY.Api
dotnet restore
dotnet run --urls http://localhost:5080      # dispositivo físico: --urls http://0.0.0.0:5080

# 2) Cliente (otra terminal)
cd DDY
dotnet restore
dotnet build -f net10.0-windows10.0.19041.0
dotnet run   -f net10.0-windows10.0.19041.0  # Android: -f net10.0-android (usa 10.0.2.2:5080)
```
La BD es en memoria: los datos creados persisten **mientras el backend esté activo**; al reiniciarlo vuelven los datos semilla.
Dispositivo físico: cambiar `ApiBaseUrl` en `DDY/MauiProgram.cs` por la IP LAN del equipo.

## Consultas OData (navegador o curl)
```bash
curl "http://localhost:5080/odata/Pokemon?\$expand=Movimientos"
curl "http://localhost:5080/odata/Pokemon?\$filter=Tipo%20eq%20'Fuego'&\$expand=Movimientos"
curl "http://localhost:5080/odata/Pokemon?\$filter=contains(tolower(Nombre),'char')&\$expand=Movimientos"
curl "http://localhost:5080/odata/Pokemon?\$filter=Hp%20gt%2080&\$orderby=Hp%20desc&\$count=true"
curl "http://localhost:5080/odata/Pokemon(1)?\$expand=Movimientos"
curl "http://localhost:5080/odata/Movimientos?\$filter=Poder%20gt%2090&\$expand=Pokemon"
curl "http://localhost:5080/odata/\$metadata"

# Alta
curl -X POST http://localhost:5080/odata/Pokemon -H "Content-Type: application/json" \
  -d '{"Nombre":"Lucario","Tipo":"Normal","Rareza":"Rara","Estado":"Saludable","Hp":70}'
curl -X POST http://localhost:5080/odata/Movimientos -H "Content-Type: application/json" \
  -d '{"Nombre":"Esfera Aural","Tipo":"Normal","Poder":80,"Precision":100,"PokemonId":9}'
curl -X DELETE "http://localhost:5080/odata/Pokemon(9)"
```

## Demostración
1. Lista `/` → buscar/filtrar (`$filter`) → clic en un Pokémon (master-detail con movimientos y propiedades calculadas).
2. «Ver detalle completo» → `/pokemon/{Id}` → «Regresar al listado».
3. «Nuevo Pokémon» → llenar y guardar → vuelve a la lista con el nuevo registro seleccionado.
4. **Validación fallida:** en el formulario pulsar «Demostrar envío inválido» (o «Guardar» con el formulario vacío): se muestran mensajes por campo y resumen.

## Git (3 ramas → PR a `main`)
| Persona | Rama | Archivos |
|---|---|---|
| 1 | `feature/backend-odata` | `DDY.Api/**`, `docs/CONTRATO.md`, este README (secciones backend) |
| 2 | `feature/cliente-maestro-detalle` | `DDY/Dtos/**`, `DDY/Services/**`, `DDY/MauiProgram.cs`, `DDY/Components/Pages/PokemonLista.razor`, `DDY/Components/Pages/FavoritosPage.razor`, `DDY/Components/Shared/PokemonFicha.razor` |
| 3 | `feature/ui-formularios-fiori` | `DDY/Components/Pages/PokemonDetalle.razor`, `DDY/Components/Pages/PokemonForm.razor`, `DDY/Components/Layout/**`, `DDY/Components/Routes.razor`, `DDY/Components/Shared/EstadoChip.razor`, `DDY/wwwroot/**`, `DDY/MainPage.xaml*` |

Base compartida (commit inicial en `main` antes de las ramas): `DDY/DDY.csproj`, `DDY/_Imports.razor`, `DDY/App.xaml*`, `DDY.slnx`, plataformas.
```bash
git checkout -b feature/<rama> && git add <archivos> && git commit -m "feat: ..." && git push -u origin feature/<rama>
```
