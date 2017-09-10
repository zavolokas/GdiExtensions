using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Utils.Processes;

namespace ScaleBitmap
{
    class Program
    {
        static void Main(string[] args)
        {
            const string resultPath = @"..\..\out.png";

            using (var bitmap = new Bitmap(@"..\..\..\Images\t023.jpg"))
            using (var scaled = bitmap.CloneWithScaleTo(1000, 300, InterpolationMode.Low))
            {
                scaled
                    .SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
