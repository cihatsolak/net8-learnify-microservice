namespace Learnify.Order.Application.Interfaces.Refit;

internal sealed class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return base.SendAsync(request, cancellationToken);
        }

        if (!httpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeader))
        {
            return base.SendAsync(request, cancellationToken);
        }

        var token = authHeader.ToString().Split(' ', 2).ElementAtOrDefault(1);
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}

