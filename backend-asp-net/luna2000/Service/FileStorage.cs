namespace luna2000.Service;

public class FileStorage : IFileStorage
{
    private readonly string _saveDir;

    public FileStorage(string saveDir = "files")
    {
        _saveDir = saveDir;
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }
    }

    public async Task<Guid> SaveFileAsync(byte[] data, string fileExtension)
    {
        var fileId = Guid.NewGuid();
        await File.WriteAllBytesAsync($"{_saveDir}/{fileId}{fileExtension}", data);
        return fileId;
    }

    public async Task<byte[]> LoadFileAsync(Guid fileId)
    {
        var path = $"{_saveDir}/{fileId}";

        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        return await File.ReadAllBytesAsync(path);
    }
}