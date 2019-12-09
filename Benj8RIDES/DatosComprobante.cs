using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epichurco.Benj8RIDES
{
    public class DatosComprobante
    {
        public string Ruc { get; set; }

        public string claveAcceso { get; set; }

        public string numeroAutorizacion { get; set; }

        public DateTime fechaAutorizacion { get; set; }

        public string estado { get; set; }

        public string ambiente { get; set; }

        public string mensaje { get; set; }

        public string comprobante { get; set; }
    }
}
