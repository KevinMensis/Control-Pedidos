using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaLogica.Entidades.EficienciaEnergetica
{
    public class DatosHistoricos
    {
        public DatosHistoricos() { }

        public string dia { get; set; }
        public string puntoVenta { get; set; }
        public int cantidadPedidos { get; set; }
        public int cantidadDevolucion { get; set; }
    }
}