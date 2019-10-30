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
            const string resultPath = @"..\..\..\out.png";

            using (var dest = new Bitmap(@"..\..\..\..\Images\m015.png"))
            using (var source = new Bitmap(@"..\..\..\..\Images\m016.png"))
            {
                dest
                    .DrawImageWithFit(source)
                    .SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
