using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Zavolokas.GdiExtensions
{
    public static class BitmapExtensions
    {
        /// <summary>
        /// Scales an image to provided size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="interpolation">The interpolation.</param>
        /// <returns></returns>
        public static T CloneWithScaleTo<T>(this T image, int width, int height, InterpolationMode interpolation = InterpolationMode.Default)
            where T: Image
        {
            var result = new Bitmap(width, height);
            using (var g = Graphics.FromImage(result))
            {
                g.InterpolationMode = interpolation;
                g.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return (T)(Image)result;
        }

        /// <summary>
        /// Saves to.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="path">The path.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static FileInfo SaveTo(this Image image, string path, ImageFormat format)
        {
            image.Save(path, format);
            return new FileInfo(path);
        }

        /// <summary>
        /// Draws provided image with fit to the current one.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="source"></param>
        public static T DrawImageWithFit<T>(this T image, T source)
            where T : Image
        {
            using (var gr = Graphics.FromImage(image))
            {
                gr.DrawImage(source, new Rectangle(0, 0, image.Width, image.Height));
            }
            return image;
        }

        /// <summary>
        /// Copys one of the ARGB channels from source bitmap to one of the ARGB channels of dest bitmap.
        /// </summary>
        /// <param name="channelSource">Bitmap with a source RGBA channel.</param>
        /// <param name="dest">Destination bitmap.</param>
        /// <param name="sourceChannel">Index of a source channel to copy. 3 - Alpha.</param>
        /// <param name="destChannel">Index of a dest channel. 3 - Alpha.</param>
        public static Bitmap CopyChannel(this Bitmap dest, int destChannel, Bitmap channelSource, int sourceChannel)
        {
            if (channelSource.Size != dest.Size)
                throw new ArgumentException();

            var r = new Rectangle(Point.Empty, channelSource.Size);
            var bdSrc = channelSource.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bdDst = dest.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* bpSrc = (byte*)bdSrc.Scan0.ToPointer();
                byte* bpDst = (byte*)bdDst.Scan0.ToPointer();
                bpSrc += sourceChannel;
                bpDst += destChannel;
                for (int i = r.Height * r.Width; i > 0; i--)
                {
                    *bpDst = *bpSrc;
                    bpSrc += 4;
                    bpDst += 4;
                }
            }
            channelSource.UnlockBits(bdSrc);
            dest.UnlockBits(bdDst);
            return dest;
        }

        /// <summary>
        /// Clones the image and sets the opacity to the specefied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="image">The image.</param>
        /// <param name="opacity">The opacity.</param>
        /// <returns></returns>
        public static T CloneWithOpacity<T>(this T image, float opacity)
            where T : Image
        {
            var colorMatrix = new ColorMatrix { Matrix33 = opacity };
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            var output = new Bitmap(image.Width, image.Height);
            using (var gfx = Graphics.FromImage(output))
            {
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.DrawImage(
                    image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }
            return (T)(Image)output;
        }

        /// <summary>
        /// Gets the bytes of the image in a specified image format(JPEG by default).
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this Image image, ImageFormat format = null)
        {
            byte[] result;
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format ?? ImageFormat.Jpeg);
                result = ms.ToArray();
            }
            return result;
        }

        public static Bitmap GetDiff(this Bitmap bitmap1, Bitmap bitmap2)
        {
            var differenceBitmap = new Bitmap(bitmap1.Width, bitmap1.Height, PixelFormat.Format32bppArgb);
            var bitmap1Data = bitmap1.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadOnly, bitmap1.PixelFormat);
            var bitmap2Data = bitmap2.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.ReadOnly, bitmap1.PixelFormat);
            var differenceBitmapData = differenceBitmap.LockBits(new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), ImageLockMode.WriteOnly, bitmap1.PixelFormat);

            try
            {
                for (var y = 0; y < bitmap1.Height; y++)
                {
                    unsafe
                    {
                        byte* row1 = (byte*)bitmap1Data.Scan0 + y * bitmap1Data.Stride;
                        byte* row2 = (byte*)bitmap2Data.Scan0 + y * bitmap2Data.Stride;
                        byte* diff = (byte*)differenceBitmapData.Scan0 + y * differenceBitmapData.Stride;

                        for (var x = 0; x < bitmap1Data.Stride; x += 4)
                        {
                            var da = (byte)Math.Abs(row1[x] - row2[x]);
                            var dr = (byte)Math.Abs(row1[x + 1] - row2[x + 1]);
                            var dg = (byte)Math.Abs(row1[x + 2] - row2[x + 2]);
                            var db = (byte)Math.Abs(row1[x + 3] - row2[x + 3]);

                            if (da + dr + dg + db > 0)
                            {
                                byte full = 255;
                                diff[x + 0] = (byte)(full - dr);
                                diff[x + 1] = (byte)(full - dg);
                                diff[x + 2] = (byte)(full - db);
                                diff[x + 3] = full;
                            }
                            else
                            {
                                byte color = 255;
                                diff[x + 0] = color;
                                diff[x + 1] = color;
                                diff[x + 2] = color;
                                diff[x + 3] = color;
                            }
                        }
                    }
                }
            }
            finally
            {
                bitmap1.UnlockBits(bitmap1Data);
                bitmap2.UnlockBits(bitmap2Data);
                differenceBitmap.UnlockBits(differenceBitmapData);
            }

            return differenceBitmap;
        }

        
    }
}
