using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SenticodeTemplate.Services.Helpers
{
    internal static class PngImageHelper
    {
        public static void ScaleAndSave(string sourcePath, string targetPath, double factor)
        {
            using var sourceImage = Image.FromFile(sourcePath);
            var newWidth = (int) (sourceImage.Width * factor);
            var newHeight = (int) (sourceImage.Height * factor);
            SaveImageWithNewSize(targetPath, newWidth, newHeight, sourceImage);
        }

        public static void ResizeAndSave(string sourcePath, string targetPath, int size)
        {
            using var sourceImage = Image.FromFile(sourcePath);
            SaveImageWithNewSize(targetPath, size, size, sourceImage);
        }

        public static void ResizeWithFillAndSave(string sourcePath, string targetPath, int size, Color background)
        {
            using var sourceImage = Image.FromFile(sourcePath);
            SaveImageWithNewSize(targetPath, size, size, sourceImage, background);
        }

        private static void SaveImageWithNewSize(string targetPath, int newWidth, int newHeight, Image sourceImage,
            Color? background = null)
        {
            using var targetImage = new Bitmap(newWidth, newHeight);
            using var graphics = Graphics.FromImage(targetImage);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (background != null)
            {
                graphics.Clear(background.Value);
            }

            graphics.DrawImage(sourceImage, new Rectangle(0, 0, newWidth, newHeight));
            targetImage.Save(targetPath, ImageFormat.Png);
        }
    }
}