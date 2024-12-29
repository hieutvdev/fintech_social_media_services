using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Security;

namespace BuildingBlocks.Repository.ClientCallApi;

public class ClientCallApi : IClientCallApi
{
    private readonly HttpClient _httpClient;
    private readonly IAuthorizeExtension _authorizeExtension;
    
    public ClientCallApi(HttpClient httpClient, IAuthorizeExtension authorizeExtension)
    {
        _httpClient = httpClient;
        _authorizeExtension = authorizeExtension;
    }
    
    
    private void SetAuthorizationHeader()
    {
        string token = _authorizeExtension.GetToken() ?? "";
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T> GetAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
    
        SetAuthorizationHeader();

        if (headers != null)
        {
            foreach (var header in headers)
            {
                if (_httpClient.DefaultRequestHeaders.Contains(header.Key))
                {
                    _httpClient.DefaultRequestHeaders.Remove(header.Key);
                }
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");
        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }


    public async Task<T> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(url, content, cancellationToken).Result;
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");
            
        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");;

        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> DeleteAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var response = await _httpClient.DeleteAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");

        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> PatchAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");

        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> OptionsAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Options, url);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");

        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> HeadAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Head, url);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");
        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }

    public async Task<T> TraceAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        SetAuthorizationHeader();
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                
            }
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Trace, url);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Deserialization resulted in null.");
        }
        catch (HttpRequestException e)
        {
            throw new BadRequestException($"Request error: {e.Message}");
        }
        catch (JsonException e)
        {
            throw new BadRequestException($"Error deserializing response: {e.Message}");
        }
        catch (Exception e)
        {
            throw new BadRequestException($"An unexpected error occurred: {e.Message}");
        }
        finally
        {
            if (headers != null)
            {
                foreach (var header in headers.Keys)
                {
                    _httpClient.DefaultRequestHeaders.Remove(header);
                }
            }
        }
    }
}