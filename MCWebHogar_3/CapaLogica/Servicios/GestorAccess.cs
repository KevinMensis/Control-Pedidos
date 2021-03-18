using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CapaDatos;
namespace CapaLogica.Servicios
{
    public class GestorAccess
    {
        public static void Conectividad(UserDB db)
        {           
            DataAccess.Conexion(db.dbSIAWIN, db.dbCEM);
        }

    }
}