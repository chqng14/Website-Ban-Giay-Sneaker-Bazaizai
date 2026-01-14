using Microsoft.Extensions.Configuration;

namespace App_View.Services
{
    public static class HttpClientFactory
    {
        private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7038/";
            return new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
        });

        public static HttpClient CreateClient()
        {
            return _httpClient.Value;
        }
    }
}
