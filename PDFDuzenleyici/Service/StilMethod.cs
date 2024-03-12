using PdfSharp.Drawing;

namespace PDFDuzenleyici.Service
{
    public enum Stiller
    {
        Bold,
        Italic,
        Underline,
        Strikeout,
        UnderlineItalic
    }

    public static class StilMethod
    {
        public static XFontStyle YaziStil(string stiller)
        {
            switch (stiller)
            {
                case nameof(Stiller.Bold):
                    return XFontStyle.Bold;
                case nameof(Stiller.Italic):
                    return XFontStyle.Italic;
                case nameof(Stiller.Underline):
                    return XFontStyle.Underline;
                case nameof(Stiller.Strikeout):
                    return XFontStyle.Strikeout;
                case nameof(Stiller.UnderlineItalic):
                    return XFontStyle.Underline | XFontStyle.Italic;
                default:
                    // Varsayılan olarak Normal stilini döndür
                    return XFontStyle.Regular;
            }
        }
    }
}
