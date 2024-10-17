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
        var path = Path.Combine(_saveDir, $"{fileId}{fileExtension}");
        await File.WriteAllBytesAsync(path, data);
        return fileId;
    }

    public async Task<byte[]> LoadFileAsync(Guid fileId, string fileExtension)
    {
        var path = Path.Combine(_saveDir, fileId.ToString(), fileExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }

        return await File.ReadAllBytesAsync(path);
    }

    public void DeletePhoto(Guid photoId)
    {
        var file = Directory.GetFiles(_saveDir, $"{photoId}*")
            .FirstOrDefault();

        if (file != null)
        {
            File.Delete(file);
        }
    }
}