namespace TactiX_ModSupport;

public class ModResourceCache<TImageT>
{
    public delegate TImageT ImageTConvDelegate(MemoryStream memoryStream);

    private readonly Dictionary<string, byte[]> _audioCache = new();
    private readonly Dictionary<string, TImageT> _imageCache = new();
    private readonly ImageTConvDelegate _imageTConv;
    private readonly ModPackage _package;

    public ModResourceCache(ModPackage package, ImageTConvDelegate imageTConv)
    {
        _package = package;
        _imageTConv = imageTConv;
    }

    public TImageT GetImage(string path)
    {
        if (!_imageCache.TryGetValue(path, out var image))
        {
            var bytes = _package.ReadBinaryFile(path);
            image = _imageTConv.Invoke(new MemoryStream(bytes));
            _imageCache[path] = image;
        }

        return image;
    }

    public byte[] GetAudio(string path)
    {
        if (!_audioCache.TryGetValue(path, out var audio))
        {
            audio = _package.ReadBinaryFile(path);
            _audioCache[path] = audio;
        }

        return audio;
    }
}