using System.IO;
using Microsoft.CodeAnalysis;

namespace Clangen.ViewModels;
using Avalonia.Media.Imaging;
using SkiaSharp;

public static class ImageExtensions
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