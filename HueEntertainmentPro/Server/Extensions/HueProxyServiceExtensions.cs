using System.Net;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace HueEntertainmentPro.Server.Extensions
{
  public static class HueProxyServiceExtensions
  {
    public static IServiceCollection AddHueProxyService(this IServiceCollection services)
    {

      services.AddReverseProxy()
      .LoadFromMemory(
      [
        new RouteConfig()
        {
          RouteId = "hue-proxy-route",
          ClusterId = "hue-proxy-cluster",
          Match = new RouteMatch
          {
              Path = "/hueproxy/{ip}/{**catchall}"
          }
        }
      ],
      [
        new ClusterConfig()
        {
          ClusterId = "hue-proxy-cluster",
          Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
          {
              { "destination1", new DestinationConfig() { Address = "http://placeholder/" } }
          },
          HttpRequest = new ForwarderRequestConfig()
          {
              ActivityTimeout = TimeSpan.FromSeconds(30)
          },
          HttpClient = new HttpClientConfig
          {
              DangerousAcceptAnyServerCertificate = true
          }
        }
    ])
    .AddTransforms(builderContext =>
    {
      // Extract IP, validate, and transform the destination URL in one transform
      builderContext.AddRequestTransform(transformContext =>
      {
        var path = transformContext.HttpContext.Request.Path.Value;

        if (!string.IsNullOrEmpty(path) && path.StartsWith("/hueproxy/"))
        {
          var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

          if (segments.Length >= 2)
          {
            var ipAddress = segments[1];

            // Validate IP address format
            if (IPAddress.TryParse(ipAddress, out _))
            {
              if (segments.Length >= 3)
              {
                // Remove "hueproxy" and IP segments, keep the rest
                var newPath = "/" + string.Join("/", segments.Skip(2));
                var queryString = transformContext.HttpContext.Request.QueryString.Value;
                var newUrl = $"https://{ipAddress}{newPath}{queryString}";
                transformContext.ProxyRequest.RequestUri = new Uri(newUrl);

                // Update Host header to match target
                transformContext.ProxyRequest.Headers.Host = ipAddress;
              }
              else
              {
                // Handle case where there's only IP but no additional path
                var newUrl = $"https://{ipAddress}/";
                transformContext.ProxyRequest.RequestUri = new Uri(newUrl);
                transformContext.ProxyRequest.Headers.Host = ipAddress;
              }
            }
            else
            {
              // Invalid IP address - return 400 Bad Request
              transformContext.HttpContext.Response.StatusCode = 400;
              transformContext.HttpContext.Response.WriteAsync($"Invalid IP address format: {ipAddress}");
              return ValueTask.CompletedTask;
            }
          }
          else
          {
            // Missing IP address in route
            transformContext.HttpContext.Response.StatusCode = 400;
            transformContext.HttpContext.Response.WriteAsync("Missing IP address in route");
            return ValueTask.CompletedTask;
          }
        }

        return ValueTask.CompletedTask;
      });

      // Optional header customization
      builderContext.AddRequestHeaderRemove("X-Forwarded-Host");
      builderContext.AddRequestHeader("X-Proxied-By", "HueProxy");
    });

      return services;
    }
  }
}
