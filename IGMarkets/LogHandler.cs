using Flurl.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IGMarkets
{
    internal static class LogHandler
    {
        public static string ReadSanitizedRequest(FlurlCall call)
        {
            string verb = call.Request.Verb.ToString();
            string path = call.Request.Url;
            string headers = string.Join(Environment.NewLine, call.Request.Headers.Select((name, index) => $"{index + 1}: {name}"));
            string body = call.Request.Url.Path == "/gateway/deal/session" ? "*******" : call.RequestBody.JsonPrettify();

            return $"--> HTTP {verb} {call.Request.Url}{Environment.NewLine}HEADERS:{Environment.NewLine}{headers}{Environment.NewLine}BODY:{Environment.NewLine}:{body}";
        }

        public static async Task<string> ReadResponse(FlurlCall call)
        {
            var response = await call.Response.ResponseMessage.Content.ReadAsStringAsync();
            var status = call.Response.ResponseMessage?.StatusCode;
            string headers = string.Join(Environment.NewLine, call.Response?.Headers?.Select((name, index) => $"{index + 1}: {name}"));
            double? duration = call.Completed ? call.Duration?.TotalMilliseconds : 0;

            return $"<-- HTTP {status} [{call}] [duration:{duration}ms]{Environment.NewLine}HEADERS:{Environment.NewLine}{headers}{Environment.NewLine}BODY:{Environment.NewLine}:{response}";
        }
    }
}