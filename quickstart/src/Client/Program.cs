using System.Text.Json;
using IdentityModel.Client;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.IsError);
} else Console.WriteLine("Fetched Discovery Document");

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1"
});

Console.WriteLine(tokenResponse.IsError ? tokenResponse.Error : tokenResponse.AccessToken);

if (!tokenResponse.IsError)
{
    var apiClient = new HttpClient();
    apiClient.SetBearerToken(tokenResponse.AccessToken!);

    var response = await apiClient.GetAsync("https://localhost:6001/api/identity");
    if (!response.IsSuccessStatusCode)
        Console.WriteLine(response.StatusCode);
    else
    {
        var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
        Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
    }
}
Console.ReadLine();