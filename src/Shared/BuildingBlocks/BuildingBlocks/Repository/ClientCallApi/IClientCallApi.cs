namespace BuildingBlocks.Repository.ClientCallApi;

public interface IClientCallApi 
{
    Task<T> GetAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> PostAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> PutAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> PatchAsync<T>(string url, object data, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> OptionsAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> HeadAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
    Task<T> TraceAsync<T>(string url, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}