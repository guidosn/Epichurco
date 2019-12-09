using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epichurco.Benj8RIDES.Formatos;

namespace TestBenj8
{
    class Program
    {
        
        static void Main(string[] args)
        {

            FacCabecera documento = new FacCabecera();
            documento.detalles = new List<FacDetalle>();

            documento.numeroAutorizacion = "2808201901179174314800120010010002033920020339210";
            documento.ambiente = "PRODUCCION";
            documento.mensaje = "NORMAL";
            documento.archivo = @"C:\Users\guido\Desktop\CerverceriaSabai\LogoComputerEC.png";
            documento.claveAcceso = "2808201901179174314800120010010002033920020339210";
            documento.Ruc = "1714194451001";
            documento.codDoc = "001-001-000203392";
            documento.nombreComercial = "ComputerEC";
            documento.direccion = "Homero Salas OE5-148 y Altar";

            documento.razonSocial = "Pablo Rueda";
            documento.fechaEmision = "11-09-2019";
            documento.ruc = "1001001001001";

            FacDetalle linea = new FacDetalle();
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

            FacDetalle linea1 = new FacDetalle();
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

            FacDetalle linea2 = new FacDetalle();
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

            FacDetalle linea3 = new FacDetalle();
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

            string rutaPDF = @"C:\Users\guido\Desktop\CerverceriaSabai\generadasfacturas\";

            string nombrePDF = "ModeloEstandar.pdf";

            //*** Aquí envío a llamar a la función principal que genera el PDF
            Generador gen = new Generador();
            if (gen.ReportePDF(documento, "FacturaEstandar",rutaPDF,nombrePDF))
                System.Console.WriteLine("Se generó el PDF");
            else
                System.Console.WriteLine("Error en el PDF");


        }

      
    }
}
