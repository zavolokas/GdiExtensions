using System.Drawing;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Utils.Processes;

namespace MergeImage
{
    class Program
    {
        static void Main(string[] args)
        {
            const string resultPath = @"..\..\out.png";

            using (var bitmap1 = new Bitmap(@"..\..\..\Images\m015.png"))
            using (var bitmap2 = new Bitmap(@"..\..\..\Images\m016.png"))
            {
                bitmap1.MegreWithImage(bitmap2);
                bitmap1.SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
