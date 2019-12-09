using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Epichurco.Benj8RIDES.Formatos
{
    public class Generador
    {
        //public static String DEST = "results/zugferd/pdf/basic%05d.pdf";
        //public static String ICC = "resources/color/sRGB_CS_profile.icm";
        //public static String REGULAR = "resources/fonts/OpenSans-Regular.ttf";
        //public static String BOLD = "resources/fonts/OpenSans-Bold.ttf";
        //public static String NEWLINE = "\n";
        public static String NAMEFONT = Path.Combine(Application.StartupPath, "Fonts\\Megan June.otf");
        public static String NAMEFONTCODE = Path.Combine(Application.StartupPath, "Fonts\\code.xero.ttf");
        public iTextSharp.text.Font boldTableFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
        public iTextSharp.text.Font boldTableFontSize7 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
        public iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL);

        //// Estandar
        public static BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
        iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf, 7, 0, BaseColor.BLACK);
        //// Personalizada
        public static BaseFont bfp = BaseFont.CreateFont(NAMEFONT, BaseFont.CP1250, BaseFont.EMBEDDED);
        iTextSharp.text.Font fontTextP = new iTextSharp.text.Font(bfp, 7, 0, BaseColor.BLACK);

        private bool debug = false;
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

       

        /// <summary>
        /// /Moon
        /// </summary>
        /// <param name="Comprobante"></param>
        /// <param name="formato"></param>
        /// <param name="rutaPDF"></param>
        /// <param name="nombrePDF"></param>
        /// <returns></returns>


        public bool ReportePDF(FacCabecera Comprobante, string formato, string rutaPDF, string nombrePDF)
        {


            bool Resultado = true;
            FileStream fs = new FileStream(rutaPDF + nombrePDF, FileMode.Create);
            Document documento = new Document(iTextSharp.text.PageSize.A4, 30f, 30f, 30f, 30f);
            PdfWriter pw = PdfWriter.GetInstance(documento, fs);
           

            documento.Open();

           
            string programaAReader = @"C:\Program Files (x86)\Adobe\Reader 9.0\Reader\AcroRd32.exe";

            switch (formato)
            {
                case "FacturaEstandar":
                    #region FacturaEstandar

                   
                    var imagepath = Comprobante.archivo;

                    using (FileStream fsLogo = new FileStream(imagepath, FileMode.Open))
                    {
                        iTextSharp.text.Image png = 
                        iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fsLogo), System.Drawing.Imaging.ImageFormat.Png);
                        png.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                        png.SetAbsolutePosition(41, 1010);
                        png.ScaleAbsoluteHeight(100);
                        png.ScaleAbsoluteWidth(365);
                        png.Alignment = iTextSharp.text.Image.UNDERLYING;
                        documento.Add(png);
                    }

                    //Creamos un párrafo nuevo llamado "vacio1" para espaciar los elementos.
                    Paragraph vacio1 = new Paragraph();
                    vacio1.Add("\n");
                    documento.Add(vacio1);

                    Paragraph saltolinea1 = new Paragraph();
                    saltolinea1.Add("\n");
                    documento.Add(saltolinea1);

                    Hashtable headerData = new Hashtable();
                    headerData.Add("{razonSocial}", "documento.razonSocial");
                    headerData.Add("{fechaEmision", "documento.fechaEmision");
                    headerData.Add("{ruc}", "documento.ruc");


                    PdfPTable tableHeader = new PdfPTable(4);
                    //Bind and instance with properties set
                    tableHeader.TableEvent = new TopBottomTableBorderMaker(BaseColor.BLACK, 0.5f);
                    //Datos de porcentaje a la tabla (tamaño ancho).
                    //tableHeader.SetWidthPercentage(new float[] { 15, 20, 10, 10 }, documento.PageSize);
                    //Datos del ancho de cada columna.
                    tableHeader.SetWidths(new float[] { 6.70f, 8.70f, 3.70f, 4.70f });
                    tableHeader.WidthPercentage = 100;
                    //The rest is the same

                    var cell = new PdfPCell(new Phrase("Razón Social/Nombres Apellidos:", boldTableFont));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
                    cell.Border = 0;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderWidthLeft = .5f;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderWidthRight = .5f;
                    tableHeader.AddCell(cell);
                    tableHeader.AddCell(new Paragraph(Comprobante.razonSocial, bodyFont));

                    cell = new PdfPCell(new Phrase("Identificacion:", boldTableFont));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
                    cell.Border = 0;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderWidthLeft = .5f;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderWidthRight = .5f;
                    tableHeader.AddCell(cell);
                    tableHeader.AddCell(new Paragraph(Comprobante.ruc, bodyFont));

                    tableHeader.AddCell(new Paragraph("Fecha de emisión:", boldTableFont));
                    tableHeader.AddCell(new Paragraph(Comprobante.fechaEmision, bodyFont));

                    cell = new PdfPCell(new Phrase("Guía de remisión:", boldTableFont));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
                    cell.Border = 0;
                    cell.BorderColorLeft = BaseColor.BLACK;
                    cell.BorderWidthLeft = .5f;
                    cell.BorderColorRight = BaseColor.BLACK;
                    cell.BorderWidthRight = .5f;
                    tableHeader.AddCell(cell);
                    tableHeader.AddCell(new Paragraph(""));
                    documento.Add(tableHeader);

                    saltolinea1 = new Paragraph();
                    saltolinea1.Add("\n");
                    documento.Add(saltolinea1);
                    /*  Detalles */
                    // ...
                    PdfPTable table = new PdfPTable(8);
                    table.WidthPercentage = 100;
                    PdfPCell[] cells = new PdfPCell[] {
                                    new PdfPCell(GetCell("Cod.Principal",1)), // metodo local en esta misma clase
                                    new PdfPCell(GetCell("Cod.Auxiliar",1)),
                                    new PdfPCell(GetCell("Cantidad",1)),
                                    new PdfPCell(GetCell("Descripcion",1)),
                                    new PdfPCell(GetCell("Detalle Adicional",1)),
                                    new PdfPCell(GetCell("Precio Unitario",1)),
                                    new PdfPCell(GetCell("Descuento",1)),
                                    new PdfPCell(GetCell("Precio Total",1)),
                    };
                    PdfPRow row = new PdfPRow(cells);
                    table.Rows.Add(row);
                    /* Items */
                    foreach (FacDetalle fila in Comprobante.detalles)
                    {

                        PdfPCell[] cellsfila = new PdfPCell[] {
                                    new PdfPCell(GetCell(fila.codigoPrincipal.ToString(),0)), 
                                    new PdfPCell(GetCell(fila.codigoAuxiliar.ToString(),0)),
                                    new PdfPCell(GetCell(fila.cantidad.ToString(),0)),
                                    new PdfPCell(GetCell(fila.descripcion.ToString(),0)),
                                    new PdfPCell(GetCell(fila.detalleadicional1.ToString(),0)),
                                    new PdfPCell(GetCell(fila.precioUnitario.ToString(),0)),
                                    new PdfPCell(GetCell(fila.descuento.ToString(),0)),
                                    new PdfPCell(GetCell(fila.totalSinImpueto.ToString(),0)), };
                        PdfPRow rowfila = new PdfPRow(cellsfila);
                        table.Rows.Add(rowfila);
                    }
                    documento.Add(table);
                    saltolinea1 = new Paragraph();
                    saltolinea1.Add("\n");
                    documento.Add(saltolinea1);


                    /* Totales */
                    // Header part
                    PdfPTable tableTotales = new PdfPTable(2);
                    tableTotales.WidthPercentage = 100;
                    tableTotales.SetWidths(new int[] { 50, 50 });
                    // first cell
                    PdfPTable table1 = new PdfPTable(1);
                    table1.DefaultCell.MinimumHeight = 30;
                    table1.AddCell("Address 1");
                    table1.AddCell("Address 2");
                    table1.AddCell("Address 3");
                    tableTotales.AddCell(new PdfPCell(table1));
                    // second cell
                    PdfPTable table2 = new PdfPTable(2);
                    table2.AddCell("Date");
                    table2.AddCell("Place");
                    PdfPCell cell10 = new PdfPCell(new Phrase("References"));
                    cell10.MinimumHeight=40;
                    cell10.Colspan=2;
                    table2.AddCell(cell10);
                    cell10 = new PdfPCell(new Phrase("destination"));
                    cell10.Colspan=2;
                    table2.AddCell(cell10);
                    tableTotales.AddCell(new PdfPCell(table2));
                    // second row
                    cell10 = new PdfPCell();
                    cell10.Colspan=2;
                    cell10.MinimumHeight = 16;
                    tableTotales.AddCell(cell10);
                    documento.Add(tableTotales);

                    #endregion
                    break;

                case "FacturaPersonalizada":

                    break;


                default:
                    break;
            }


            documento.Close();

            #region AbrePDF
            if (programaAReader != null || programaAReader.Trim() != "")
            {
                if (Debug)
                {
                    Console.WriteLine("Showing PDF -> " + rutaPDF + nombrePDF);
                }
                System.Diagnostics.Process.Start(programaAReader, rutaPDF + nombrePDF);
            }
            #endregion

            return Resultado;
        }


        public PdfPCell GetCell(string texto,int tipoFuente)
        {
            PdfPCell cell;

            if (tipoFuente == 1)
            {
                cell = new PdfPCell(new Phrase(texto, boldTableFontSize7)); // cabecera en negrita
                cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            }
            else
            { 
                cell = new PdfPCell(new Phrase(texto, bodyFont));           // detalles sin negrita
                cell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
            }

            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 1;
            cell.BorderColorLeft = BaseColor.BLACK;
            cell.BorderWidthLeft = .1f;
            cell.BorderColorRight = BaseColor.BLACK;
            cell.BorderWidthRight = .1f;
            cell.BorderColorBottom = BaseColor.BLACK;
            cell.BorderWidthBottom = .1f;
            cell.BorderColorTop= BaseColor.BLACK;
            cell.BorderWidthTop = .1f;
            return cell;
        }
        public bool ReportePDFGenerico(string formatoXML, string rutaLogo, string rutaPDF)
        {
            bool Resultado = true;

            Hashtable headerData = new Hashtable();
            Hashtable bodyData = new Hashtable();
            Hashtable footerData = new Hashtable();



            var pdfTemplate = new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp(formatoXML);

            //  parameters load
            headerData.Add("{titreDocument}", "DOCUMENT \nTITLE");
            headerData.Add("{logoUrl}", rutaLogo);
            footerData.Add("{titreDocument}", "Document Title");

            // data load
            var firstTable = new TableData
            {
                HeadData = new Hashtable(),
                LoopData = new List<Hashtable>(),
                FootData = new Hashtable()
            };

            DateTime debut = new DateTime(2016, 1, 1);
            for (int i = 0; i < 100; i++)
            {
                var donnees1 = new Hashtable
                {
                    {"{Date}", debut.AddDays(i)},
                    {"{Centre}", "Centre 1"},
                    {"{Frais}", 5},
                    {"{Nombre}", "200,00"},
                    {"{Base}", "5,00"},
                    {"{Montant}", i}
                };
                firstTable.LoopData.Add(donnees1);
            }

            firstTable.FootData.Add("{Total}", 250.5);
            bodyData.Add("{FirstTable}", firstTable);
            bodyData.Add("{SecondTable}", firstTable);

            // pdf generation
            //pdfTemplate.Draw(headerData,bodyData, footerData);

            pdfTemplate.Draw(headerData, firstTable.LoopData, bodyData, footerData);

            // save file locally
            string fileDirectory = rutaPDF;
            string fileName = "MultipleTables-" + string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + ".pdf";
            using (var filePdf = new FileStream(fileDirectory + fileName, FileMode.Create))
            {
                using (MemoryStream stream = pdfTemplate.Close())
                {
                    byte[] content = stream.ToArray();
                    filePdf.Write(content, 0, content.Length);
                }
            }



            return Resultado;
        }

   } 



    public class TableData
    {
        /// <summary>
        /// List of dynamic columns, will be added to the end of table 
        /// Data related to dynamics columns needs to be in data fields like for static columns
        /// </summary>
        public List<DynamicColumnDefinition> DynamicColumns { get; set; }

        /// <summary>
        /// table header data
        /// </summary>
        public Hashtable HeadData { get; set; }
        /// <summary>
        /// table data. Table won't show if null
        /// </summary>
        public List<Hashtable> LoopData { get; set; }
        /// <summary>
        /// data for footer
        /// </summary>
        public Hashtable FootData { get; set; }
    }

    public class DynamicColumnDefinition
    {
        /// <summary>
        /// Width for this column, must be >= 1
        /// </summary>
        public int CellWidth { get; set; } = 1;

        /// <summary>
        /// define the template for header ex:
        /// "<tablecell border="Top, Bottom" backgroundcolor="#9BCFF9"><textbox text = "Montant" align="right"></textbox></tablecell>"
        /// </summary>
        public string HeaderTemplate { get; set; }

        /// <summary>
        /// define the template for data cells
        /// </summary>
        public string DataTemplate { get; set; }

        /// <summary>
        /// define the template for footer cells
        /// </summary>
        public string FooterTemplate { get; set; }
    }

}

