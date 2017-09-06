using System.Drawing;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Utils.Processes;

namespace CopyChannel
{
    class Program
    {
        static void Main(string[] args)
        {
            const string resultPath = @"..\..\out.png";

            using (var shape = new Bitmap(@"..\..\..\Images\m023.png"))
            using (var image = new Bitmap(@"..\..\..\Images\t023.jpg"))
            {
                image.CopyChannel(2, shape, 3);

                image.SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
