using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaLogica.Entidades.ControlPedidos
{
    public class DatosHistoricos
    {
        public DatosHistoricos() { }

        public string dia { get; set; }
        public string semana { get; set; }
        public string puntoVenta { get; set; }
        public string plantaProduccion { get; set; }
        public int cantidadPedidos { get; set; }
        public int cantidadDespacho { get; set; }
        public int cantidadRecibido { get; set; }
        public int cantidadDevolucion { get; set; }
        public int cantidadDesecho { get; set; }
        public int cantidadEmpaque { get; set; }
        public int cantidadInsumo { get; set; }
        public int cantidadFacturas { get; set; }
        public int montoPedidos { get; set; }
        public int montoDespacho { get; set; }
        public int montoRecibido { get; set; }
        public int montoEmpaque { get; set; }
        public int montoInsumo { get; set; }
        public decimal precioUnitario { get; set; }
    }
}