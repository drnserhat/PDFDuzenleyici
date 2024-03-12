namespace PDFDuzenleyici.Models
{
    public class UploadYaziViewModel
    {
        public string WatermarkText { get; set; }
        public int YaziBoyutu { get; set;}
        public string YaziRengi { get; set;}
        public string YaziStili { get; set;}
        public string YaziAilesi { get; set;}
        public int Transparanlik { get; set; }
        public string WatermarkMetinYerlesimi { get; set; }
    }
    public class UploadFotoViewModel
    {
        public IFormFile UploadFoto { get; set; }
        public int FotografYatayBoyutu { get; set; }
        public int FotografDikeyBoyutu { get; set; }
        public int Transparanlik1 { get; set; }
        public string MetinYerlesimi { get; set; }


    }
}
