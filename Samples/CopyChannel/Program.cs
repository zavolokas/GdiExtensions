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
            const string resultPath = @"..\..\..\out.png";

            using (var source = new Bitmap(@"..\..\..\..\Images\m023.png"))
            using (var dest = new Bitmap(@"..\..\..\..\Images\t023.jpg"))
            {
                const int dstChannelIndex = 2;
                const int srcChannelIndex = 3;

                dest.CopyChannel(dstChannelIndex, source, srcChannelIndex)
                    .SaveTo(resultPath, ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
