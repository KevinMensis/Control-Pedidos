using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaLogica.Entidades.ControlCostos
{
    public class DatosCostos
    {
        public DatosCostos() { }

        public string factor { get; set; }
        public decimal porcentaje { get; set; }
        public decimal monto { get; set; }
    }
}