using Epichurco.Benj8RIDES;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            string rutaTemplateXML = @"C:\Users\guido\Desktop\CerverceriaSabai\formatosfacturas\test.xml";
            string rutaLogo = @"C:\Users\guido\Desktop\CerverceriaSabai\formatosfacturas\LogoPdf.jpg";
            string rutaSalidaPdf = @"C:\Users\guido\Desktop\CerverceriaSabai\formatosfacturas\";


            Generador generaPDF = new Generador();

            if (generaPDF.ReportePDFGenerico(rutaTemplateXML,rutaLogo,rutaSalidaPdf))
                System.Console.WriteLine("Se generó el PDF");
            else
                System.Console.WriteLine("Error PDF");

            System.Console.ReadKey();

        }
    }
}

