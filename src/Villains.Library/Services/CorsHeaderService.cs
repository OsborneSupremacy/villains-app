using System.Net.Mime;

namespace Villains.Library.Services;

public static class CorsHeaderService
{
   public static Dictionary<string, string> GetCorsHeaders() =>
       new()
       {
           { "Content-Type", MediaTypeNames.Application.Json },
           { "Access-Control-Allow-Origin", "*" }
       };
}