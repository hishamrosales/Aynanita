# Contrato compartido (backend ⇄ cliente)

Base URL: `http://localhost:5080/` (emulador Android: `http://10.0.2.2:5080/`). Prefijo OData: `/odata`.
JSON con propiedades en **PascalCase** (sin camelCase). Colecciones: `{ "@odata.context", "@odata.count"?, "value": [...] }`.

## Entidades (relación 1:N)
| Pokemon | tipo | | Movimiento | tipo |
|---|---|---|---|---|
| Id | int (PK) | | Id | int (PK) |
| Nombre | string 3–30 | | Nombre | string 2–30 |
| Tipo | string | | Tipo | string |
| Rareza | string | | Poder | int 0–250 |
| Estado | Saludable / Cansado / Herido | | Precision | int 1–100 |
| Hp | int 1–300 | | PokemonId | int (FK) |
| Movimientos | ICollection&lt;Movimiento&gt; (nav) | | Pokemon | Pokemon (nav) |

## Endpoints
| Verbo | Ruta | Uso |
|---|---|---|
| GET | `/odata/Pokemon?$expand=Movimientos&$filter=...&$orderby=...` | Lista (con `$filter`, `$expand`) |
| GET | `/odata/Pokemon({id})?$expand=Movimientos` | Detalle |
| POST | `/odata/Pokemon` | Alta (`Nombre, Tipo, Rareza, Estado, Hp`) |
| DELETE | `/odata/Pokemon({id})` | Baja (cascada a movimientos) |
| GET | `/odata/Movimientos?$filter=Poder gt 80` | Consulta de movimientos |
| POST | `/odata/Movimientos` | Alta (`Nombre, Tipo, Poder, Precision, PokemonId`) |
| GET | `/odata/$metadata` | Modelo EDM |

## DTOs del cliente (`DDY/Dtos`)
`ODataResponse<T>`, `PokemonDto` (+ calculadas `TotalMovimientos`, `PoderTotal`, `PoderPromedio`), `MovimientoDto`, `PokemonCrearDto` (formulario con DataAnnotations).

## Servicio (`DDY/Services/PokemonApiService`)
`ObtenerPokemonAsync(tipo?, busqueda?)`, `ObtenerPorIdAsync(id)`, `CrearAsync(PokemonCrearDto)`, `EliminarAsync(id)`.

## Rutas del cliente
`/` lista + master-detail · `/pokemon/{Id:int}` detalle · `/pokemon/nuevo` alta · `/favoritos`.
