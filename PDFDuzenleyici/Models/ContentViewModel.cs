namespace PDFDuzenleyici.Models
{
    public class ContentViewModel
    {
        public string Baslik { get; set; }
        public string Yazi { get; set; }
        public string ArkaPlanYazisi { get; set; }
        public IFormFile ArkaPlanFoto {  get; set; }
    }
}
