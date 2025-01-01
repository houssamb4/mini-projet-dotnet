/*namespace Partie_Font_Office.Helpers
{
    using PdfSharp.Fonts;
    using System.IO;
    using System.Reflection;

    public class CustomFontResolver : IFontResolver
    {
        public string DefaultFontName => "Arial";

        public byte[] GetFont(string faceName)
        {
            string resourceName = faceName switch
            {
                "Arial" => "YourNamespace.Resources.Fonts.Arial.ttf",
                _ => throw new ArgumentException($"Font '{faceName}' is not available.")
            };

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Font file not found: {resourceName}");
                }

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Arial");
            }

            throw new ArgumentException($"Font '{familyName}' is not available.");
        }
    }

}
*/