using System.Net.Http.Headers;

namespace MyFeedlyClient.Console
{
    static class ClientExtensions
    {
        private static string Bearer = nameof(Bearer);

        public static void AddJWTAuth(this Client client)
        {
            client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, client.JWT);
        }

        public static void RemoveJWTAuth(this Client client)
        {
            client.HttpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}