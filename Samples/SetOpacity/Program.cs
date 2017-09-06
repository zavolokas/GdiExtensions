using System.Drawing;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Utils.Processes;

namespace SetOpacity
{
    class Program
    {
        static void Main(string[] args)
        {
            const string resultPath = @"..\..\out.png";

            using (var image = new Bitmap(@"..\..\..\Images\t023.jpg"))
            {
                image
                    .SetOpacity(0.3f)
                    .SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
