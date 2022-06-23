using Flurl.Http;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public static class Extensions
{
    public static string JsonPrettify(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }

    public static string FormatRequest(this FlurlCall call, bool withHeaders = false)
    {
        string verb = call.Request.Verb.ToString();
        string path = call.Request.Url;
        string body = call.Request.Url.Path == "/gateway/deal/session" ? "*******" : call.RequestBody.JsonPrettify();
        if (withHeaders)
        {
            string headers = string.Join(Environment.NewLine, call.Request.Headers.Select((name, index) => $"{index + 1}: {name}"));
            return $"--> HTTP {verb} {call.Request.Url}{Environment.NewLine}HEADERS:{Environment.NewLine}{headers}{Environment.NewLine}BODY:{Environment.NewLine}:{body}";
        }
        return $"--> HTTP {verb} {call.Request.Url}{Environment.NewLine}{body}";
    }

    public static async Task<string> FormatResponse(this FlurlCall call, bool withHeaders = false)
    {
        var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
        var status = call.Response.ResponseMessage?.StatusCode;

        double? duration = call.Completed ? call.Duration?.TotalMilliseconds : 0;
        if (withHeaders)
        {
            string headers = string.Join(Environment.NewLine, call.Response?.Headers?.Select((name, index) => $"{index + 1}: {name}"));
            return $"<-- HTTP {status} [{call}] [duration:{duration}ms]{Environment.NewLine}HEADERS:{Environment.NewLine}{headers}{Environment.NewLine}BODY:{Environment.NewLine}:{response}";
        }
        return $"<-- HTTP {status} [{call}] [duration:{duration}ms]{Environment.NewLine}{response}";
    }
}