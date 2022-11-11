using System.IO;

namespace Atata
{
    /// <summary>
    /// Represents the file content item with file extension.
    /// </summary>
    public abstract class FileContentWithExtension
    {
        protected FileContentWithExtension(string extension)
        {
            Extension = extension.CheckNotNullOrWhitespace(nameof(extension));

            if (Extension[0] != '.')
                Extension = '.' + Extension;
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        public string Extension { get; }

        public static FileContentWithExtension CreateFromText(string text, string extension)
        {
            text.CheckNotNull(nameof(text));
            extension.CheckNotNullOrWhitespace(nameof(extension));

            return new TextFileContentWithExtension(text, extension);
        }

        public static FileContentWithExtension CreateFromByteArray(byte[] bytes, string extension)
        {
            bytes.CheckNotNull(nameof(bytes));
            extension.CheckNotNullOrWhitespace(nameof(extension));

            return new ByteArrayFileContentWithExtension(bytes, extension);
        }

        internal abstract void Save(string absoluteFilePath);

        private sealed class TextFileContentWithExtension : FileContentWithExtension
        {
            private readonly string _text;

            public TextFileContentWithExtension(string text, string extension)
                : base(extension) =>
                _text = text;

            internal override void Save(string absoluteFilePath) =>
                File.WriteAllText(absoluteFilePath, _text);
        }

        private sealed class ByteArrayFileContentWithExtension : FileContentWithExtension
        {
            private readonly byte[] _bytes;

            public ByteArrayFileContentWithExtension(byte[] bytes, string extension)
                : base(extension) =>
                _bytes = bytes;

            internal override void Save(string absoluteFilePath) =>
                File.WriteAllBytes(absoluteFilePath, _bytes);
        }
    }
}
