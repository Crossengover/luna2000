namespace luna2000.Service;

public interface IFileStorage
{
    Task<Guid> SaveFileAsync(byte[] data, string fileExtension);

    Task<byte[]> LoadFileAsync(Guid fileId);
}