using PdfSharp.Fonts;
using System.Reflection;

public class SimpleFontResolver : IFontResolver
{
    private readonly byte[] _fontData;

    public SimpleFontResolver()
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream("PDMSRestServices.Fonts.OpenSans-Regular.ttf");
        using var ms = new MemoryStream();

        stream.CopyTo(ms);
        _fontData = ms.ToArray();
    }

    public byte[] GetFont(string faceName)
    {
        return _fontData;
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // ✅ Ignore requested font and always use OpenSans
        return new FontResolverInfo("OpenSans");
    }
}