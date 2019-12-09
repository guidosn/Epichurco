using Epichurco.Benj8RIDES;
using System.Collections.Generic;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //*** Ruta al escritorio del usuario ***//
            //var file1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            //*** Directorio actual de la aplicación ***//
            //string directorio = Environment.CurrentDirectory;

            AutorizacionFacturas a = new AutorizacionFacturas();
            //leo del sri
            DatosComprobante dt = a.ConsultarAutorizaciones("THOMAS0111ws");
            Comprobante documento = new Comprobante();
            documento.detalles = new List<DetalleComprobante>();

            dt.comprobante = "";
            dt.numeroAutorizacion = "2808201901179174314800120010010002033920020339210";
            dt.ambiente = "PRODUCCION";
            dt.mensaje = "NORMAL";
            documento.archivo = @"C:\Users\guido\Desktop\CerverceriaSabai\LogoComputerEC.png";
            documento.claveAcceso = "2808201901179174314800120010010002033920020339210";
            dt.Ruc = "1714194451001";
            documento.codDoc = "001-001-000203392";
            documento.nombreComercial = "ComputerEC";
            documento.direccion = "Homero Salas OE5-148 y Altar";

            documento.razonSocial = "Pablo Rueda";
            documento.fechaEmision = "11-09-2019";
            documento.ruc = "1001001001001";

            DetalleComprobante linea = new DetalleComprobante();
            linea.impuestos = new List<DetalleImpuesto>();

            linea.codigoPrincipal = "001";
            linea.codigoAuxiliar = "001";
            linea.cantidad = 1;
            linea.descripcion = "Computadora Portátil Marca DELL";
            linea.detalleadicional1 = "CompDell";
            linea.detalleadicional2 = "CompDell";
            linea.detalleadicional3 = "";
            linea.precioUnitario = (decimal)500.00f;
            linea.descuento = 0;
            linea.totalSinImpueto = linea.precioUnitario * linea.cantidad;
            DetalleImpuesto detalleimpuesto = new DetalleImpuesto();
            detalleimpuesto.tarifa = 12;
            detalleimpuesto.valor = (linea.precioUnitario * detalleimpuesto.tarifa) / 100;
            linea.impuestos.Add(detalleimpuesto);

            documento.detalles.Add(linea);

            DetalleComprobante linea1 = new DetalleComprobante();
            linea1.impuestos = new List<DetalleImpuesto>();

            linea1.codigoPrincipal = "002";
            linea1.codigoAuxiliar = "002";
            linea1.cantidad = 2;
            linea1.descripcion = "Disco duro externo 2,5 1TB";
            linea1.detalleadicional1 = "Disc1TB";
            linea1.detalleadicional2 = "Disc1TB";
            linea1.detalleadicional3 = "";
            linea1.precioUnitario = (decimal)60.56f;
            linea1.descuento = 0;
            linea1.totalSinImpueto = linea1.precioUnitario * linea1.cantidad;
            DetalleImpuesto detalleimpuesto1 = new DetalleImpuesto();
            detalleimpuesto1.tarifa = 12;
            detalleimpuesto1.valor = (linea1.precioUnitario * detalleimpuesto1.tarifa) / 100;
            linea.impuestos.Add(detalleimpuesto1);

            documento.detalles.Add(linea1);

            DetalleComprobante linea2 = new DetalleComprobante();
            linea2.impuestos = new List<DetalleImpuesto>();

            linea2.codigoPrincipal = "003";
            linea2.codigoAuxiliar = "003";
            linea2.cantidad = 3;
            linea2.descripcion = "Monitor de 19.5";
            linea2.detalleadicional1 = "MONI195";
            linea2.detalleadicional2 = "MONI195";
            linea2.detalleadicional3 = "";
            linea2.precioUnitario = (decimal)60.56f;
            linea2.descuento = 0;
            linea2.totalSinImpueto = linea2.precioUnitario * linea1.cantidad;
            DetalleImpuesto detalleimpuesto2 = new DetalleImpuesto();
            detalleimpuesto2.tarifa = 12;
            detalleimpuesto2.valor = (linea2.precioUnitario * detalleimpuesto2.tarifa) / 100;
            linea2.impuestos.Add(detalleimpuesto2);

            documento.detalles.Add(linea2);

            DetalleComprobante linea3 = new DetalleComprobante();
            linea3.impuestos = new List<DetalleImpuesto>();

            linea3.codigoPrincipal = "004";
            linea3.codigoAuxiliar = "004";
            linea3.cantidad = 3;
            linea3.descripcion = "Monitor de 20.5";
            linea3.detalleadicional1 = "MONI205";
            linea3.detalleadicional2 = "MONI205";
            linea3.detalleadicional3 = "";
            linea3.precioUnitario = (decimal)60.56f;
            linea3.descuento = 0;
            linea3.totalSinImpueto = linea3.precioUnitario * linea3.cantidad;
            DetalleImpuesto detalleimpuesto3 = new DetalleImpuesto();
            detalleimpuesto3.tarifa = 12;
            detalleimpuesto3.valor = (linea3.precioUnitario * detalleimpuesto3.tarifa) / 100;
            linea2.impuestos.Add(detalleimpuesto3);

            documento.detalles.Add(linea3);


            //*** Subtotales de la factura
            documento.importeTotal = 621.12m;
            documento.totalSinImpuetos = 621.12m;
            decimal ivatotal;
            ivatotal = detalleimpuesto.valor + detalleimpuesto2.valor;
            documento.totalConImpuestos = documento.totalSinImpuetos + ivatotal;

            //*** Datos de información adicional ***//

            //** Crea el estampado de los datos en la plantilla PDF 
            //** factura-1-formato-cabecera.pdf (archivo de plantilla)
            //** factura-1-formato-totales.pdf  (archivo de plantilla)
            //** genera el documento : archivotemporal.pdf  totales_temporal
            // Properties.Settings.Default.PathDefecto + @"generadasfacturas\" + nombrepdf.pdf;

            string rutaPDF = @"C:\Users\guido\Desktop\CerverceriaSabai\generadasfacturas\";
            //string rutaPDF = Properties.Settings.Default.FacturasGeneradas;
            string nombrePDF = "pdf.pdf";
            string mensajeEstado;
            bool Genera = false;

            //mensajeEstado = GenerarRIDE.CreatePDF2(dt, documento, rutaPDF, nombrePDF);

            //if (mensajeEstado == "True")
            //    System.Console.WriteLine("Se generó el PDF");
            //else
            //    System.Console.WriteLine(mensajeEstado);
            
            Genera = Generador.ReportePDF(documento, "FacturaEstandar", rutaPDF, nombrePDF);

            if (Genera)
                System.Console.WriteLine("Se generó el PDF");
            else
                System.Console.WriteLine("Ups! Algo pasó");

            // Metodos adicionales 
            //GenerarRIDE.ManipulatePdfWaterMark(@"C:\Users\guido\Desktop\CerverceriaSabai\generadasfacturas\archivotemporal.pdf", 
            // @"C:\Users\guido\Desktop\CerverceriaSabai\generadasfacturas\totales_temporal.pdf");

            System.Console.ReadKey();
            //** Activar para probar el proceso en memoria y envío por correo
            //MemoryStream ms = GenerarRIDE.CreatePDF1Stream();
            //ms.Position = 0;
            //GenerarRIDE.SendEmail(ms);


        }
    }
}
