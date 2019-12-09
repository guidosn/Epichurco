using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epichurco.Benj8RIDES
{
    public class DetalleComprobante
    {
        public string iditem { get; set; }

        public string codigoPrincipal { get; set; }

        public string codigoAuxiliar { get; set; }

        public string descripcion { get; set; }

        public string detalleadicional1 { get; set; }
        public string detalleadicional2 { get; set; }
        public string detalleadicional3 { get; set; }

        public string codigoQB { get; set; }

        public string descripcionQB { get; set; }

        public decimal cantidad { get; set; }

        public decimal precioUnitario { get; set; }

        public decimal descuento { get; set; }

        public decimal totalSinImpueto { get; set; }

        public decimal iva { get; set; }

        public string bienservi { get; set; }

        public string idclase { get; set; }

        public string clase { get; set; }

        public List<DetalleImpuesto> impuestos { get; set; }

    }

    public class DetalleImpuesto
    {

        public decimal tarifa { get; set; }
        public decimal baseImponible { get; set; }
        public decimal valor { get; set; }

    }
}
