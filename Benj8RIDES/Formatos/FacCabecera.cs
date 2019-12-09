using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epichurco.Benj8RIDES.Formatos
{
    public class FacCabecera
    {
        #region Propiedades
        public string Ruc { get; set; }

        public string claveAcceso { get; set; }

        public string numeroAutorizacion { get; set; }

        public DateTime fechaAutorizacion { get; set; }

        public string estado { get; set; }

        public string ambiente { get; set; }

        public string mensaje { get; set; }

        public string comprobante { get; set; }

        #endregion

        public string archivo { get; set; }
        public string tipo { get; set; }
        public string idvendor { get; set; }
        public string idqb { get; set; }
        public string idclasec { get; set; }
        public string clasec { get; set; }
        public string cxp { get; set; }
        public string civa { get; set; }
        public string memo { get; set; }
        public string direccion { get; set; }
        public string numretencion { get; set; }
        public string fecretencion { get; set; }
        public bool listo { get; set; }
        public bool subidoqb { get; set; }
        public bool exportar { get; set; }
        public string tipoformulario { get; set; }
        public string subebillPurcha { get; set; }

        #region infoTributaria
        public string codDoc { get; set; }
        public string establecimiento { get; set; }
        public string ptoEmision { get; set; }
        public string secuencial { get; set; }
        public string ruc { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        #endregion
        #region infoFactura
        public string fechaEmision { get; set; }
        public string periodoFiscal { get; set; }
        public decimal totalSinImpuetos { get; set; }
        public decimal totalDescuento { get; set; }
        public decimal totalConImpuestos { get; set; }
        public decimal propina { get; set; }
        public decimal importeTotal { get; set; }
        #endregion
        #region infoTributaria
        public string motivo { get; set; }
        public decimal valorModificacion { get; set; }
        public string fechaEmisionDocSustento { get; set; }
        public string numDocModificado { get; set; }
        public string codDocModificado { get; set; }
        #endregion
        public List<FacDetalle> detalles { get; set; }

        public List<DetalleRetenciones> retenciones { get; set; }

    }

    public class DetalleRetenciones
    {

        public string cuentaFullName { get; set; }
        public string codigoRetencion { get; set; }
        public decimal baseImponible { get; set; }
        public decimal porcentajeRetener { get; set; }
        public decimal valorRetenido { get; set; }

        public string numDocSustento { get; set; }
        public string fechaEmisionDocSustento { get; set; }
        public string codDocSustento { get; set; }

        public string codigo { get; set; }
    }
}
