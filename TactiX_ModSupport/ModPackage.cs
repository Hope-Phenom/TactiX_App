using System.IO.Compression;
using Newtonsoft.Json;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public class ModPackage : IDisposable
{
    private readonly ZipArchive _archive;
    private readonly Dictionary<string, ZipArchiveEntry> _entries = new();
    private readonly FileStream _fileStream;

    public ModPackage(string zipFilePath)
    {
        _fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
        _archive = new ZipArchive(_fileStream, ZipArchiveMode.Read);

        // 缓存所有条目路径（小写优化查找）
        foreach (var entry in _archive.Entries) _entries[entry.FullName.ToLowerInvariant()] = entry;

        LoadManifest();
    }

    public LModDesc? ModDesc { get; private set; }

    public void Dispose()
    {
        _archive?.Dispose();
        _fileStream?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void LoadManifest()
    {
        using var manifestStream = GetFileStream("manifest.json");
        using var reader = new StreamReader(manifestStream);
        var json = reader.ReadToEnd();

        ModDesc = JsonConvert.DeserializeObject<LModDesc>(json);
    }

    public Stream GetFileStream(string relativePath)
    {
        var normalizedPath = relativePath.Replace('\\', '/').ToLowerInvariant();

        if (_entries.TryGetValue(normalizedPath, out var entry)) return entry.Open();

        throw new FileNotFoundException($"File not found in mod package: {relativePath}");
    }

    public byte[] ReadBinaryFile(string relativePath)
    {
        using var stream = GetFileStream(relativePath);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public string ReadTextFile(string relativePath)
    {
        using var stream = GetFileStream(relativePath);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}