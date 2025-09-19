using System.Text.Json;

namespace HueEntertainmentPro.Server.Services
{
  public static class JsonElementConverter
  {
    public static Dictionary<string, object> ConvertJsonElementDictionary(Dictionary<string, JsonElement> source)
    {
      var result = new Dictionary<string, object>();
      foreach (var kvp in source)
      {
        result[kvp.Key] = ConvertJsonElement(kvp.Value);
      }
      return result;
    }

    private static object ConvertJsonElement(JsonElement element)
    {
      switch (element.ValueKind)
      {
        case JsonValueKind.String:
          return element.GetString() ?? string.Empty;
        case JsonValueKind.Number:
          if (element.TryGetInt32(out int intValue))
            return intValue;
          if (element.TryGetInt64(out long longValue))
            return longValue;
          return element.GetDouble();
        case JsonValueKind.True:
          return true;
        case JsonValueKind.False:
          return false;
        case JsonValueKind.Null:
          return null!;
        case JsonValueKind.Array:
          var array = new List<object>();
          foreach (var item in element.EnumerateArray())
          {
            array.Add(ConvertJsonElement(item));
          }
          return array;
        case JsonValueKind.Object:
          var dict = new Dictionary<string, object>();
          foreach (var property in element.EnumerateObject())
          {
            dict[property.Name] = ConvertJsonElement(property.Value);
          }
          return dict;
        default:
          throw new NotSupportedException($"Unsupported JsonValueKind: {element.ValueKind}");
      }
    }
  }
}
