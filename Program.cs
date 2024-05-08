// See https://aka.ms/new-console-template for more information
using FreeImageAPI.Metadata;
using FreeImageAPI;

Console.WriteLine("Hello, World!");
ClearMetadata("Original.jpg", "Stripped.jpg");

public partial class Program
{
    public static bool ClearMetadata(string inputFile, string outputFile)
    {
        using (var fstream = File.OpenRead(inputFile))
            return ClearMetadata(fstream, outputFile);
    }

    public static bool ClearMetadata(Stream data, string outputFile)
    {
        FREE_IMAGE_FORMAT format = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
        FIBITMAP dib = FreeImage.LoadFromStream(data, ref format);

        if (format == FREE_IMAGE_FORMAT.FIF_UNKNOWN)
            throw new Exception("Couldn't decode the image data");

        // Information from 2008: https://sourceforge.net/p/freeimage/discussion/36111/thread/2d087f91/

        ImageMetadata imageMetadata = new ImageMetadata(dib, true, true);
        var model = imageMetadata[FREE_IMAGE_MDMODEL.FIMD_EXIF_RAW];
        var success = model.DestroyModel();
        
        using (var fStream = File.OpenWrite(outputFile))
            FreeImage.SaveToStream(dib, fStream, format);

        FreeImage.Unload(dib);

        return success;
    }
}
