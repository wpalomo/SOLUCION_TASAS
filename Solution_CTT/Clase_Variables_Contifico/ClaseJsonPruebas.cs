using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution_CTT.Clase_Variables_Contifico
{
    public class ClaseJsonPruebas
    {
        public void ver()
        {
            string sJson;

            sJson = "";
            sJson += "{" + Environment.NewLine;
            sJson += "\"id\": 31232," + Environment.NewLine;
            sJson += "\"fecha_hora_venta\": \"2019-12-19T13:10:00-05:00\"," + Environment.NewLine;
            sJson += "\"numero_documento\": \"\"," + Environment.NewLine;
            sJson += "\"clave_acceso\": null," + Environment.NewLine;
            sJson += "\"numero_documento_tasa\": \"001-001-000000007\"," + Environment.NewLine;
            sJson += "\"clave_acceso_tasa\": \"1912201901099223657400110010010000000071404488312\"," + Environment.NewLine;
            sJson += "\"total_tasas\": 0.5," + Environment.NewLine;
            sJson += "\"viaje\": 11002615," + Environment.NewLine;
            sJson += "\"forma_de_pago\": 1," + Environment.NewLine;
            sJson += "\"cliente\": {" + Environment.NewLine;
            sJson += "\"identificacion\": \"1717644551\"," + Environment.NewLine;
            sJson += "\"id\": 10641153," + Environment.NewLine;
            sJson += "\"nombre\": \"ELVIS GEOVANNI GUAIGUA AGUALSACA\"," + Environment.NewLine;
            sJson += "\"correo\": \"\"," + Environment.NewLine;
            sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
            sJson += "\"direccion\": null," + Environment.NewLine;
            sJson += "\"telefono\": null," + Environment.NewLine;
            sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
            sJson += "\"extranjero\": false," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.737187-05:00\"" + Environment.NewLine;
            sJson += "}," + Environment.NewLine;
            sJson += "\"boletos\": [" + Environment.NewLine;
            sJson += "{" + Environment.NewLine;
            sJson += "\"id\": 34931," + Environment.NewLine;
            sJson += "\"asiento\": 6," + Environment.NewLine;
            sJson += "\"asiento_nombre\": \"6\"," + Environment.NewLine;
            sJson += "\"nivel\": 1," + Environment.NewLine;
            sJson += "\"valor\": 8," + Environment.NewLine;
            sJson += "\"localidad_embarque\": 1," + Environment.NewLine;
            sJson += "\"tipo_cliente\": 1," + Environment.NewLine;
            sJson += "\"parada_embarque\": null," + Environment.NewLine;
            sJson += "\"parada_destino\": 846," + Environment.NewLine;
            sJson += "\"pasajero\": {" + Environment.NewLine;
            sJson += "\"identificacion\": \"1755971478\"," + Environment.NewLine;
            sJson += "\"id\": 13630902," + Environment.NewLine;
            sJson += "\"nombre\": \"KATHERIN NICOLE ENRIQUEZ JIMENEZ\"," + Environment.NewLine;
            sJson += "\"correo\": \"contabilidad@expressatenas.com.ec\"," + Environment.NewLine;
            sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
            sJson += "\"direccion\": null," + Environment.NewLine;
            sJson += "\"telefono\": null," + Environment.NewLine;
            sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
            sJson += "\"extranjero\": false," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.809424-05:00\"" + Environment.NewLine;
            sJson += "}," + Environment.NewLine;
            sJson += "\"tasa\": \"6413802695\"," + Environment.NewLine;
            sJson += "\"estado\": 1," + Environment.NewLine;
            sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.822454-05:00\"" + Environment.NewLine;
            sJson += "}," + Environment.NewLine;

            sJson += "{" + Environment.NewLine;
            sJson += "\"id\": 34930," + Environment.NewLine;
            sJson += "\"asiento\": 5," + Environment.NewLine;
            sJson += "\"asiento_nombre\": \"5\"," + Environment.NewLine;
            sJson += "\"nivel\": 1," + Environment.NewLine;
            sJson += "\"valor\": 8," + Environment.NewLine;
            sJson += "\"localidad_embarque\": 1," + Environment.NewLine;
            sJson += "\"tipo_cliente\": 1," + Environment.NewLine;
            sJson += "\"parada_embarque\": null," + Environment.NewLine;
            sJson += "\"parada_destino\": 846," + Environment.NewLine;
            sJson += "\"pasajero\": {" + Environment.NewLine;
            sJson += "\"identificacion\": \"1717644551\"," + Environment.NewLine;
            sJson += "\"id\": 10641153," + Environment.NewLine;
            sJson += "\"nombre\": \"ELVIS GEOVANNI GUAIGUA AGUALSACA\"," + Environment.NewLine;
            sJson += "\"correo\": \"contabilidad@expressatenas.com.ec\"," + Environment.NewLine;
            sJson += "\"tipo_cliente\": \"N\"," + Environment.NewLine;
            sJson += "\"direccion\": null," + Environment.NewLine;
            sJson += "\"telefono\": null," + Environment.NewLine;
            sJson += "\"tipo_identificacion\": \"CED\"," + Environment.NewLine;
            sJson += "\"extranjero\": false," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.772417-05:00\"" + Environment.NewLine;
            sJson += "}," + Environment.NewLine;
            sJson += "\"tasa\": \"0497329342\"," + Environment.NewLine;
            sJson += "\"estado\": 1," + Environment.NewLine;
            sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.788847-05:00\"" + Environment.NewLine;
            sJson += "}" + Environment.NewLine;

            sJson += "]," + Environment.NewLine;
            sJson += "\"uuid\": \"fbd6f704-cf59-4ed1-b74f-5a5864309147\"," + Environment.NewLine;
            sJson += "\"estado\": 1," + Environment.NewLine;
            sJson += "\"estado_nombre\": \"Vendido\"," + Environment.NewLine;
            sJson += "\"offline\": false," + Environment.NewLine;
            sJson += "\"emision\": null," + Environment.NewLine;
            sJson += "\"cooperativa\": 101," + Environment.NewLine;
            sJson += "\"destino\": \"QUITO\"," + Environment.NewLine;
            sJson += "\"is_active\": true," + Environment.NewLine;
            sJson += "\"is_enable\": true," + Environment.NewLine;
            sJson += "\"actualizacion\": \"2019-12-19T13:11:04.743249-05:00\"" + Environment.NewLine;
            sJson += "}";
        }
    }
}