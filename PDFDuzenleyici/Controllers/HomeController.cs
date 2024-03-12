
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using System.Drawing;
using System.Globalization;
using System.Text;
using PDFDuzenleyici.Models;
using PdfSharp.Pdf.IO;
using PDFDuzenleyici.Service;
using System.Drawing.Imaging;

namespace PDFDuzenleyici.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View(new ContentViewModel());
        }


        #region WATERMARK TEXT
        public XColor ParseARGB(string argbCode,int alpha)
        {
            if (argbCode.Length != 7 || !argbCode.StartsWith("#"))
            {
                return XColor.Empty;
            }

            alpha = alpha*25;
            int red = Convert.ToInt32(argbCode.Substring(1, 2), 16);
            int green = Convert.ToInt32(argbCode.Substring(3, 2), 16);
            int blue = Convert.ToInt32(argbCode.Substring(5, 2), 16);

            return XColor.FromArgb(alpha, red, green, blue);
        }

        public void ApplyWatermark(XGraphics gfx, string watermarkText, XFont font, XBrush brush,string yon)
        {
            if (!string.IsNullOrWhiteSpace(watermarkText))
            {
                XGraphicsState state = gfx.Save();

                double pageWidth = gfx.PageSize.Width;
                double pageHeight = gfx.PageSize.Height;

                XSize textSize = gfx.MeasureString(watermarkText, font);
                double centerX = (pageWidth - textSize.Width) / 2;
                double centerY = (pageHeight - textSize.Height) / 2;

                gfx.TranslateTransform(centerX, centerY);
                if (yon=="saga")
                {
                    gfx.RotateTransform(45);

                }
                else if (yon=="sola")
                {
                    gfx.RotateTransform(-45);

                }
                gfx.TranslateTransform(-centerX, -centerY);



                gfx.DrawString(watermarkText, font, brush, new XPoint(centerX, centerY));

                gfx.Restore(state);

            }
        }

        [HttpPost]
        public IActionResult UploadText(IFormFile uploadedFile, UploadYaziViewModel model)
        {
            if (uploadedFile != null)
            {
                // PDF dosyasını işlemek için PdfDocument nesnesini kullanın
                using (var document = PdfReader.Open(uploadedFile.OpenReadStream(), PdfDocumentOpenMode.Modify))
                {
                    foreach (var page in document.Pages)
                    {
                        using (var gfx = XGraphics.FromPdfPage(page))
                        {
                            var watermarkFont = new XFont(model.YaziAilesi, model.YaziBoyutu, StilMethod.YaziStil(model.YaziStili));
                            string yaziRengi = model.YaziRengi;

                            System.Drawing.Color color = System.Drawing.Color.FromName(yaziRengi);

                            //XColor xColor = XColor.FromArgb(50, color.A,color.R, color.G);
                               XColor xColor = ParseARGB(model.YaziRengi,model.Transparanlik);
                            var watermarkBrush = new XSolidBrush(xColor);
                         

                            ApplyWatermark(gfx, model.WatermarkText, watermarkFont, watermarkBrush,model.WatermarkMetinYerlesimi);
                        }
                    }

                    // PDF'i tarayıcıya gönder
                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream, false);
                        var content = stream.ToArray();
                        return File(content, "application/pdf", "output.pdf");
                    }
                }
            }
            return View();
        }

        #endregion

        #region WATERMARK FOTO
        [HttpPost]
        public IActionResult UploadFoto(IFormFile uploadedFile, UploadFotoViewModel model)
        {
            if (uploadedFile != null)
            {
                using (var document = PdfReader.Open(uploadedFile.OpenReadStream(), PdfDocumentOpenMode.Modify))
                {
                    foreach (var page in document.Pages)
                    {
                        using (var gfx = XGraphics.FromPdfPage(page))
                        {

                           
                            ApplyWatermarkImage(gfx,model.FotografYatayBoyutu,model.FotografDikeyBoyutu ,ChangeOpacity(model.UploadFoto,OpacityMethod.Opacity(model.Transparanlik1)),model.MetinYerlesimi); // 0.5 değeri saydamlık seviyesini belirler

                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        document.Save(stream, false);
                        var content = stream.ToArray();
                        return File(content, "application/pdf", "output.pdf");
                    }
                }
            }
            return View();
        }

        public void ApplyWatermarkImage(XGraphics gfx, double YatayBoyut, double DikeyBoyut, IFormFile watermarkImage,string yon)
        {
            if (watermarkImage != null)
            {
                using (var imageStream = watermarkImage.OpenReadStream())
                {
                    XImage image = XImage.FromStream(imageStream);

                    // Yeni boyutları hesapla
                    double imageWidth = YatayBoyut; // Yatay boyut (genişlik)
                    double imageHeight = DikeyBoyut; // Dikey boyut (yükseklik)


                    // Resmi konumlandırma
                    double centerX = gfx.PageSize.Width / 2 - imageWidth / 2;
                    double centerY = gfx.PageSize.Height / 2 - imageHeight / 2;
                    if (yon=="sola")
                    {
                        gfx.RotateAtTransform(-45, new XPoint(centerX + imageWidth / 2, centerY + imageHeight / 2));

                    }else if (yon=="saga")
                    {
                        gfx.RotateAtTransform(45, new XPoint(centerX + imageWidth / 2, centerY + imageHeight / 2));

                    }

                    gfx.DrawImage(image, centerX, centerY, imageWidth, imageHeight);
                }
            }
        }


        public IFormFile ChangeOpacity(IFormFile inputImage, double opacity)
        {
            var imageStream = new MemoryStream(); // Akışı burada oluşturun

            using (var image = System.Drawing.Image.FromStream(inputImage.OpenReadStream()))
            using (var bitmap = new Bitmap(image.Width, image.Height))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(System.Drawing.Color.Transparent);

                // Opacity değerini 0 ile 255 arasına dönüştürün.
                int alpha = (int)(opacity * 255);

                // Renk matrisini ayarlayın
                var colorMatrix = new System.Drawing.Imaging.ColorMatrix();
                colorMatrix.Matrix33 = alpha / 255f;

                // İşlem için ImageAttributes oluşturun
                var alphaAttributes = new ImageAttributes();
                alphaAttributes.SetColorMatrix(colorMatrix);

                // Resmi çizin
                graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, alphaAttributes);

                // İşlem tamamlandığında alphaAttributes'i serbest bırakın.
                alphaAttributes.Dispose();
                bitmap.Save(imageStream, ImageFormat.Png); // Saydamlığı desteklemesi için PNG formatını kullanıyoruz

            }

            // Değiştirilmiş resmi imageStream'e kaydedin

            return new FormFile(imageStream, 0, imageStream.Length, null, Path.GetFileName(inputImage.FileName));
        }


        #endregion


    }
}

