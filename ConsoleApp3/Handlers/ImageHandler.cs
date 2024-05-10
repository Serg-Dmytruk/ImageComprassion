using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ConsoleApp3.Models;
using ConsoleApp3.Options;
using static System.Drawing.Graphics;

namespace ConsoleApp3.Handlers;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class ImageHandler
{
    public static void SaveImages(List<Image> images)
    {
        foreach (var image in images)
            SetResolution(image);
    }

    public static void SaveImage(Image image)
    {
        SetResolution(image);
    }

    private static void SetResolution(Image image, bool compressionRequired = true)
    {
        foreach (var resolution in AllowedResolutions.Resolutions)
        {
            var scale = GetScale(image, resolution);
            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            using var imageToSave = new Bitmap(newWidth, newHeight);
            using var graphics = FromImage(imageToSave);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graphics.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight));
            imageToSave.Save(Path.Combine(ImageRouts.OutRout, GenerateName(resolution, "beforeCompression")));

            if (compressionRequired)
            {
                CompressImage(imageToSave, resolution);
            }
        }
    }

    #region scale

    private static double GetScale(Image image, Resolution resolution)
    {
        var scaleFactorWidth = (double)resolution.Width / image.Width;
        var scaleFactorHeight = (double)resolution.Height / image.Height;

        return Math.Min(scaleFactorWidth, scaleFactorHeight);
    }

    #endregion

    #region compression

    private static void CompressImage(Image image, Resolution resolution)
    {
        var jpgEncoder = GetEncoder(ImageFormat.Jpeg);

        if (jpgEncoder == null)
        {
            image.Save(Path.Combine(ImageRouts.OutRout));
        }
        else
        {
            var qualityEncoder = Encoder.Quality;
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, 50L);

            image.Save(Path.Combine(ImageRouts.OutRout, GenerateName(resolution, "afterCompression")), jpgEncoder, encoderParameters);
        }
    }

    private static ImageCodecInfo? GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageDecoders();
        return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }

    #endregion

    private static string GenerateName(Resolution resolution, string additionalPrefix) =>
        $"{Guid.NewGuid().ToString()}{resolution.Prefix}{additionalPrefix}.jpg";
}