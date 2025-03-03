using System.IO;

namespace ModernBaseLibrary.Graphics
{
    public class ImageInfo
    {
        public enum ImageInfoType
        {
            Unknown = 0,
            Bmp,
            Gif,
            Jpeg,
            Png,
            Tiff
        }

        public static ImageInfoBase GetInfo(ImageInfoType type, Stream stream)
        {
            ImageInfoBase info;

            switch (type)
            {
                case ImageInfoType.Bmp:
                    info = new BmpInfo();
                    break;
                case ImageInfoType.Gif:
                    info = new GifInfo();
                    break;
                case ImageInfoType.Jpeg:
                    info = new JpegInfo();
                    break;
                case ImageInfoType.Png:
                    info = new PngInfo();
                    break;
                case ImageInfoType.Tiff:
                    info = new TiffInfo();
                    break;
                default:
                    return null;
            }

            info.Init(stream);
            return info;
        }

        public static ImageInfoType GetType(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (extension == null)
            {
                return ImageInfoType.Unknown;
            }

            switch (extension.ToLower())
            {
                case ".bmp":
                    return ImageInfoType.Bmp;
                case ".gif":
                    return ImageInfoType.Gif;
                case ".jpg":
                case ".jpeg":
                    return ImageInfoType.Jpeg;
                case ".png":
                    return ImageInfoType.Png;
                case ".tif":
                case ".tiff":
                    return ImageInfoType.Tiff;
            }

            return ImageInfoType.Unknown;
        }
    }
}