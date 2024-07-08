using System.Net.Http;
using System.Text;
using System.Text.Json;

public class NodeRedConnector
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public NodeRedConnector(string baseUrl)
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
    }

    public async Task<string> SendMessageToNodeRed(string endpoint, object payload)
    {
        string jsonPayload = JsonSerializer.Serialize(payload);
        StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
    }
}