using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaLogica.Entidades.EficienciaEnergetica
{
    public class DatosRefrigeradores
    {
        public DatosRefrigeradores() { }

        public string TipoRefrigerador { get; set; }
        public int PiesCubicos { get; set; }
        public decimal ConsumoKWHMensual { get; set; }
    }
}