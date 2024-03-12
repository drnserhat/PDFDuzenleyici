namespace PDFDuzenleyici.Models
{
    public class WaterMarkModel
    {
        public string ArkaPlanYazisi { get; set; }
        public IFormFile ArkaPlanFoto { get; set; }

        public WaterMarkModel(string _ArkaPlanYazisi,IFormFile _ArkaPlanFoto) {
            ArkaPlanFoto = _ArkaPlanFoto;
            ArkaPlanYazisi = _ArkaPlanYazisi;
        }
    }
}
