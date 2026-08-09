namespace FixNow.Application.Common.Interfaces.Storage;

public interface IFileStorage
{
    Task<Result<string>> StoreAsync(
        string key,
        Stream content,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> DeleteAsync(
        string key,
        CancellationToken cancellationToken = default);
}
