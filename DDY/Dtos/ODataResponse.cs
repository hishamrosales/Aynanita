using System.Text.Json.Serialization;

namespace DDY.Dtos;

/// <summary>Wrapper de las colecciones OData: { "@odata.context": ..., "@odata.count": n, "value": [...] }</summary>
public class ODataResponse<T>
{
    [JsonPropertyName("@odata.context")]
    public string? Context { get; set; }

    [JsonPropertyName("@odata.count")]
    public long? Count { get; set; }

    [JsonPropertyName("value")]
    public List<T> Value { get; set; } = new();
}
