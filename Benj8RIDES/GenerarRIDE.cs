using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Epichurco.Benj8RIDES
{
    public class GenerarRIDE
    {

       
        static float altotal;

        public static string CreatePDF1(DatosComprobante dt, Comprobante comprobante,string rutaPDF, string nombrePDF)
        {
            //*** Copia el recurso embebido en un directorio temporal , cuando se le envíe la plantilla del pdf en la dll
            // ContentLoading.GetPDFEmbedded(); 
            //*** Esto hay que activar cuando el pdf esté compilado en la librería

            try
            {
                var boldTableFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                //*** Configuración de directorios para los diferentes formatos
                string carpetaPrincipal = Properties.Settings.Default.PathDefecto;
                string carpetaFormatosFacturas = Properties.Settings.Default.DeFacturas;
                string formatoPDFCabecera = Properties.Settings.Default.EsteFormatoFacturaCabecera;
                string formatoPDFDetalles = Properties.Settings.Default.EsteFormatoFacturaDetalles;
                string formatoPDFTotales = Properties.Settings.Default.EsteFormatoFacturaTotales;
                string carpetaGeneradasFacturas = Properties.Settings.Default.FacturasGeneradas;

                string rutaCompleta = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFCabecera;
                string rutaCompleta_1 = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFDetalles;
                string rutaCompleta_2 = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFTotales;

                string nombreFacturaGenerada = Properties.Settings.Default.ArchivoTemporal;
                string nuevaFacturaCompleta = carpetaPrincipal + carpetaGeneradasFacturas + @"\" + nombreFacturaGenerada;
                // estos dos archivos, se eliminan al terminar el proceso
                string detalletemporal = carpetaPrincipal + carpetaGeneradasFacturas + @"\detalle_temporal.pdf";
                string totalestemporal = carpetaPrincipal + carpetaGeneradasFacturas + @"\totales_temporal.pdf";

                //*** Esto se activa cuando el pdf está dentro de la librería
                //string fileNameExisting = ContentLoading.CrearCarpetaTemporal("modelo_facturaFinal.pdf");
                //*** Va junto con la opción de arriba

                string fileNameExisting = rutaCompleta;
                string fileNameExisting_1 = rutaCompleta_1;
                string fileNameExisting_2 = rutaCompleta_2;

                //*** Archivo resultante temporal
                string fileNameNew = nuevaFacturaCompleta;
                //*** estos dos archivos, se eliminan al terminar el proceso
                string fileNameNew_1 = detalletemporal;
                string fileNameNew_2 = totalestemporal;

                //*** Proceso de estampado de la cabecera del documento
                using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
                using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                {
                    // Open existing PDF
                    PdfReader pdfReader = new PdfReader(existingFileStream);
                    Rectangle pagesize = pdfReader.GetPageSize(1);
                    // PdfStamper, which will create
                    PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);
                    var form = stamper.AcroFields;
                    //var fieldKeys = form.Fields.Keys;
                    var imagepath = comprobante.archivo;

                    using (FileStream fs = new FileStream(imagepath, FileMode.Open))
                    {
                        var png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                        png.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                        png.SetAbsolutePosition(41, 1010);
                        png.ScaleAbsoluteHeight(100);
                        png.ScaleAbsoluteWidth(365);
                        PdfContentByte over = null;
                        over = stamper.GetOverContent(1); // 1 = página
                        over.AddImage(png);
                    }

                    //*** mapeo de datos de la factura ESTAMPADO DE LOS DATOS EN LA PLANTILLA
                    //*** TECNOLOGÍA ACROFIELDS
                    form.SetField("RUC", dt.Ruc);
                    form.SetField("NROFACTURA", comprobante.codDoc);
                    form.SetField("NROAUTORIZACION", dt.numeroAutorizacion);
                    form.SetField("AMBIENTE", dt.ambiente);
                    form.SetField("EMISION", dt.estado);
                    form.SetField("CLAVEACCESONUMEROS", dt.numeroAutorizacion);

                    //***mapeo de datos de la empresa
                    form.SetField("NOMBREEMPRESA", "ComputerEC");
                    form.SetField("DIRECCION1EMPRESA", "Calle Homero Salas OE5-148 y Altar");
                    form.SetField("DIRECCION2EMPRESA", "Sector del Antiguo Aeropuerto");
                    form.SetField("DESCRIPCION1EMPRESA", "Teléfonos: 0983833901  0991324900");
                    form.SetField("DESCRIPCION2EMPRESA", "Horarios: Lun-Vie 08:00 a 18:00");

                    //***mapeo de los datos del cliente
                    //***ESTOS DATOS 
                    //form.SetField("RazonSocial", comprobante.Comprobante.razonSocial);
                    //form.SetField("FechaEmision", comprobante.Comprobante.fechaEmision);
                    //form.SetField("Identificacion", comprobante.Comprobante.ruc);
                    #region Detalles


                    PdfPTable itemTable = new PdfPTable(10);
                    itemTable.HorizontalAlignment = 0;
                    itemTable.WidthPercentage = 146;
                    itemTable.SpacingBefore = 10f;
                    itemTable.SpacingAfter = 10f;
                    itemTable.SetWidths(new float[] { 3f, 3f, 2f, 6f, 4f, 4f, 4f, 3f, 3f, 3f });  // then set the column's __relative__ widths
                    itemTable.DefaultCell.Border = Rectangle.BOX;


                    // VALIDACION DE LOS CAMPOS QUE SE GENERAN EN EL PDF

                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Cod.Principal", boldTableFont)));
                    cell1.HorizontalAlignment = 1;
                    itemTable.AddCell(cell1);

                    PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Cod.Auxiliar", boldTableFont)));
                    cell2.HorizontalAlignment = 1;
                    itemTable.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Cantidad", boldTableFont)));
                    cell3.HorizontalAlignment = 1;
                    itemTable.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Descripcion", boldTableFont)));
                    cell4.HorizontalAlignment = 1;
                    itemTable.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Det.Adicional1", boldTableFont)));
                    cell5.HorizontalAlignment = 1;
                    itemTable.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Det.Adicional2", boldTableFont)));
                    cell6.HorizontalAlignment = 1;
                    itemTable.AddCell(cell6);


                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Det.Adicional3", boldTableFont)));
                    cell7.HorizontalAlignment = 1;
                    itemTable.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("ValorUnit", boldTableFont)));
                    cell8.HorizontalAlignment = 1;
                    itemTable.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Descuento", boldTableFont)));
                    cell9.HorizontalAlignment = 1;
                    itemTable.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("PrecioTotal", boldTableFont)));
                    cell10.HorizontalAlignment = 1;
                    itemTable.AddCell(cell10);

                    itemTable.HeaderRows = 1;

                    foreach (DetalleComprobante fila in comprobante.detalles)
                    {

                        itemTable.AddCell(fila.codigoPrincipal.ToString());
                        itemTable.AddCell(fila.codigoAuxiliar.ToString());
                        itemTable.AddCell(fila.cantidad.ToString());
                        itemTable.AddCell(fila.descripcion.ToString());
                        itemTable.AddCell(fila.detalleadicional1.ToString());
                        itemTable.AddCell(fila.detalleadicional2.ToString());
                        itemTable.AddCell(fila.detalleadicional3.ToString());
                        itemTable.AddCell(fila.precioUnitario.ToString());
                        itemTable.AddCell(fila.descuento.ToString());
                        itemTable.AddCell(fila.totalSinImpueto.ToString());

                    }

                    ColumnText column = new ColumnText(stamper.GetOverContent(1));
                    Rectangle rectPage1 = new Rectangle(43, 36, 559, 856); // POSICIÓN DONDE INICIA LA TABLA
                    column.SetSimpleColumn(rectPage1);
                    column.AddElement(itemTable);
                    int pagecount = 1;
                    Rectangle rectPage2 = new Rectangle(43, 36, 559, 1000); // 1000 = POSICIÓN DEL TITULO DE LA TABLA EN CADA PÁGINA 
                    int status = column.Go();
                    while (ColumnText.HasMoreText(status))
                    {
                        status = triggerNewPage(stamper, pagesize, column, rectPage2, ++pagecount);
                    }
                    #endregion

                    itemTable.SpacingBefore = 20;
                    altotal = itemTable.TotalHeight;

                    stamper.FormFlattening = true;
                    //stamper.FreeTextFlattening = true;
                    stamper.Close();
                    pdfReader.Close();

                   

                }

                //*** Proceso de estampado de los totales del documento en otro PDF
                using (var existingFileStream = new FileStream(fileNameExisting_2, FileMode.Open))
                using (var newFileStream = new FileStream(fileNameNew_2, FileMode.Create))
                {
                    // Open existing PDF
                    PdfReader pdfReader = new PdfReader(existingFileStream);

                    // PdfStamper, which will create
                    PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);

                    var form = stamper.AcroFields;
                    var fieldKeys = form.Fields.Keys;


                    //*** mapeo de datos de la factura
                    form.SetField("SUBTOTAL12", comprobante.totalConImpuestos.ToString());
                    form.SetField("SUBTOTALCERO", "");
                    form.SetField("SUBTOTALNOIVA", "");
                    form.SetField("SUBTOTALEXCENTO", "");
                    form.SetField("SUBTOTALSINIMPUESTOS", comprobante.totalSinImpuetos.ToString());
                    form.SetField("TOTALDESCUENTO", comprobante.totalDescuento.ToString());
                    form.SetField("ICE", "");
                    form.SetField("VALORIVA", "");
                    form.SetField("IRBPNR", "");
                    form.SetField("PROPINA", comprobante.propina.ToString());
                    form.SetField("VALORTOTAL", comprobante.importeTotal.ToString());

                    stamper.FormFlattening = true;
                    //stamper.FreeTextFlattening = true;
                    stamper.Close();
                    pdfReader.Close();
                }

                // SE ACTIVA CUANDO SE REQUIERA HACER EL PROCESO EN MEMORIA         
                //List<byte[]> fbytes = new List<byte[]>();
                //MemoryStream ms = GenerarRIDE.CreatePDF1Stream(fileNameNew);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());
                //ms = GenerarRIDE.CreatePDF1Stream(fileNameNew_1);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());
                //ms = GenerarRIDE.CreatePDF1Stream(fileNameNew_2);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());

                //MemoryStream PDFData = new MemoryStream();
                ////*** Se guarda el archivo generado
                ////string fileNameNew = nuevaFacturaCompleta;
                //PDFData = new MemoryStream(concatAndAddContent(fbytes)); // une los 3 archivos 
                //PDFData.Position = 0;
                ////MemoryStream ms1 = GenerarRIDE.CreatePDF1Stream(final3);
                ////ms1.Position = 0;
                //SavePDFFile(final3, PDFData);

                // Merge de archivos
                #region MergePDF
                //altotal 856
                float y = -altotal-250;

                

                // obteniendo las rutas 
                string src = rutaPDF + "archivotemporal.pdf";
                string src1 = rutaPDF + "totales_temporal.pdf";
                string final = rutaPDF + nombrePDF;


                PdfReader reader = new PdfReader(src);
                PdfStamper stamper1 =
                    new PdfStamper(reader, new FileStream(final, FileMode.Create));

                PdfContentByte canvas = stamper1.GetUnderContent(1);

                PdfReader r;
                PdfImportedPage page;

                PdfReader s_reader = new PdfReader(src1);
                Rectangle pageSize = reader.GetPageSize(1);

                int n = reader.NumberOfPages;

                for (int i = 1; i <= n; i++)
                {
                    r = new PdfReader(src1);
                    page = stamper1.GetImportedPage(r, 1);
                    canvas.AddTemplate(page, 0, y); //float x   float y
                    stamper1.Writer.FreeReader(r);
                    r.Close();
                }

                stamper1.Close();
                s_reader.Close();
                reader.Close();
                return "True";
                #endregion

            }catch (Exception e0)
            {
                return "No se generó el PDF : " + e0.Message;
            }

        }

        public static string CreatePDF2(DatosComprobante dt, Comprobante comprobante, string rutaPDF, string nombrePDF)
        {
            //*** Copia el recurso embebido en un directorio temporal , cuando se le envíe la plantilla del pdf en la dll
            // ContentLoading.GetPDFEmbedded(); 
            //*** Esto hay que activar cuando el pdf esté compilado en la librería

            try
            {
                var boldTableFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
                //*** Configuración de directorios para los diferentes formatos
                string carpetaPrincipal = Properties.Settings.Default.PathDefecto;
                string carpetaFormatosFacturas = Properties.Settings.Default.DeFacturas;
                string formatoPDFCabecera = Properties.Settings.Default.EsteFormatoFacturaCabecera;
                string formatoPDFDetalles = Properties.Settings.Default.EsteFormatoFacturaDetalles;
                string formatoPDFTotales = Properties.Settings.Default.EsteFormatoFacturaTotales;
                string carpetaGeneradasFacturas = Properties.Settings.Default.FacturasGeneradas;

                string rutaCompleta = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFCabecera;
                string rutaCompleta_1 = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFDetalles;
                string rutaCompleta_2 = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDFTotales;

                string nombreFacturaGenerada = Properties.Settings.Default.ArchivoTemporal;
                string nuevaFacturaCompleta = carpetaPrincipal + carpetaGeneradasFacturas + @"\" + nombreFacturaGenerada;
                // estos dos archivos, se eliminan al terminar el proceso
                string detalletemporal = carpetaPrincipal + carpetaGeneradasFacturas + @"\detalle_temporal.pdf";
                string totalestemporal = carpetaPrincipal + carpetaGeneradasFacturas + @"\totales_temporal.pdf";

                //*** Esto se activa cuando el pdf está dentro de la librería
                //string fileNameExisting = ContentLoading.CrearCarpetaTemporal("modelo_facturaFinal.pdf");
                //*** Va junto con la opción de arriba

                string fileNameExisting = rutaCompleta;
                string fileNameExisting_1 = rutaCompleta_1;
                string fileNameExisting_2 = rutaCompleta_2;

                //*** Archivo resultante temporal
                string fileNameNew = nuevaFacturaCompleta;
                //*** estos dos archivos, se eliminan al terminar el proceso
                string fileNameNew_1 = detalletemporal;
                string fileNameNew_2 = totalestemporal;

                //*** Proceso de estampado de la cabecera del documento
                using (var existingFileStream = new FileStream(fileNameExisting, FileMode.Open))
                using (var newFileStream = new FileStream(fileNameNew, FileMode.Create))
                {
                    // Open existing PDF
                    PdfReader pdfReader = new PdfReader(existingFileStream);
                    Rectangle pagesize = pdfReader.GetPageSize(1);
                    // PdfStamper, which will create
                    PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);
                    var form = stamper.AcroFields;
                    //var fieldKeys = form.Fields.Keys;
                    var imagepath = comprobante.archivo;

                    using (FileStream fs = new FileStream(imagepath, FileMode.Open))
                    {
                        var png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), System.Drawing.Imaging.ImageFormat.Png);
                        png.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                        png.SetAbsolutePosition(41, 1010);
                        png.ScaleAbsoluteHeight(100);
                        png.ScaleAbsoluteWidth(365);
                        PdfContentByte over = null;
                        over = stamper.GetOverContent(1); // 1 = página
                        over.AddImage(png);
                    }

                    //*** mapeo de datos de la factura ESTAMPADO DE LOS DATOS EN LA PLANTILLA
                    //*** TECNOLOGÍA ACROFIELDS
                    form.SetField("RUC", dt.Ruc);
                    form.SetField("NROFACTURA", comprobante.codDoc);
                    form.SetField("NROAUTORIZACION", dt.numeroAutorizacion);
                    form.SetField("AMBIENTE", dt.ambiente);
                    form.SetField("EMISION", dt.estado);
                    form.SetField("CLAVEACCESONUMEROS", dt.numeroAutorizacion);

                    //***mapeo de datos de la empresa
                    form.SetField("NOMBREEMPRESA", "ComputerEC");
                    form.SetField("DIRECCION1EMPRESA", "Calle Homero Salas OE5-148 y Altar");
                    form.SetField("DIRECCION2EMPRESA", "Sector del Antiguo Aeropuerto");
                    form.SetField("DESCRIPCION1EMPRESA", "Teléfonos: 0983833901  0991324900");
                    form.SetField("DESCRIPCION2EMPRESA", "Horarios: Lun-Vie 08:00 a 18:00");

                    //***mapeo de los datos del cliente
                    //***ESTOS DATOS 
                    //form.SetField("RazonSocial", comprobante.Comprobante.razonSocial);
                    //form.SetField("FechaEmision", comprobante.Comprobante.fechaEmision);
                    //form.SetField("Identificacion", comprobante.Comprobante.ruc);
                    #region Detalles


                    PdfPTable itemTable = new PdfPTable(9);
                    itemTable.HorizontalAlignment = 0;
                    itemTable.WidthPercentage = 146;
                    itemTable.SpacingBefore = 10f;
                    itemTable.SpacingAfter = 10f;
                    itemTable.SetWidths(new float[] { 3f, /*3f,*/ 2f, 6f, 4f, 4f, 4f, 3f, 3f, 3f });  // then set the column's __relative__ widths
                    itemTable.DefaultCell.Border = Rectangle.BOX;


                    // VALIDACION DE LOS CAMPOS QUE SE GENERAN EN EL PDF

                    PdfPCell cell1 = new PdfPCell(new Phrase(new Chunk("Cod.Principal", boldTableFont)));
                    cell1.HorizontalAlignment = 1;
                    itemTable.AddCell(cell1);

                    //PdfPCell cell2 = new PdfPCell(new Phrase(new Chunk("Cod.Auxiliar", boldTableFont)));
                    //cell2.HorizontalAlignment = 1;
                    //itemTable.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase(new Chunk("Cantidad", boldTableFont)));
                    cell3.HorizontalAlignment = 1;
                    itemTable.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(new Chunk("Descripcion", boldTableFont)));
                    cell4.HorizontalAlignment = 1;
                    itemTable.AddCell(cell4);

                    PdfPCell cell5 = new PdfPCell(new Phrase(new Chunk("Det.Adicional1", boldTableFont)));
                    cell5.HorizontalAlignment = 1;
                    itemTable.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase(new Chunk("Det.Adicional2", boldTableFont)));
                    cell6.HorizontalAlignment = 1;
                    itemTable.AddCell(cell6);


                    PdfPCell cell7 = new PdfPCell(new Phrase(new Chunk("Det.Adicional3", boldTableFont)));
                    cell7.HorizontalAlignment = 1;
                    itemTable.AddCell(cell7);

                    PdfPCell cell8 = new PdfPCell(new Phrase(new Chunk("ValorUnit", boldTableFont)));
                    cell8.HorizontalAlignment = 1;
                    itemTable.AddCell(cell8);

                    PdfPCell cell9 = new PdfPCell(new Phrase(new Chunk("Descuento", boldTableFont)));
                    cell9.HorizontalAlignment = 1;
                    itemTable.AddCell(cell9);

                    PdfPCell cell10 = new PdfPCell(new Phrase(new Chunk("PrecioTotal", boldTableFont)));
                    cell10.HorizontalAlignment = 1;
                    itemTable.AddCell(cell10);

                    itemTable.HeaderRows = 1;

                    foreach (DetalleComprobante fila in comprobante.detalles)
                    {

                        itemTable.AddCell(fila.codigoPrincipal.ToString());
                        //itemTable.AddCell(fila.codigoAuxiliar.ToString());
                        itemTable.AddCell(fila.cantidad.ToString());
                        itemTable.AddCell(fila.descripcion.ToString());
                        itemTable.AddCell(fila.detalleadicional1.ToString());
                        itemTable.AddCell(fila.detalleadicional2.ToString());
                        itemTable.AddCell(fila.detalleadicional3.ToString());
                        itemTable.AddCell(fila.precioUnitario.ToString());
                        itemTable.AddCell(fila.descuento.ToString());
                        itemTable.AddCell(fila.totalSinImpueto.ToString());

                    }

                    ColumnText column = new ColumnText(stamper.GetOverContent(1));
                    Rectangle rectPage1 = new Rectangle(43, 36, 559, 856); // POSICIÓN DONDE INICIA LA TABLA
                    column.SetSimpleColumn(rectPage1);
                    column.AddElement(itemTable);
                    int pagecount = 1;
                    Rectangle rectPage2 = new Rectangle(43, 36, 559, 1000); // 1000 = POSICIÓN DEL TITULO DE LA TABLA EN CADA PÁGINA 
                    int status = column.Go();
                    while (ColumnText.HasMoreText(status))
                    {
                        status = triggerNewPage(stamper, pagesize, column, rectPage2, ++pagecount);
                    }
                    #endregion

                    itemTable.SpacingBefore = 20;
                    altotal = itemTable.TotalHeight;

                    stamper.FormFlattening = true;
                    //stamper.FreeTextFlattening = true;
                    stamper.Close();
                    pdfReader.Close();



                }

                //*** Proceso de estampado de los totales del documento en otro PDF
                using (var existingFileStream = new FileStream(fileNameExisting_2, FileMode.Open))
                using (var newFileStream = new FileStream(fileNameNew_2, FileMode.Create))
                {
                    // Open existing PDF
                    PdfReader pdfReader = new PdfReader(existingFileStream);

                    // PdfStamper, which will create
                    PdfStamper stamper = new PdfStamper(pdfReader, newFileStream);

                    var form = stamper.AcroFields;
                    var fieldKeys = form.Fields.Keys;


                    //*** mapeo de datos de la factura
                    form.SetField("SUBTOTAL12", comprobante.totalConImpuestos.ToString());
                    form.SetField("SUBTOTALCERO", "");
                    form.SetField("SUBTOTALNOIVA", "");
                    form.SetField("SUBTOTALEXCENTO", "");
                    form.SetField("SUBTOTALSINIMPUESTOS", comprobante.totalSinImpuetos.ToString());
                    form.SetField("TOTALDESCUENTO", comprobante.totalDescuento.ToString());
                    form.SetField("ICE", "");
                    form.SetField("VALORIVA", "");
                    form.SetField("IRBPNR", "");
                    form.SetField("PROPINA", comprobante.propina.ToString());
                    form.SetField("VALORTOTAL", comprobante.importeTotal.ToString());

                    stamper.FormFlattening = true;
                    //stamper.FreeTextFlattening = true;
                    stamper.Close();
                    pdfReader.Close();
                }

                // SE ACTIVA CUANDO SE REQUIERA HACER EL PROCESO EN MEMORIA         
                //List<byte[]> fbytes = new List<byte[]>();
                //MemoryStream ms = GenerarRIDE.CreatePDF1Stream(fileNameNew);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());
                //ms = GenerarRIDE.CreatePDF1Stream(fileNameNew_1);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());
                //ms = GenerarRIDE.CreatePDF1Stream(fileNameNew_2);
                //ms.Position = 0;
                //fbytes.Add(ms.ToArray());

                //MemoryStream PDFData = new MemoryStream();
                ////*** Se guarda el archivo generado
                ////string fileNameNew = nuevaFacturaCompleta;
                //PDFData = new MemoryStream(concatAndAddContent(fbytes)); // une los 3 archivos 
                //PDFData.Position = 0;
                ////MemoryStream ms1 = GenerarRIDE.CreatePDF1Stream(final3);
                ////ms1.Position = 0;
                //SavePDFFile(final3, PDFData);

                // Merge de archivos
                #region MergePDF
                //altotal 856
                float y = -altotal - 250;



                // obteniendo las rutas 
                string src = rutaPDF + "archivotemporal.pdf";
                string src1 = rutaPDF + "totales_temporal.pdf";
                string final = rutaPDF + nombrePDF;


                PdfReader reader = new PdfReader(src);
                PdfStamper stamper1 =
                    new PdfStamper(reader, new FileStream(final, FileMode.Create));

                PdfContentByte canvas = stamper1.GetUnderContent(1);

                PdfReader r;
                PdfImportedPage page;

                PdfReader s_reader = new PdfReader(src1);
                Rectangle pageSize = reader.GetPageSize(1);

                int n = reader.NumberOfPages;

                for (int i = 1; i <= n; i++)
                {
                    r = new PdfReader(src1);
                    page = stamper1.GetImportedPage(r, 1);
                    canvas.AddTemplate(page, 0, y); //float x   float y
                    stamper1.Writer.FreeReader(r);
                    r.Close();
                }

                stamper1.Close();
                s_reader.Close();
                reader.Close();
                return "True";
                #endregion

            }
            catch (Exception e0)
            {
                return "No se generó el PDF : " + e0.Message;
            }

        }
        #region TriggerNewPage
        //*** METODO PARA GENERAR NUEVAS PÁGINAS EN EL ESTAMPADO DE LA TABLA DETALLES
        public static int triggerNewPage(PdfStamper stamper, Rectangle pagesize, ColumnText column, Rectangle rect, int pagecount)
        {
            stamper.InsertPage(pagecount, pagesize);
            PdfContentByte canvas = stamper.GetOverContent(pagecount);
            //column.SetCanvas(canvas);
            column.Canvas = canvas;
            column.SetSimpleColumn(rect);
            return column.Go();
        }

        #endregion
        #region CreatePDF1Stream 
        //** archivo en memoria
        public static MemoryStream CreatePDF1Stream(string fileNameNew)
        {
            //*** Configuración de directorios para las facturas 
            string carpetaPrincipal = Properties.Settings.Default.PathDefecto;
            string carpetaFormatosFacturas = Properties.Settings.Default.DeFacturas;
            string formatoPDF = Properties.Settings.Default.EsteFormatoFacturaCabecera;
            string carpetaGeneradasFacturas = Properties.Settings.Default.FacturasGeneradas;
            string rutaCompleta = carpetaPrincipal + carpetaFormatosFacturas + @"\" + formatoPDF;
            string nombreFacturaGenerada = Properties.Settings.Default.ArchivoTemporal;
            string nuevaFacturaCompleta = carpetaPrincipal + carpetaGeneradasFacturas + @"\" + nombreFacturaGenerada;

            //*** MemoryStream
            MemoryStream PDFData = new MemoryStream();
            //*** Se guarda el archivo generado
            //string fileNameNew = nuevaFacturaCompleta;
            PDFData = new MemoryStream(System.IO.File.ReadAllBytes(fileNameNew));

            return PDFData;
        }
        #endregion

        #region SavePDFFile
        //*** grabar el archivo en disco
        public static void SavePDFFile(string cReportName, MemoryStream pdfStream)
        {

            //Check file exists, delete  
            if (File.Exists(cReportName))
            {
                File.Delete(cReportName);
            }

            byte[] bytes = new byte[pdfStream.Length];
            pdfStream.Read(bytes, 0, (int)pdfStream.Length);
            File.WriteAllBytes(cReportName, bytes);
            pdfStream.Close();


        }
        #endregion

        #region SendEmail
        //*** envía por correo el archivo en memoria
        public static void SendEmail(MemoryStream ms)
        {
            const string fromPassword = "THOMAS0111ws";
            const string subject = "Enviando PDF";
            const string body = "Este PDf es de ejemplo generado desde C#";
            MailAddress _From = new MailAddress("guidosn.gp@gmail.com", "Guido Pilaquinga");
            MailAddress _To = new MailAddress("guidosn@hotmail.com", "Guido Pilaquinga");
            MailMessage email = new MailMessage(_From, _To);
            Attachment attach = new Attachment(ms, new System.Net.Mime.ContentType("application/pdf"));
            email.Attachments.Add(attach);
            SmtpClient mailSender = new SmtpClient("smtp.gmail.com");
            mailSender.Port = 587;
            mailSender.EnableSsl = true;
            mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
            //mailSender.UseDefaultCredentials = false;
            mailSender.Credentials = new NetworkCredential(_From.Address, fromPassword);
            mailSender.Timeout = 20000;
            email.Subject = subject;
            email.Body = body;
            mailSender.Send(email);
            ms.Close();
        }
        #endregion

        #region MergePDF
        //*** une los archivos, está actualmente aplicado
        public static void MergePDF(string src, string src1, string final)
        {
            //altotal 856
            float y = -302;
                                  
            final = Properties.Settings.Default.PathDefecto + @"generadasfacturas\"+final;

            PdfReader reader = new PdfReader(src);
            PdfStamper stamper =
                new PdfStamper(reader, new FileStream(final, FileMode.Create));

            PdfContentByte canvas = stamper.GetUnderContent(1);

            PdfReader r;
            PdfImportedPage page;
            
            PdfReader s_reader = new PdfReader(src1);
            Rectangle pageSize = reader.GetPageSize(1);
                       
            int n = reader.NumberOfPages;
            
            for (int i = 1; i <= n; i++)
            {
                r = new PdfReader(src1);
                page = stamper.GetImportedPage(r, 1);
                canvas.AddTemplate(page, 0, y);
                stamper.Writer.FreeReader(r);
                r.Close();
            }

            stamper.Close();
        }

        #endregion

        #region PhraseCell
        //*** para aplicar formato a los datos de la tabla
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }
        #endregion

        #region Metadatos
        //*** para aplicar datos al arhcivo 
        private static void MetaDatos(PdfDocument doc)
        {
            // Setting Document properties e.g.
            // 1. Title
            // 2. Subject
            // 3. Keywords
            // 4. Creator
            // 5. Author
            // 6. Header
            doc.AddTitle("Hello World example");
            doc.AddSubject("This is an Example 4 of Chapter 1 of Book 'iText in Action'");
            doc.AddKeywords("Metadata, iTextSharp 5.4.4, Chapter 1, Tutorial");
            doc.AddCreator("iTextSharp 5.4.4");
            doc.AddAuthor("Debopam Pal");
            doc.AddHeader("Nothing", "No Header");
        }
        #endregion

        #region ConcatenarEnMemoria
        //*** concatena archivos pdf , todo en memoria
        public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();

                        //Loop through each byte array
                        foreach (var p in pdfByteContent)
                        {

                            //Create a PdfReader bound to that byte array
                            using (var reader = new PdfReader(p))
                            {

                                //Add the entire document instead of page-by-page
                                copy.AddDocument(reader);
                            }
                        }

                        doc.Close();
                    }
                }

                //Return just before disposing
                return ms.ToArray();
            }
        }
        #endregion

        #region UnirPDFDirectorio
        //*** se le envía el archivo resultante y la carpeta que contiene los archivos a unir
        public static void CreateMergedPDF(string targetPDF, string sourceDir)
        {
            using (FileStream stream = new FileStream(targetPDF, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfCopy pdf = new PdfCopy(pdfDoc, stream);
                pdfDoc.Open();

                var files = Directory.GetFiles(sourceDir);
                foreach (string file in files)
                {
                    if (file.Split('.').Last().ToUpper() == "PDF")
                    {
                        pdf.AddDocument(new PdfReader(file));
                    }
                }
                if (pdfDoc != null)
                    pdfDoc.Close();
            }
        }
        #endregion



        public static void ManipulatePdfWaterMark(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(dest, FileMode.Create)); // create?
            int n = reader.NumberOfPages;
            Rectangle pagesize;
            for (int i = 1; i <= n; i++)
            {
                PdfContentByte under = stamper.GetUnderContent(i);
                pagesize = reader.GetPageSize(i);
                float x = (pagesize.Left + pagesize.Right) / 2;
                float y = (pagesize.Bottom + pagesize.Top) / 2;
                PdfGState gs = new PdfGState();
                gs.FillOpacity = 0.3f;
                under.SaveState();
                under.SetGState(gs);
                under.SetRGBColorFill(200, 200, 0);
                ColumnText.ShowTextAligned(under, Element.ALIGN_CENTER,
                    new Phrase("Watermark", new Font(Font.FontFamily.HELVETICA, 120)),
                    x, y, 45);
                under.RestoreState();
            }
            stamper.Close();
            reader.Close();
        }
    }
      
}

