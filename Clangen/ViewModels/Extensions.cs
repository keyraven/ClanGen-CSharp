using System.IO;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Clangen.ViewModels;

public static class Extensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this SKImage source)
    {
        SKData encode = source.Encode();
        
        Stream memoryStream = encode.AsStream();
        Bitmap output = new Bitmap(memoryStream);
        memoryStream.Dispose();

        return output;
    }
    
    
}