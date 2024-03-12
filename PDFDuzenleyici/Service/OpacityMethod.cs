namespace PDFDuzenleyici.Service
{
    public static class OpacityMethod
    {
        public static double Opacity(int opa)
        {
            switch (opa)
            {
                case 1:
                    return 0.1;
                case 2:
                    return 0.2;
                case 3:
                    return 0.3;
                case 4:
                    return 0.4;
                case 5:
                    return 0.5;
                case 6:
                    return 0.6;
                case 7:
                    return 0.7;
                case 8:
                    return 0.8;
                case 9:
                    return 0.9;
                case 10:
                    return 1.0; 
                default:
                    return 0.0; 
            }
        }

    }
}
