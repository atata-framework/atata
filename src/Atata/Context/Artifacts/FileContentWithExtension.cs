namespace Atata;

/// <summary>
/// Represents the file content item with file extension.
/// </summary>
public abstract class FileContentWithExtension
{
    protected FileContentWithExtension(string extension)
    {
        Guard.ThrowIfNullOrWhitespace(extension);

        Extension = extension;

        if (Extension[0] != '.')
            Extension = '.' + Extension;
    }

    /// <summary>
    /// Gets the file extension.
    /// </summary>
    public string Extension { get; }

    public static FileContentWithExtension CreateFromText(string text, string extension)
    {
        Guard.ThrowIfNull(text);
        Guard.ThrowIfNullOrWhitespace(extension);

        return new TextFileContentWithExtension(text, extension);
    }

    public static FileContentWithExtension CreateFromByteArray(byte[] bytes, string extension)
    {
        Guard.ThrowIfNull(bytes);
        Guard.ThrowIfNullOrWhitespace(extension);

        return new ByteArrayFileContentWithExtension(bytes, extension);
    }

    public static FileContentWithExtension CreateFromBase64String(string base64String, string extension)
    {
        Guard.ThrowIfNull(base64String);
        Guard.ThrowIfNullOrWhitespace(extension);

        var bytes = Convert.FromBase64String(base64String);

        return new ByteArrayFileContentWithExtension(bytes, extension);
    }

    internal abstract void Save(string absoluteFilePath);

    internal abstract string ToBase64String();

    private sealed class TextFileContentWithExtension : FileContentWithExtension
    {
        private readonly string _text;

        public TextFileContentWithExtension(string text, string extension)
            : base(extension) =>
            _text = text;

        internal override void Save(string absoluteFilePath) =>
            File.WriteAllText(absoluteFilePath, _text);

        internal override string ToBase64String()
        {
            var bytes = Encoding.Unicode.GetBytes(_text);
            return Convert.ToBase64String(bytes);
        }
    }

    private sealed class ByteArrayFileContentWithExtension : FileContentWithExtension
    {
        private readonly byte[] _bytes;

        public ByteArrayFileContentWithExtension(byte[] bytes, string extension)
            : base(extension) =>
            _bytes = bytes;

        internal override void Save(string absoluteFilePath) =>
            File.WriteAllBytes(absoluteFilePath, _bytes);

        internal override string ToBase64String() =>
            Convert.ToBase64String(_bytes);
    }
}
