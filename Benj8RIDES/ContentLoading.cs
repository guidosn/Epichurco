using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace Epichurco.Benj8RIDES
{
    public static class ContentLoading
    {
        // Debe enviarse el parámetro para crear el directorio de formatos
        public static void GetPDFEmbedded() 
        {
            
            CopiarRecurso(Assembly.GetExecutingAssembly(), "Formatos.modelo_facturaFinal.pdf", 
                CrearCarpetaTemporal("modelo_facturaFinal.pdf"));

        }

        public static void CopiarRecurso(Assembly pAssembly, string pNombreRecurso, string pRuta)
        {
            using (Stream s = pAssembly.GetManifestResourceStream(pAssembly.GetName().Name + "." + pNombreRecurso))
            {
                if (s == null)
                {
                    throw new Exception("No se puede encontrar el recurso '" + pNombreRecurso + "'");
                }

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                using (BinaryWriter sw = new BinaryWriter(File.Open(pRuta, FileMode.Create)))
                {
                    sw.Write(buffer);
                }
                                
            }
        }

        public static string CrearCarpetaTemporal(string texto)
        {
            //obtenemos la carpeta y ejecutable de nuestra aplicación
            string rutaFichero = Environment.GetCommandLineArgs()[0];
            //obtenemos sólo la carpeta (quitamos el ejecutable)
            string carpeta = Path.GetDirectoryName(rutaFichero);
            //Montamos la carpeta y el fichero temporal con el
            //primer parámetro que es el código de solicitud
            //rutaFichero = Path.Combine(carpeta, "factura_" + Environment.GetCommandLineArgs()[1] + ".inc");
            try
            {
                //si no existe la carpeta temporal la creamos
                if (!(Directory.Exists(carpeta)))
                {
                    Directory.CreateDirectory(carpeta);
                }

                if (Directory.Exists(carpeta))
                {
                    rutaFichero = Path.Combine(carpeta, texto);
                    //Creamos el fichero temporal y
                    //añadimos el texto pasado como parámetro
                    //System.IO.StreamWriter ficheroTemporal =
                    //    new System.IO.StreamWriter(rutaFichero);
                    //ficheroTemporal.WriteLine(texto);
                    //ficheroTemporal.Close();
                }
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " +
                         "crear el fichero temporal:" +
                         Environment.NewLine + Environment.NewLine +
                         rutaFichero + Environment.NewLine +
                         Environment.NewLine + errorC.Message,
                         "Error al crear fichero temporal",
                         MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return rutaFichero;
        }

    }
}
