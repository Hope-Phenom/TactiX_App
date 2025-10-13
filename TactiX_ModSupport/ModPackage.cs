using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TactiX_Models.Tactics;

namespace TactiX_ModSupport;

public class ModPackage : IDisposable
{
    private ZipArchive _archive;
    private FileStream _fileStream;
    private readonly Dictionary<string, ZipArchiveEntry> _entries = new();

    public L_ModDesc? ModDesc { get; private set; }

    public ModPackage(string zipFilePath)
    {
        _fileStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read);
        _archive = new ZipArchive(_fileStream, ZipArchiveMode.Read);

        // 缓存所有条目路径（小写优化查找）
        foreach (var entry in _archive.Entries)
        {
            _entries[entry.FullName.ToLowerInvariant()] = entry;
        }

        LoadManifest();
    }

    private void LoadManifest()
    {
        using var manifestStream = GetFileStream("manifest.json");
        using var reader = new StreamReader(manifestStream);
        var json = reader.ReadToEnd();

        ModDesc = JsonConvert.DeserializeObject<L_ModDesc>(json);
    }

    public Stream GetFileStream(string relativePath)
    {
        var normalizedPath = relativePath.Replace('\\', '/').ToLowerInvariant();

        if (_entries.TryGetValue(normalizedPath, out var entry))
        {
            return entry.Open();
        }

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

    public void Dispose()
    {
        _archive?.Dispose();
        _fileStream?.Dispose();
        GC.SuppressFinalize(this);
    }
}