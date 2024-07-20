using System.Net;
using System.Text.Json.Serialization;

namespace Gnome.Sys;

public class HttpResponseError : ExceptionError
{
    public HttpResponseError(string message)
        : base(message)
    {
    }

    public HttpResponseError(string message, IInnerError inner)
        : base(message, inner)
    {
    }

    public HttpResponseError(HttpRequestException ex)
        : base(ex)
    {
#if !NETLEGACY
        this.StatusCode = ex.StatusCode;
#endif
    }

    [JsonPropertyName("status")]
    public HttpStatusCode? StatusCode { get; set; }

    public override Exception ToException()
    {
        if (this.Exception is not null)
            return this.Exception;

#if NETLEGACY
        return new HttpRequestException(this.Message);
#else
        return new HttpRequestException(this.Message, null, this.StatusCode);
#endif
    }
}