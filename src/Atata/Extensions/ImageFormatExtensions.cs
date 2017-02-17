using System.Drawing.Imaging;
using System.Linq;

namespace Atata
{
    public static class ImageFormatExtensions
    {
        public static string GetExtension(this ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().
                First(x => x.FormatID == format.Guid).
                FilenameExtension.
                Split(';').
                First().
                TrimStart('*').
                ToLower();
        }
    }
}
