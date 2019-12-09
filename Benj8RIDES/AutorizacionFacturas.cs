// Basado en el codigo de Leandro Tuttini
// https://social.msdn.microsoft.com/Forums/es-ES/fe88048d-f803-481b-87b1-f1b2e43ec40f/consumo-de-servicios-web-sri-ecuador-para-facturacin-electrnica?forum=vcses
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epichurco.Benj8RIDES
{
    public class AutorizacionFacturas
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DatosComprobante ConsultarAutorizaciones(string claveAcceso)
        {
            return TraerRespuestaSri(claveAcceso);
        }

        private DatosComprobante TraerRespuestaSri(string claveAcceso)
        {
            var resultado = string.Empty;
            DatosComprobante datosTributarios = new DatosComprobante();
            string url = ConfigurationSettings.AppSettings.Get("url_sri");

            string xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ec=\"http://ec.gob.sri.ws.autorizacion\">";
            xml = xml + "<soapenv:Header/>";
            xml = xml + "<soapenv:Body>";
            xml = xml + "<ec:autorizacionComprobante>";
            xml = xml + "<claveAccesoComprobante>" + claveAcceso + "</claveAccesoComprobante>";
            xml = xml + "</ec:autorizacionComprobante>";
            xml = xml + "</soapenv:Body>";
            xml = xml + "</soapenv:Envelope>";

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "text/xml";

                //log.Debug("Clave acceso: " + claveAcceso);

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //log.Debug("Status: " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    resultado = reader.ReadToEnd();
                }

                resultado = WebUtility.HtmlDecode(resultado);
                //log.Debug("Resultado: " + resultado);
                response.Close();
                var caracterPrincipal = resultado.IndexOf('?') - 1;
                var caracterSecundario = resultado.LastIndexOf('?') + 2;
                if (caracterPrincipal > 0 && caracterSecundario > 0)
                {
                    resultado = resultado.Remove(caracterPrincipal, (caracterSecundario - caracterPrincipal));
                }
                resultado = "<?xml version=" + "\"1.0\"" + " encoding=" + "\"UTF-8\"" + "?>" + resultado;
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(resultado);
                System.Xml.XmlNode element = doc.SelectSingleNode("numeroAutorizacion");
                datosTributarios.claveAcceso = doc.GetElementsByTagName("claveAcceso")[0].InnerText;
                datosTributarios.numeroAutorizacion = doc.GetElementsByTagName("numeroAutorizacion")[0].InnerText;
                datosTributarios.fechaAutorizacion = Convert.ToDateTime(doc.GetElementsByTagName("fechaAutorizacion")[0].InnerText);
                datosTributarios.estado = doc.GetElementsByTagName("estado")[0].InnerText;
                datosTributarios.comprobante = doc.GetElementsByTagName("comprobante")[0].InnerXml;
                //log.Debug("Estado: " + doc.GetElementsByTagName("estado")[0].InnerText);

            }
            catch (Exception e)
            {
                //log.Error(e);
            }

            return datosTributarios;
        }

    }
}
